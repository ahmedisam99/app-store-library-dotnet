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

        // A report rather than a failure: Apple ships the replacement before it retires the original.
        foreach (var pair in implemented
            .Where(pair => spec.Operations.TryGetValue(pair.Key, out var operation) && operation.Deprecated)
            .OrderBy(pair => pair.Key.Path, StringComparer.Ordinal)
            .ThenBy(pair => pair.Key.Verb, StringComparer.Ordinal))
        {
            report.Reports.Add(new CheckItem("Implemented operation Apple deprecated", pair.Key.ToString(), $"`{pair.Value.Method}` calls it; `{spec.Operations[pair.Key].OperationId}` is marked deprecated."));
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

                spec.Operations[route] = new SpecOperation(identifier, deprecated);

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

    private readonly record struct SpecOperation(string OperationId, bool Deprecated);

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
