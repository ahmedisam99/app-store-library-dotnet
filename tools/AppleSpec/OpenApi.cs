using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppleSpec;

public static class OpenApi
{
    private const string Source = "https://developer.apple.com/sample-code/app-store-connect/app-store-connect-openapi-specification.zip";

    private const string Framework = "appstoreconnectapi";

    // Apple's OpenAPI document carries the exact shapes and not one line of prose: every one of its
    // operations and schemas has an empty description. The prose lives on the documentation site
    // instead, so both are fetched into the same directory and written in one pass.
    public static async Task<IReadOnlyList<ManifestSource>> SyncAsync(SpecPaths paths, CancellationToken cancellationToken)
    {
        var documentation = await Sync.BuildDocumentationAsync(Framework, cancellationToken).ConfigureAwait(false);
        var meta = await AppleDocs.HeadAsync(Source, cancellationToken).ConfigureAwait(false);
        var archive = await AppleDocs.GetBytesAsync(Source, cancellationToken).ConfigureAwait(false);

        using var stream = new MemoryStream(archive, writable: false);
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read);

        // The JSON member is currently named "openapi.oas (2).json" and the name is not stable,
        // so the sole JSON member is selected instead.
        var candidates = zip.Entries.Where(IsSpecification).ToList();

        if (candidates.Count != 1)
        {
            throw new SpecFormatException(
                $"Expected exactly one JSON member in {Source}, found {candidates.Count}. The archive holds: {string.Join(", ", zip.Entries.Select(entry => entry.FullName))}");
        }

        var document = Read(candidates[0]);

        // Apple ships it pretty-printed at a quarter of a million lines and it already diffs
        // cleanly; reformatting would turn every future release into one unreadable change.
        var specification = new SortedDictionary<string, byte[]>(StringComparer.Ordinal) { ["openapi.json"] = document };

        var files = new SortedDictionary<string, byte[]>(documentation.Files, StringComparer.Ordinal);

        if (!files.TryAdd("openapi.json", document))
        {
            throw new SpecFormatException($"A documentation page of {Framework} claims the file name openapi.json.");
        }

        var removed = Sync.WriteDirectory(paths, paths.FrameworkDir(Framework), files);

        Console.WriteLine($"{Framework}: {documentation.Pages} pages and openapi.json at {document.Length / 1024}KB"
            + $"{(removed > 0 ? $", {removed} removed" : "")}.");

        return
        [
            new ManifestSource
            {
                Name = Framework,
                Kind = "documentation",
                Url = AppleDocs.IndexUrl(Framework),
                Version = documentation.Version,
                LastModified = null,
                ETag = null,
                Pages = documentation.Pages,
                Sha256 = Sync.Digest(documentation.Files)
            },
            new ManifestSource
            {
                Name = Framework,
                Kind = "openapi",
                Url = Source,
                Version = ReadVersion(document),
                LastModified = meta.LastModified,
                ETag = meta.ETag,
                Pages = specification.Count,
                Sha256 = Sync.Digest(specification)
            }
        ];
    }

    private static bool IsSpecification(ZipArchiveEntry entry) =>
        entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
        // A zip built on a Mac carries a shadow copy of every member under __MACOSX, prefixed "._".
        && !entry.FullName.StartsWith("__MACOSX/", StringComparison.Ordinal)
        && !entry.Name.StartsWith("._", StringComparison.Ordinal);

    private static byte[] Read(ZipArchiveEntry entry)
    {
        using var source = entry.Open();
        using var buffer = new MemoryStream(entry.Length > 0 && entry.Length < int.MaxValue ? (int)entry.Length : 0);

        source.CopyTo(buffer);

        return buffer.ToArray();
    }

    private static string ReadVersion(byte[] document)
    {
        try
        {
            using var parsed = JsonDocument.Parse(document);

            if (parsed.RootElement.TryGetProperty("info", out var info)
                && info.TryGetProperty("version", out var version)
                && version.ValueKind == JsonValueKind.String)
            {
                return version.GetString() ?? "";
            }
        }
        catch (JsonException ex)
        {
            throw new SpecFormatException($"The specification inside {Source} is not valid JSON: {ex.Message}");
        }

        throw new SpecFormatException($"The specification inside {Source} carries no info.version.");
    }
}
