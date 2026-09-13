using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppleSpec;

public static class Sync
{
    // Eight at a time has never drawn a throttle from Apple; the ceiling is politeness, not throughput.
    private const int MaxInFlight = 8;

    private static readonly UTF8Encoding Utf8 = new(encoderShouldEmitUTF8Identifier: false);

    public static async Task<int> RunAsync(SpecPaths paths, CancellationToken cancellationToken)
    {
        var clock = Stopwatch.StartNew();
        var sources = new List<ManifestSource>();

        foreach (var framework in Program.Frameworks)
        {
            sources.Add(await SyncFrameworkAsync(paths, framework, cancellationToken).ConfigureAwait(false));
        }

        sources.AddRange(await OpenApi.SyncAsync(paths, cancellationToken).ConfigureAwait(false));

        Directory.CreateDirectory(paths.SpecDir);
        WriteFile(paths.ManifestFile, Utf8.GetBytes(JsonSerializer.Serialize(new Manifest { Sources = sources }, Json.Options) + "\n"));

        Console.WriteLine($"spec/ synced in {clock.Elapsed.TotalSeconds:F1}s — {sources.Sum(source => source.Pages)} pages across {sources.Count} sources.");

        return 0;
    }

    internal static int WriteDirectory(SpecPaths paths, string directory, SortedDictionary<string, byte[]> files)
    {
        Directory.CreateDirectory(directory);

        foreach (var (name, body) in files)
        {
            // The name comes from a page's own identifier, so a name that is not bare would write,
            // and later delete, outside the directory being synced.
            if (Path.GetFileName(name) != name || name.Length == 0)
            {
                throw new SpecFormatException($"{name} is not a file name that can live in {Path.GetFileName(directory)}/.");
            }

            WriteFile(Path.Combine(directory, name), body);
        }

        var persisted = paths.PersistedFiles.Select(Path.GetFullPath).ToHashSet(StringComparer.Ordinal);
        var removed = 0;

        foreach (var existing in Directory.EnumerateFiles(directory))
        {
            if (files.ContainsKey(Path.GetFileName(existing)))
            {
                continue;
            }

            // Sync owns this directory, so anything it did not just write is a page Apple retired
            // and the deletion is the point. A file this repository maintains is not that, and
            // deleting it would be silent data loss rather than a visible retirement.
            if (persisted.Contains(Path.GetFullPath(existing)))
            {
                throw new SpecLayoutException(
                    $"{existing} is maintained by this repository but sits in a directory `sync` rewrites, "
                    + "which would delete it. It belongs directly under spec/.");
            }

            File.Delete(existing);
            removed++;
        }

        return removed;
    }

    internal static string Digest(SortedDictionary<string, byte[]> files)
    {
        using var digest = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        foreach (var (name, body) in files)
        {
            digest.AppendData(Utf8.GetBytes(name));
            digest.AppendData(SHA256.HashData(body));
        }

        return Convert.ToHexString(digest.GetHashAndReset()).ToLowerInvariant();
    }

    // Fetches a framework's pages and normalizes every one of them before returning, so a caller
    // never writes a half-built directory that would read as Apple deleting the pages it never got to.
    internal static async Task<DocumentationSet> BuildDocumentationAsync(string framework, CancellationToken cancellationToken)
    {
        var slugs = await AppleDocs.ListSlugsAsync(framework, cancellationToken).ConfigureAwait(false);
        var pages = await FetchAllAsync(slugs, cancellationToken).ConfigureAwait(false);

        try
        {
            var files = new SortedDictionary<string, byte[]>(StringComparer.Ordinal);

            foreach (var slug in slugs)
            {
                var page = Normalizer.Normalize(pages[slug].RootElement, framework);
                var name = Normalizer.FileName(page);

                if (!files.TryAdd(name, Utf8.GetBytes(JsonSerializer.Serialize(page, Json.Options) + "\n")))
                {
                    throw new SpecFormatException($"Two pages of {framework} claim the file name {name}; the second is /documentation/{slug}.");
                }
            }

            var pageCount = files.Count;
            string? version = null;

            if (FindChangelogSlug(slugs) is { } changelogSlug)
            {
                var changelog = pages[changelogSlug].RootElement;

                files.Add("CHANGELOG.md", Utf8.GetBytes(Text(Changelog.Render(changelog))));
                version = Changelog.LatestVersion(changelog);
            }

            return new DocumentationSet(files, pageCount, version);
        }
        finally
        {
            foreach (var page in pages.Values)
            {
                page.Dispose();
            }
        }
    }

    private static async Task<ManifestSource> SyncFrameworkAsync(SpecPaths paths, string framework, CancellationToken cancellationToken)
    {
        var clock = Stopwatch.StartNew();
        var documentation = await BuildDocumentationAsync(framework, cancellationToken).ConfigureAwait(false);

        {
            var files = documentation.Files;
            var pageCount = documentation.Pages;
            var version = documentation.Version;

            var removed = WriteDirectory(paths, paths.FrameworkDir(framework), files);
            var meta = await AppleDocs.HeadAsync(AppleDocs.IndexUrl(framework), cancellationToken).ConfigureAwait(false);

            Console.WriteLine($"{framework}: {pageCount} pages{(removed > 0 ? $", {removed} removed" : "")} in {clock.Elapsed.TotalSeconds:F1}s.");

            return new ManifestSource
            {
                Name = framework,
                Kind = "documentation",
                Url = AppleDocs.IndexUrl(framework),
                Version = version,
                LastModified = meta.LastModified,
                ETag = meta.ETag,
                Pages = pageCount,
                Sha256 = Digest(files)
            };
        }
    }

    internal static async Task<Dictionary<string, JsonDocument>> FetchAllAsync(IReadOnlyList<string> slugs, CancellationToken cancellationToken)
    {
        using var gate = new SemaphoreSlim(MaxInFlight);
        var fetched = new ConcurrentDictionary<string, JsonDocument>(StringComparer.Ordinal);

        var tasks = slugs.Select(async slug =>
        {
            await gate.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                fetched[slug] = await AppleDocs.GetPageAsync(slug, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                gate.Release();
            }
        }).ToArray();

        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch
        {
            // WhenAll completes only once every task has, so nothing is still writing to the dictionary here.
            foreach (var page in fetched.Values)
            {
                page.Dispose();
            }

            throw;
        }

        return new Dictionary<string, JsonDocument>(fetched, StringComparer.Ordinal);
    }

    // Apple names the changelog page per framework: "changelog" for one,
    // "retention-messaging-changelog" for another.
    private static string? FindChangelogSlug(IReadOnlyList<string> slugs) =>
        slugs.FirstOrDefault(slug =>
        {
            var leaf = slug[(slug.LastIndexOf('/') + 1)..];

            return leaf == "changelog" || leaf.EndsWith("-changelog", StringComparison.Ordinal);
        });

    private static string Text(string markdown) =>
        markdown.Replace("\r\n", "\n").TrimEnd('\n') + "\n";

    private static void WriteFile(string path, byte[] body)
    {
        if (File.Exists(path) && File.ReadAllBytes(path).AsSpan().SequenceEqual(body))
        {
            return;
        }

        File.WriteAllBytes(path, body);
    }
}
