using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AppleSpec;

// A documented path the client does not implement never fails: 807 of those exist today and Apple
// adds more every release.
public static class ConnectCheck
{
    private const string Framework = "appstoreconnectapi";
    private const string ClientProject = "Enjna.AppStoreConnectApi";
    private const string DocumentationBase = "https://developer.apple.com/documentation/appstoreconnectapi/";

    private const string UpdateFlag = "--update-baseline";

    // Exempt: two upload helpers that PUT to absolute URLs Apple hands back, three pagination
    // walkers that follow an opaque cursor, one constructor shorthand, and Dispose.
    private static readonly string[] NonEndpointMethods =
    [
        "UploadAssetAsync",
        "ComputeSourceFileChecksum",
        "GetNextPageAsync",
        "EnumeratePagesAsync",
        "EnumerateResourcesAsync",
        "ForIndividualKey",
        "Dispose"
    ];

    // Apple's root type name → the class that models it, where the package chose another name.
    private static readonly Dictionary<string, string> RenamedTypes = new(StringComparer.Ordinal)
    {
        // The package models the reply a developer writes, Apple's CustomerReviewResponseV1, under the
        // plainer name. Apple's own CustomerReviewResponse is a response envelope the package doesn't model.
        ["CustomerReviewResponseV1"] = "CustomerReviewResponse"
    };

    // OpenAPI puts a shared parameters array beside the verbs under a path.
    private static readonly string[] HttpVerbs =
    [
        "get", "put", "post", "delete", "options", "head", "patch", "trace"
    ];

