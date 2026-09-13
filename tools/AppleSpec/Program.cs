using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppleSpec;

public static class Program
{
    // The server library alone spans three of these: its endpoints cover the App Store Server
    // API, the Retention Messaging API and the Advanced Commerce API.
    public static readonly string[] Frameworks =
    [
        "appstoreserverapi",
        "appstoreservernotifications",
        "retentionmessaging",
        "advancedcommerceapi"
    ];

    public static async Task<int> Main(string[] args)
    {
        var verb = args.Length > 0 ? args[0] : "";
        var paths = SpecPaths.Discover();

        var cancellation = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cancellation.Cancel();
        };

        try
        {
            switch (verb)
            {
                case "sync":
                    return await Sync.RunAsync(paths, cancellation.Token).ConfigureAwait(false);

                case "check":
                    return RunCheck(paths, Flag(args, "--report"));

                default:
                    return Usage();
            }
        }
        catch (SpecFormatException ex)
        {
            Console.Error.WriteLine($"Apple's documentation has a shape this tool does not handle yet.\n{ex.Message}");
            return 2;
        }
        catch (SpecLayoutException ex)
        {
            Console.Error.WriteLine($"This repository is laid out in a way this tool cannot work with.\n{ex.Message}");
            return 2;
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Cancelled.");
            return 130;
        }
    }

    private static int RunCheck(SpecPaths paths, string? reportPath)
    {
        var report = CheckReport.Merge(ServerCheck.Run(paths), ConnectCheck.Run(paths));
        var markdown = report.ToMarkdown();

        Console.Write(markdown);

        if (reportPath is not null)
        {
            File.WriteAllText(reportPath, markdown, new UTF8Encoding(false));
        }

        return report.Failures.Count > 0 ? 1 : 0;
    }

    private static string? Flag(string[] args, string name)
    {
        var index = Array.IndexOf(args, name);
        return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
    }

    private static int Usage()
    {
        Console.Error.WriteLine("""
                                Usage: dotnet run --project tools/AppleSpec -- <verb>

                                  sync                    Re-fetch Apple's documentation into spec/.
                                  check [--report FILE]   Compare spec/ against src/ and report what is not covered.
                                                          Exits non-zero when something fails rather than merely differs.
                                        [--update-baseline]
                                                          Rewrite spec/baseline.connect.json, which is what tells a route
                                                          Apple just added apart from one the library has long skipped.
                                """);

        return 64;
    }
}

public sealed class SpecPaths
{
    public required string RepoRoot { get; init; }

    public string SpecDir => Path.Combine(RepoRoot, "spec");
    public string SourceDir => Path.Combine(RepoRoot, "src");
    public string ManifestFile => Path.Combine(SpecDir, "manifest.json");

    public string ServerCoverageFile => Path.Combine(SpecDir, "coverage.server.json");
    public string ConnectBaselineFile => Path.Combine(SpecDir, "baseline.connect.json");

    // Files this repository maintains rather than fetches. None may live inside a framework
    // directory: `sync` rewrites those wholesale and deletes whatever it did not just write.
    public IReadOnlyList<string> PersistedFiles => [ManifestFile, ServerCoverageFile, ConnectBaselineFile];

    public string FrameworkDir(string framework) => Path.Combine(SpecDir, framework);

    public static SpecPaths Discover()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "AppStoreLibrary.sln")))
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException(
                "Run this from inside the repository; AppStoreLibrary.sln was not found above the executable.");
        }

        return new SpecPaths { RepoRoot = directory.FullName };
    }
}

// A page carrying a key, section kind or node type the normalizer does not model fails the sync
// rather than writing a snapshot that silently omits it.
public sealed class SpecFormatException(string message) : Exception(message);

// A file this repository maintains, sitting where `sync` would delete it. Nothing to do with what
// Apple published, so it is reported as the repository mistake it is rather than as a page shape.
public sealed class SpecLayoutException(string message) : Exception(message);

// A failure means the library is provably wrong or the checker has gone blind; uncovered surface
// is only a report, because both libraries cover Apple's surface selectively on purpose.
public sealed class CheckReport
{
    public required string Title { get; init; }
    public List<CheckItem> Failures { get; init; } = [];
    public List<CheckItem> Reports { get; init; } = [];
    public List<string> Notes { get; init; } = [];

    public static CheckReport Merge(params CheckReport[] reports)
    {
        var merged = new CheckReport { Title = "Coverage" };

        foreach (var report in reports)
        {
            merged.Failures.AddRange(report.Failures.Select(item =>
                item with { Subject = $"{report.Title}: {item.Subject}" }));
            merged.Reports.AddRange(report.Reports.Select(item =>
                item with { Subject = $"{report.Title}: {item.Subject}" }));
            merged.Notes.AddRange(report.Notes.Select(note => $"{report.Title}: {note}"));
        }

        return merged;
    }

    public string ToMarkdown()
    {
        var text = new StringBuilder();

        text.AppendLine("## Coverage");
        text.AppendLine();

        foreach (var note in Notes)
        {
            text.AppendLine($"- {note}");
        }

        if (Notes.Count > 0)
        {
            text.AppendLine();
        }

        Section(text, $"Failures ({Failures.Count})", Failures);
        Section(text, $"Reported ({Reports.Count})", Reports);

        return text.ToString();
    }

    private static void Section(StringBuilder text, string heading, List<CheckItem> items)
    {
        text.AppendLine($"### {heading}");
        text.AppendLine();

        if (items.Count == 0)
        {
            text.AppendLine("None.");
            text.AppendLine();
            return;
        }

        foreach (var group in items.GroupBy(item => item.Category))
        {
            text.AppendLine($"**{group.Key}**");
            text.AppendLine();

            foreach (var item in group)
            {
                text.AppendLine($"- `{item.Subject}` — {item.Detail}");
            }

            text.AppendLine();
        }
    }
}

public readonly record struct CheckItem(string Category, string Subject, string Detail);