    private static readonly Regex PathParameter = new(@"\{[^}]*\}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex VersionedPath = new(@"^/v([0-9]+)(/.*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static CheckReport Run(SpecPaths paths)
    {
        var report = new CheckReport { Title = "App Store Connect" };
        var specFile = Path.Combine(paths.FrameworkDir(Framework), "openapi.json");

        if (!File.Exists(specFile))
        {
            report.Failures.Add(new CheckItem("Snapshot", $"spec/{Framework}/openapi.json", "not in the working tree; run `sync` before `check`."));
            return report;
        }

        using var document = JsonDocument.Parse(File.ReadAllBytes(specFile));

        if (!document.RootElement.TryGetProperty("paths", out var specPaths) || specPaths.ValueKind != JsonValueKind.Object)
        {
            report.Failures.Add(new CheckItem("Snapshot", $"spec/{Framework}/openapi.json", "carries no `paths` object; the snapshot is not an OpenAPI document."));
            return report;
        }

        var clientDir = Path.Combine(paths.SourceDir, ClientProject);

        if (!Directory.Exists(clientDir))
        {
            report.Failures.Add(new CheckItem("Snapshot", $"src/{ClientProject}", "not in the working tree; there is nothing to check."));
            return report;
        }

        var spec = ReadSpec(document.RootElement, specPaths);
        var pages = ReadPages(paths.FrameworkDir(Framework), report);
        MergeDeprecations(spec, pages);
        var surface = ConnectInventory.Read(clientDir);

        if (surface.Calls.Count == 0)
        {
            // Zero is never right, and reading it as "nothing implemented" would report all 966
            // documented paths as gaps. The walker has lost the client, not the client its endpoints.
            report.Failures.Add(new CheckItem("Call site the walker could not read", $"src/{ClientProject}", "no requests recovered at all; the walker no longer recognizes the client."));
            return report;
        }

        var updating = Environment.GetCommandLineArgs().Contains(UpdateFlag);
        var implemented = Resolve(surface, spec, report);
        var baseline = LoadBaseline(paths, report, updating);

        Summarize(report, spec, implemented, baseline);
        ReportGaps(report, spec, implemented, surface, baseline);
        CheckDeprecatedOperations(report, spec, implemented);
        CheckDeprecatedModels(report, pages, ConnectInventory.ReadModels(clientDir));

        if (updating)
        {
            WriteBaseline(paths, report, spec, implemented);
        }

        return report;
    }

    private static Dictionary<Route, ConnectCall> Resolve(ConnectSurface surface, SpecSnapshot spec, CheckReport report)
    {
        var implemented = new Dictionary<Route, ConnectCall>();
        var undocumented = new List<CheckItem>();
        var links = new List<CheckItem>();
        var duplicates = new List<CheckItem>();

        foreach (var call in surface.Calls.OrderBy(call => call.Method, StringComparer.Ordinal))
        {
            var route = new Route(call.Verb, Normalize(call.Path));
            var expected = SeeAlsoFor(route);

            // Checked before anything else can skip this call site: the link is a second, separately
            // hand-written encoding of the route, and a disagreement is a real bug in one of the two.
            if (call.SeeAlso is null)
            {
                links.Add(new CheckItem("Documentation link", call.Method, $"no `<seealso>` route link; expected {expected}."));
            }
            else if (!string.Equals(call.SeeAlso, expected, StringComparison.Ordinal))
            {
                links.Add(new CheckItem("Documentation link", call.Method, $"points at {call.SeeAlso} but issues {route}, which is {expected}."));
            }

            if (implemented.TryGetValue(route, out var first))
            {
                // The whole mapping rests on one method per route; two of them means the counts
                // below describe something other than the client's surface.
                duplicates.Add(new CheckItem("Two methods, one route", route.ToString(), $"`{first.Method}` and `{call.Method}` both issue it."));
                continue;
            }

            implemented.Add(route, call);

            if (!spec.Operations.ContainsKey(route))
            {
                undocumented.Add(new CheckItem("Route Apple no longer documents", route.ToString(), $"`{call.Method}` in {call.File} calls it; the specification does not list it."));
            }
        }

        foreach (var group in surface.Calls.GroupBy(call => call.Method, StringComparer.Ordinal).Where(group => group.Count() > 1))
        {
            duplicates.Add(new CheckItem("Method issuing more than one request", group.Key, $"issues {group.Count()} requests: {string.Join(", ", group.Select(call => $"{call.Verb} {call.Path}"))}."));
        }

        report.Failures.AddRange(undocumented);

        foreach (var problem in surface.Unreduced)
        {
            report.Failures.Add(new CheckItem("Call site the walker could not read", problem, "every request must be recoverable or the counts below are a lie."));
        }

        report.Failures.AddRange(links);
        report.Failures.AddRange(duplicates);

        return implemented;
    }

    private static void Summarize(CheckReport report, SpecSnapshot spec, Dictionary<Route, ConnectCall> implemented, ConnectBaseline? baseline)
    {
        var implementedPaths = implemented.Keys.Select(route => route.Path).Distinct(StringComparer.Ordinal).Count();

        report.Notes.Add($"{implemented.Count} operations across {implementedPaths} paths implemented; specification {spec.Version} documents {spec.Operations.Count} operations across {spec.Paths.Count} paths.");

        if (baseline is null)
        {
            report.Notes.Add("no baseline was read, so nothing below is marked new in this release.");
            return;
        }

        if (!string.Equals(baseline.SpecVersion, spec.Version, StringComparison.Ordinal))
        {
            report.Notes.Add($"specification version moved: {baseline.SpecVersion} → {spec.Version}.");
        }

        var recorded = baseline.Implemented.Select(entry => $"{entry.Verb} {entry.Path} {entry.Method}").ToHashSet(StringComparer.Ordinal);
        var live = implemented.Select(pair => $"{pair.Key.Verb} {pair.Key.Path} {pair.Value.Method}").ToHashSet(StringComparer.Ordinal);

        if (!recorded.SetEquals(live))
        {
            report.Notes.Add($"implemented baseline is stale: {live.Except(recorded).Count()} added, {recorded.Except(live).Count()} removed. Rerun with `{UpdateFlag}` and commit the diff.");
        }
    }

    private static void ReportGaps(CheckReport report, SpecSnapshot spec, Dictionary<Route, ConnectCall> implemented, ConnectSurface surface, ConnectBaseline? baseline)
    {
        var known = baseline?.SpecOperations.ToHashSet(StringComparer.Ordinal);
        var implementedPaths = implemented.Keys.Select(route => route.Path).ToHashSet(StringComparer.Ordinal);

        string Since(Route route) => known is null || known.Contains(route.ToString()) ? "" : " New in this release.";

        foreach (var operation in spec.Operations
            .Where(operation => !operation.Value.Deprecated
                && implementedPaths.Contains(operation.Key.Path)
                && !implemented.ContainsKey(operation.Key))
            .OrderBy(operation => operation.Key.Path, StringComparer.Ordinal)
            .ThenBy(operation => operation.Key.Verb, StringComparer.Ordinal))
        {
            report.Reports.Add(new CheckItem("Missing verb on an implemented path", operation.Key.ToString(), $"`{operation.Value.OperationId}`.{Since(operation.Key)}"));
        }

        var unimplemented = spec.Paths.Where(path => !implementedPaths.Contains(path)).ToList();
        var live = unimplemented.Where(spec.IsLive).OrderBy(path => path, StringComparer.Ordinal).ToList();
        var accounted = new HashSet<string>(StringComparer.Ordinal);

        foreach (var path in live)
        {
            var sibling = LowerVersionSibling(path, implementedPaths);

            if (sibling is null)
            {
                continue;
            }

            accounted.Add(path);
            report.Reports.Add(new CheckItem("Newer version of an implemented path", path, $"newer form of implemented `{sibling}`.{SincePath(spec, known, path)}"));
        }

        foreach (var path in live.Where(path => !accounted.Contains(path)))
        {
            var parent = Parent(path);

            if (parent is null || !implementedPaths.Contains(parent))
            {
                continue;
            }

            accounted.Add(path);
            report.Reports.Add(new CheckItem("Unimplemented path under an implemented one", path, $"hangs off implemented `{parent}`.{SincePath(spec, known, path)}"));
        }

        // Paths whose every operation is deprecated are left out, or the number stays permanently
        // inflated by surface Apple has already retired.
        var rest = live.Where(path => !accounted.Contains(path)).ToList();
        var retired = unimplemented.Count - live.Count;
        var arrived = known is null ? "" : $" {rest.Count(path => IsNewPath(spec, known, path))} new in this release.";

        report.Reports.Add(new CheckItem(
            "Everything else Apple documents",
            $"{rest.Count} paths",
            $"neither a newer form of nor adjacent to anything implemented; {retired} fully deprecated paths excluded.{arrived} Not enumerated."));

        foreach (var method in surface.NonEndpointMethods.Where(method => Array.IndexOf(NonEndpointMethods, method) < 0).OrderBy(method => method, StringComparer.Ordinal))
        {
            report.Reports.Add(new CheckItem("Public method issuing no documented request", method, "either it should call an endpoint, or it belongs on this check's exemption list."));
        }
    }

    // Keeping a deprecated operation is fine, since Apple ships the replacement before it retires the
    // original. Keeping it without [Obsolete] is not: callers would build on it with no warning.
    private static void CheckDeprecatedOperations(CheckReport report, SpecSnapshot spec, Dictionary<Route, ConnectCall> implemented)
    {
        var marked = 0;

        foreach (var (route, call) in implemented
            .OrderBy(pair => pair.Key.Path, StringComparer.Ordinal)
            .ThenBy(pair => pair.Key.Verb, StringComparer.Ordinal))
        {
            if (!spec.Operations.TryGetValue(route, out var operation))
            {
                continue;
            }

            if (operation.Deprecated && call.Obsolete)
            {
                marked++;
            }
            else if (operation.Deprecated)
            {
                report.Failures.Add(new CheckItem("Deprecated operation not marked [Obsolete]", route.ToString(),
                    $"`{call.Method}` calls it; Apple deprecated it{Since(operation.DeprecatedAt)}.{Replacement(operation.Summary)}"));
            }
            else if (call.Obsolete)
            {
                // Wrong in the other direction: callers are steered off something Apple still supports.
                report.Failures.Add(new CheckItem("[Obsolete] on a current operation", route.ToString(),
                    $"`{call.Method}` is marked [Obsolete], but Apple documents `{operation.OperationId}` as current."));
            }
        }

        if (marked > 0)
        {
            report.Notes.Add($"{marked} implemented operations are ones Apple deprecated, each marked [Obsolete].");
        }
    }

    // Apple deprecates a type on its own page and leaves the nested Data, Attributes and Relationships
    // pages beneath it unflagged, so a page counts as deprecated when it or any enclosing page is.
    private static void CheckDeprecatedModels(CheckReport report, Dictionary<string, SpecPage> pages, Dictionary<string, ConnectModel> models)
    {
        Deprecation? DeprecationOf(string title)
        {
            for (var name = title; ; name = name[..name.LastIndexOf('.')])
            {
                if (pages.TryGetValue(name, out var page) && page.Deprecated is not null)
                {
                    return page.Deprecated;
                }

                if (!name.Contains('.'))
                {
                    return null;
                }
            }
        }

        foreach (var (title, page) in pages.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            if (ClassNameFor(title) is not { } className || !models.TryGetValue(className, out var model))
            {
                continue;
            }

            var deprecation = DeprecationOf(title);

            if (deprecation is not null && !model.Obsolete)
            {
                report.Failures.Add(new CheckItem("Deprecated type not marked [Obsolete]", model.Name,
                    $"models `{title}`, which Apple deprecated{Since(deprecation.At)}.{Replacement(deprecation.Summary?.Text)}"));
            }
            else if (deprecation is null && model.Obsolete)
            {
                report.Failures.Add(new CheckItem("[Obsolete] on a current type", model.Name,
                    $"is marked [Obsolete], but Apple documents `{title}` as current."));
            }

            if (model.Obsolete)
            {
                continue;
            }

            foreach (var member in page.Properties ?? [])
            {
                if (member.Deprecated && model.Properties.TryGetValue(member.Name, out var obsolete) && !obsolete)
                {
                    report.Failures.Add(new CheckItem("Deprecated property not marked [Obsolete]", $"{model.Name}.{member.Name}",
                        $"Apple marks `{title}.{member.Name}` deprecated."));
                }
            }
        }
    }

    // Apple's nested type titles join their parts with dots, and the package's class names join the
    // same parts without them. A renamed root carries its nested types along; a title whose root a
    // rename took over belongs to a type the package doesn't model.
    private static string? ClassNameFor(string title)
    {
        var dot = title.IndexOf('.');
        var root = dot < 0 ? title : title[..dot];
        var nested = dot < 0 ? "" : title[dot..].Replace(".", "");

        if (RenamedTypes.TryGetValue(root, out var renamed))
        {
            return renamed + nested;
        }

        return RenamedTypes.ContainsValue(root) ? null : root + nested;
    }

    private static string Since(string? at) => at is null ? "" : $" in {at}";

    private static string Replacement(string? summary) => string.IsNullOrWhiteSpace(summary) ? "" : $" Apple: \"{summary.Trim()}\"";

    private static string SincePath(SpecSnapshot spec, HashSet<string>? known, string path) =>
        IsNewPath(spec, known, path) ? " New in this release." : "";

    private static bool IsNewPath(SpecSnapshot spec, HashSet<string>? known, string path) =>
        known is not null && !spec.OperationsOn(path).Any(route => known.Contains(route.ToString()));

    // A bijection only because Apple names every path parameter id and no path carries two: a
    // template with two holes normalizes to something no documented path can equal.
    private static string Normalize(string template)
    {
        var path = PathParameter.Replace(template, "{id}");

        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return path.Length > 1 && path.EndsWith('/') ? path[..^1] : path;
    }

    private static string SeeAlsoFor(Route route) =>
        DocumentationBase + $"{route.Verb}-{route.Path.TrimStart('/').Replace("{id}", "_id_").Replace("/", "-")}".ToLowerInvariant();

    private static string? LowerVersionSibling(string path, HashSet<string> implementedPaths)
    {
        var match = VersionedPath.Match(path);

        if (!match.Success)
        {
            return null;
        }

        for (var version = int.Parse(match.Groups[1].Value) - 1; version > 0; version--)
        {
            var sibling = $"/v{version}{match.Groups[2].Value}";

            if (implementedPaths.Contains(sibling))
            {
                return sibling;
            }
        }

        return null;
    }

    private static string? Parent(string path)
    {
        var cut = path.LastIndexOf('/');
        return cut > 0 ? path[..cut] : null;
    }

    // Apple's OpenAPI document leaves `deprecated` off some operations its documentation deprecates,
    // every one of 4.4.1's in-app purchase and subscription metadata endpoints among them, so the flag
    // is read from both.
    // Keyed by title, which is the endpoint's route for a REST page and the type's name for a data page.
    private static Dictionary<string, SpecPage> ReadPages(string directory, CheckReport report)
    {
        var pages = new Dictionary<string, SpecPage>(StringComparer.Ordinal);
        var restPages = 0;
        var dataPages = 0;

        foreach (var file in Directory.EnumerateFiles(directory, "*.json").OrderBy(file => file, StringComparer.Ordinal))
        {
            var name = Path.GetFileName(file);
            var isRest = name.StartsWith("rest-", StringComparison.Ordinal);

            if (!isRest && !name.StartsWith("data-", StringComparison.Ordinal))
            {
                continue;
            }

            SpecPage? page;

            try
            {
                page = JsonSerializer.Deserialize<SpecPage>(File.ReadAllText(file), Json.Options);
            }
            catch (JsonException exception)
            {
                report.Failures.Add(new CheckItem("Blind", name, $"unreadable snapshot page: {exception.Message}"));
                continue;
            }

            if (page is null)
            {
                continue;
            }

            if (isRest)
            {
                restPages++;

                foreach (var endpoint in page.Endpoints ?? [])
                {
                    pages[new Route(endpoint.Method.ToUpperInvariant(), Normalize(endpoint.Path)).ToString()] = page;
                }
            }
            else
            {
                dataPages++;
                pages[page.Title] = page;
            }
        }

        // Without either kind, every deprecation Apple states only in its documentation goes unseen.
        if (restPages == 0)
        {
            report.Failures.Add(new CheckItem("Blind", $"spec/{Framework}", "holds no REST endpoint pages; run `sync` before `check`."));
        }

        if (dataPages == 0)
        {
            report.Failures.Add(new CheckItem("Blind", $"spec/{Framework}", "holds no data type pages; run `sync` before `check`."));
        }

        return pages;
    }

    private static void MergeDeprecations(SpecSnapshot spec, Dictionary<string, SpecPage> pages)
    {
        foreach (var (route, operation) in spec.Operations.ToList())
        {
            if (pages.TryGetValue(route.ToString(), out var page) && page.Deprecated is { } deprecation)
            {
                spec.Operations[route] = operation with
                {
                    Deprecated = true,
                    DeprecatedAt = deprecation.At,
                    Summary = deprecation.Summary?.Text
                };
            }
        }
    }

    private static SpecSnapshot ReadSpec(JsonElement root, JsonElement paths)
    {
        var spec = new SpecSnapshot
        {
            Version = root.TryGetProperty("info", out var info) && info.TryGetProperty("version", out var version)
                ? version.GetString() ?? "unversioned"
                : "unversioned"
        };

        foreach (var path in paths.EnumerateObject())
        {
            spec.Paths.Add(path.Name);

            foreach (var operation in path.Value.EnumerateObject().Where(entry => Array.IndexOf(HttpVerbs, entry.Name) >= 0))
            {
                var identifier = operation.Value.TryGetProperty("operationId", out var id) ? id.GetString() ?? "" : "";
                var deprecated = operation.Value.TryGetProperty("deprecated", out var flag) && flag.ValueKind == JsonValueKind.True;
                var route = new Route(operation.Name.ToUpperInvariant(), path.Name);

                spec.Operations[route] = new SpecOperation(identifier, deprecated, null, null);

                if (!spec.ByPath.TryGetValue(path.Name, out var routes))
                {
                    spec.ByPath[path.Name] = routes = [];
                }

                routes.Add(route);
            }
        }

        return spec;
    }

    // Without it every gap reads as pre-existing, so the run stays green on the release that added
    // one. That is the check going blind, which is a failure here exactly as it is on the server side.
    private static ConnectBaseline? LoadBaseline(SpecPaths paths, CheckReport report, bool updating)
    {
        var file = paths.ConnectBaselineFile;
        var name = $"spec/{Path.GetFileName(file)}";

        if (!File.Exists(file))
        {
            if (!updating)
            {
                report.Failures.Add(new CheckItem("Blind", name,
                    $"no committed baseline, so a route Apple just added cannot be told from one long skipped; write one with `check {UpdateFlag}` and commit it."));
            }

            return null;
        }

        try
        {
            var baseline = JsonSerializer.Deserialize<ConnectBaseline>(File.ReadAllText(file));

            if (baseline is not null)
            {
                return baseline;
            }

            report.Failures.Add(new CheckItem("Blind", name,
                $"the baseline holds no object; rewrite it with `check {UpdateFlag}`."));
        }
        catch (JsonException exception)
        {
            report.Failures.Add(new CheckItem("Blind", name,
                $"the baseline will not parse ({exception.Message}); rewrite it with `check {UpdateFlag}`."));
        }

        return null;
    }

    // The specification's own routes at the pinned version are what tell "new in this release"
    // apart from "always existed, we chose not to"; without them every run reports the same 807.
    private static void WriteBaseline(SpecPaths paths, CheckReport report, SpecSnapshot spec, Dictionary<Route, ConnectCall> implemented)
    {
        var baseline = new ConnectBaseline
        {
            SpecVersion = spec.Version,
            Implemented = implemented
                .OrderBy(pair => pair.Key.Path, StringComparer.Ordinal)
                .ThenBy(pair => pair.Key.Verb, StringComparer.Ordinal)
                .Select(pair => new ConnectBaselineEndpoint
                {
                    Verb = pair.Key.Verb,
                    Path = pair.Key.Path,
                    Method = pair.Value.Method
                })
                .ToList(),
            SpecOperations = spec.Operations.Keys
                .Select(route => route.ToString())
                .OrderBy(route => route, StringComparer.Ordinal)
                .ToList()
        };

        var file = paths.ConnectBaselineFile;
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, JsonSerializer.Serialize(baseline, Json.Options) + "\n", new UTF8Encoding(false));

        report.Notes.Add($"rewrote spec/{Path.GetFileName(file)}: {baseline.Implemented.Count} implemented routes, {baseline.SpecOperations.Count} documented at {spec.Version}.");
    }

    private readonly record struct Route(string Verb, string Path)
    {
        public override string ToString() => $"{Verb} {Path}";
    }

    private readonly record struct SpecOperation(string OperationId, bool Deprecated, string? DeprecatedAt, string? Summary);

    private sealed class SpecSnapshot
    {
        public required string Version { get; init; }
        public Dictionary<Route, SpecOperation> Operations { get; init; } = [];
        public HashSet<string> Paths { get; init; } = new(StringComparer.Ordinal);

        public Dictionary<string, List<Route>> ByPath { get; init; } = new(StringComparer.Ordinal);

        public IEnumerable<Route> OperationsOn(string path) =>
            ByPath.TryGetValue(path, out var routes) ? routes : [];

        public bool IsLive(string path) =>
            OperationsOn(path).Any(route => !Operations[route].Deprecated);
    }

    private sealed class ConnectBaseline
    {
        [JsonPropertyName("specVersion")] public string SpecVersion { get; set; } = "";

        [JsonPropertyName("implemented")] public List<ConnectBaselineEndpoint> Implemented { get; set; } = [];

        [JsonPropertyName("specOperations")] public List<string> SpecOperations { get; set; } = [];
    }

    private sealed class ConnectBaselineEndpoint
    {
        [JsonPropertyName("verb")] public string Verb { get; set; } = "";
        [JsonPropertyName("path")] public string Path { get; set; } = "";
        [JsonPropertyName("method")] public string Method { get; set; } = "";
    }
}
