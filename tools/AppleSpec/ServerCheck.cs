using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppleSpec;

public static class ServerCheck
{
    private const string ProjectName = "Enjna.AppStoreServerLibrary";
    private const string CoverageFile = "coverage.server.json";

    public static CheckReport Run(SpecPaths paths)
    {
        var report = new CheckReport { Title = "Server" };
        var inventory = ServerInventory.Read(Path.Combine(paths.SourceDir, ProjectName));
        var documented = ReadSnapshot(paths, report);

        var blind = Blindness(paths, documented, inventory);

        if (blind is not null)
        {
            report.Failures.Add(blind.Value);

            return report;
        }

        var coverage = ServerCoverage.Load(paths.ServerCoverageFile, report);

        CheckDictionaries(documented, inventory, coverage, report);
        CheckEnums(documented, inventory, coverage, report);
        CheckEndpoints(documented, inventory, coverage, report);
        CheckNetOnly(inventory, coverage, report);
        CheckBaselineIsCurrent(coverage, report);

        report.Notes.Insert(0, $"{documented.Pages} pages across {Program.Frameworks.Length} frameworks describe "
            + $"{documented.Dictionaries.Count} dictionaries, {documented.Enums.Count} enums and {documented.Endpoints.Count} endpoints; "
            + $"the library declares {inventory.Types.Count} types, {inventory.Enums.Count} enums and {inventory.Endpoints.Count} requests.");

        return report;
    }

    private static CheckItem? Blindness(SpecPaths paths, Snapshot documented, NetInventory inventory)
    {
        var source = Path.Combine(paths.SourceDir, ProjectName);

        return (documented.Pages, documented.Dictionaries.Count, documented.Enums.Count, documented.Endpoints.Count,
                inventory.Types.Count, inventory.Enums.Count, inventory.Endpoints.Count) switch
        {
            (0, _, _, _, _, _, _) => new CheckItem("Blind", paths.SpecDir, "the documentation snapshot is empty; run `sync` before `check`"),
            (_, 0, _, _, _, _, _) => new CheckItem("Blind", paths.SpecDir, "the snapshot describes no dictionaries, so no type could be checked"),
            (_, _, 0, _, _, _, _) => new CheckItem("Blind", paths.SpecDir, "the snapshot describes no enums, so no value list could be checked"),
            (_, _, _, 0, _, _, _) => new CheckItem("Blind", paths.SpecDir, "the snapshot describes no endpoints, so no request could be checked"),
            (_, _, _, _, 0, _, _) => new CheckItem("Blind", source, "the inventory walker read no types from the library source"),
            (_, _, _, _, _, 0, _) => new CheckItem("Blind", source, "the inventory walker read no enums from the library source"),
            (_, _, _, _, _, _, 0) => new CheckItem("Blind", source, "the inventory walker found no request the client issues"),
            _ => null
        };
    }

    private static void CheckDictionaries(Snapshot documented, NetInventory inventory, ServerCoverage coverage, CheckReport report)
    {
        var byName = inventory.Types
            .GroupBy(type => type.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

        var mapped = new HashSet<string>(StringComparer.Ordinal);
        var compared = 0;

        foreach (var symbol in documented.Dictionaries.Values.OrderBy(symbol => symbol.Title, StringComparer.Ordinal))
        {
            if (coverage.IsIgnored(symbol))
            {
                continue;
            }

            var type = Resolve(symbol, coverage.Types, byName, coverage, report, "type");

            if (type is null)
            {
                continue;
            }

            mapped.Add(type.Name);
            compared += CompareProperties(symbol, type, coverage, report);
        }

        report.Notes.Add($"{mapped.Count} of {documented.Dictionaries.Count} documented dictionaries resolve to a .NET type; "
            + $"{compared} of their properties were compared against a flattened wire name.");

        foreach (var type in inventory.Types.Where(type => !type.Internal && !mapped.Contains(type.Name)))
        {
            if (coverage.ClaimNetOnly(type.Name))
            {
                continue;
            }

            report.Reports.Add(new CheckItem("Undocumented type", type.Name,
                $"declared in {type.File} with no dictionary of that name in the snapshot"));
        }
    }

    private static int CompareProperties(DocSymbol symbol, NetType type, ServerCoverage coverage, CheckReport report)
    {
        var wire = type.Properties
            .Where(property => property.Wire is not null)
            .Select(property => property.Wire!)
            .ToList();

        var exact = new HashSet<string>(wire, StringComparer.Ordinal);
        var loose = new HashSet<string>(wire, StringComparer.OrdinalIgnoreCase);

        // Deliberately case-insensitive, and so it cannot be the set's own Ordinal lookup: a property
        // Apple spells differently in case is already reported just above, and matching it exactly
        // here would report it a second time as undocumented.
        var documented = symbol.Members.ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var member in symbol.Members.Where(member => !exact.Contains(member)))
        {
            if (coverage.IsIgnored(symbol, member))
            {
                continue;
            }

            // Apple's spelling is the wire spelling: a field the library spells differently still
            // arrives, so it is reported; one it does not carry at all drops data on decode, so it fails.
            if (loose.Contains(member))
            {
                report.Reports.Add(new CheckItem("Property spelling", $"{symbol.Title}.{member}",
                    $"{type.Name} carries it under a different case: {wire.First(name => string.Equals(name, member, StringComparison.OrdinalIgnoreCase))}"));

                continue;
            }

            report.Failures.Add(new CheckItem("Missing property", $"{symbol.Title}.{member}",
                $"documented in {symbol.Where()} but absent from {type.Name} ({type.File})"));
        }

        foreach (var property in type.Properties.Where(property => property.Wire is not null && !documented.Contains(property.Wire!)))
        {
            report.Reports.Add(new CheckItem("Undocumented property", $"{type.Name}.{property.Wire}",
                $"sent on the wire but not documented on {symbol.Title}"));
        }

        return symbol.Members.Count;
    }

    private static void CheckEnums(Snapshot documented, NetInventory inventory, ServerCoverage coverage, CheckReport report)
    {
        var byName = inventory.Enums
            .GroupBy(@enum => @enum.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

        var mapped = new HashSet<string>(StringComparer.Ordinal);
        var compared = 0;

        foreach (var symbol in documented.Enums.Values.OrderBy(symbol => symbol.Title, StringComparer.Ordinal))
        {
            if (coverage.IsIgnored(symbol))
            {
                continue;
            }

            var @enum = Resolve(symbol, coverage.Enums, byName, coverage, report, "enum");

            if (@enum is null)
            {
                continue;
            }

            mapped.Add(@enum.Name);
            compared += symbol.Members.Count;

            var values = @enum.Members
                .Where(member => !member.Sentinel && member.Wire is not null)
                .Select(member => member.Wire!)
                .ToHashSet(StringComparer.Ordinal);

            // _Unmapped keeps an unknown value from throwing, but it does not preserve it: the value
            // is gone the moment it decodes. That is the same silent loss as a property the library
            // does not carry, so it fails the same way. Write the decision down to excuse one.
            foreach (var value in symbol.Members.Where(value => !values.Contains(value) && !coverage.IsIgnored(symbol, value)))
            {
                report.Failures.Add(new CheckItem("Unmapped enum value", $"{symbol.Title}.{value}",
                    $"documented in {symbol.Where()} but {@enum.Name} has no member carrying it, so it decodes as _Unmapped and the value is lost"));
            }

            // The other direction loses nothing, so it stays a report: a member Apple never documents
            // is either a library-only value someone added on purpose or a spelling that stopped matching.
            foreach (var value in values.Where(value => !symbol.Members.Contains(value) && !coverage.IsIgnored(symbol, value)))
            {
                report.Reports.Add(new CheckItem("Retired enum value", $"{@enum.Name}.{value}",
                    $"mapped by the library but no longer listed on {symbol.Title}"));
            }
        }

        foreach (var @enum in inventory.Enums.Where(@enum => !mapped.Contains(@enum.Name)))
        {
            if (coverage.ClaimNetOnly(@enum.Name))
            {
                continue;
            }

            report.Reports.Add(new CheckItem("Undocumented enum", @enum.Name,
                $"declared in {@enum.File} with no documented value list of that name"));
        }

        report.Notes.Add($"{mapped.Count} of {documented.Enums.Count} documented enums resolve to a .NET enum; "
            + $"{compared} of their values were compared against a wire value.");
    }

    private static void CheckEndpoints(Snapshot documented, NetInventory inventory, ServerCoverage coverage, CheckReport report)
    {
        var bySignature = inventory.Endpoints
            .GroupBy(endpoint => Signature(endpoint.Verb, endpoint.Path), StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

        var byMethod = inventory.Endpoints
            .GroupBy(endpoint => endpoint.Method, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

        var claimed = new HashSet<string>(StringComparer.Ordinal);

        foreach (var symbol in documented.Endpoints.Values.OrderBy(symbol => symbol.Title, StringComparer.Ordinal))
        {
            if (coverage.IsIgnored(symbol))
            {
                continue;
            }

            if (coverage.Endpoints.TryGetValue(symbol.Title, out var method))
            {
                coverage.ClaimMapping(symbol.Title);

                if (!byMethod.ContainsKey(method))
                {
                    report.Failures.Add(new CheckItem("Stale mapping", symbol.Title,
                        $"the baseline maps it to {method}, which no longer issues a request"));

                    continue;
                }

                claimed.Add(method);

                continue;
            }

            if (bySignature.TryGetValue(symbol.Title, out var endpoint))
            {
                claimed.Add(endpoint.Method);

                continue;
            }

            report.Failures.Add(new CheckItem("Unmapped endpoint", symbol.Title,
                $"{symbol.Where()} documents it; no client method issues it, and the baseline neither maps nor ignores it"));
        }

        foreach (var endpoint in inventory.Endpoints.Where(endpoint => !claimed.Contains(endpoint.Method)))
        {
            report.Reports.Add(new CheckItem("Undocumented request", endpoint.Method,
                $"issues {Signature(endpoint.Verb, endpoint.Path)}, which matches no documented endpoint"));
        }

        report.Notes.Add($"{claimed.Count} of {inventory.Endpoints.Count} client methods answer one of "
            + $"{documented.Endpoints.Count} documented endpoints.");
    }

    private static void CheckNetOnly(NetInventory inventory, ServerCoverage coverage, CheckReport report)
    {
        var names = inventory.Types.Select(type => type.Name)
            .Concat(inventory.Enums.Select(@enum => @enum.Name))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var entry in coverage.NetOnly)
        {
            if (!names.Contains(entry.Name))
            {
                report.Failures.Add(new CheckItem("Stale baseline", entry.Name,
                    $"`{CoverageFile}` excuses a .NET symbol the library no longer declares"));
            }
            else if (!entry.Claimed)
            {
                report.Reports.Add(new CheckItem("Obsolete exemption", entry.Name,
                    $"`{CoverageFile}` lists it as having no Apple counterpart, but one now matches"));
            }
        }
    }

    private static void CheckBaselineIsCurrent(ServerCoverage coverage, CheckReport report)
    {
        foreach (var (key, kind) in coverage.UnclaimedKeys())
        {
            report.Failures.Add(new CheckItem("Stale baseline", key,
                $"`{CoverageFile}` {kind} an Apple symbol that no longer appears in the snapshot"));
        }
    }

    // Baseline first, then the symbol's own name, then the framework prefix: an explicit decision
    // must never be silently overtaken by a name collision.
    private static T? Resolve<T>(
        DocSymbol symbol,
        IReadOnlyDictionary<string, string> map,
        IReadOnlyDictionary<string, T> declared,
        ServerCoverage coverage,
        CheckReport report,
        string noun)
        where T : class
    {
        if (map.TryGetValue(symbol.Title, out var name))
        {
            coverage.ClaimMapping(symbol.Title);

            if (declared.TryGetValue(name, out var explicitly))
            {
                return explicitly;
            }

            report.Failures.Add(new CheckItem("Stale mapping", symbol.Title,
                $"the baseline maps it to {noun} {name}, which the library no longer declares"));

            return null;
        }

        if (declared.TryGetValue(symbol.Title, out var direct))
        {
            return direct;
        }

        foreach (var framework in symbol.Frameworks)
        {
            if (coverage.Prefixes.TryGetValue(framework, out var prefix) && declared.TryGetValue(prefix + symbol.Title, out var prefixed))
            {
                return prefixed;
            }
        }

        report.Failures.Add(new CheckItem($"Unmapped {noun}", symbol.Title,
            $"{symbol.Where()} documents it; the library declares no {noun} of that name, and the baseline neither maps nor ignores it"));

        return null;
    }

    private static Snapshot ReadSnapshot(SpecPaths paths, CheckReport report)
    {
        var snapshot = new Snapshot();

        foreach (var framework in Program.Frameworks)
        {
            var directory = paths.FrameworkDir(framework);

            if (!Directory.Exists(directory))
            {
                continue;
            }

            foreach (var file in Directory.EnumerateFiles(directory, "*.json").OrderBy(file => file, StringComparer.Ordinal))
            {
                SpecPage? page;

                try
                {
                    page = JsonSerializer.Deserialize<SpecPage>(File.ReadAllText(file), Json.Options);
                }
                catch (JsonException ex)
                {
                    report.Failures.Add(new CheckItem("Blind", Path.GetFileName(file), $"unreadable snapshot page: {ex.Message}"));

                    continue;
                }

                if (page is null)
                {
                    continue;
                }

                snapshot.Add(framework, page);
            }
        }

        return snapshot;
    }

    // Apple splits a URL across a base, literal runs and parameter tokens; the library writes one
    // interpolated string, and one of its holes is a version selector rather than a parameter.
    internal static string Signature(string verb, string path)
    {
        var segments = path
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Where(segment => !segment.StartsWith('{'));

        return $"{verb.ToUpperInvariant()} /{string.Join('/', segments)}";
    }
}

// Apple repeats a symbol such as environment in every framework that mentions it, so symbols merge
// by title and Frameworks keeps the trail back to where each is written.
internal sealed class Snapshot
{
    public int Pages { get; private set; }

    public Dictionary<string, DocSymbol> Dictionaries { get; } = new(StringComparer.Ordinal);
    public Dictionary<string, DocSymbol> Enums { get; } = new(StringComparer.Ordinal);
    public Dictionary<string, DocSymbol> Endpoints { get; } = new(StringComparer.Ordinal);

    public void Add(string framework, SpecPage page)
    {
        Pages++;

        if (page.Values is { Count: > 0 })
        {
            Merge(Enums, framework, page.Title, page.Values.Select(value => value.Name));
        }
        else if (page.Properties is not null || string.Equals(page.Kind, "dictionary", StringComparison.OrdinalIgnoreCase))
        {
            // A property section, not the kind string, is what makes a page an object shape, and an
            // empty list is a real dictionary: seven library types are shells with no properties.
            Merge(Dictionaries, framework, page.Title, (page.Properties ?? []).Select(property => property.Name));
        }

        foreach (var endpoint in page.Endpoints ?? [])
        {
            // Production and sandbox differ only in host, so both reduce to the same signature.
            Merge(Endpoints, framework, ServerCheck.Signature(endpoint.Method, Join(endpoint.BaseUrl, endpoint.Path)), []);
        }
    }

    private static void Merge(Dictionary<string, DocSymbol> into, string framework, string title, IEnumerable<string> members)
    {
        if (string.IsNullOrEmpty(title))
        {
            return;
        }

        if (!into.TryGetValue(title, out var symbol))
        {
            into[title] = symbol = new DocSymbol { Title = title };
        }

        symbol.Frameworks.Add(framework);

        foreach (var member in members.Where(member => !string.IsNullOrEmpty(member)))
        {
            symbol.Members.Add(member);
        }
    }

    // Apple is midway through a host migration, so the host is the one part of a documented URL that
    // is expected to change without the endpoint changing.
    private static string Join(string baseUrl, string path)
    {
        var combined = $"{baseUrl}/{path}";
        var scheme = combined.IndexOf("://", StringComparison.Ordinal);

        if (scheme >= 0)
        {
            var host = combined.IndexOf('/', scheme + 3);
            combined = host >= 0 ? combined[host..] : "/";
        }

        return combined;
    }
}

internal sealed class DocSymbol
{
    public required string Title { get; init; }
    public SortedSet<string> Frameworks { get; } = new(StringComparer.Ordinal);
    public SortedSet<string> Members { get; } = new(StringComparer.Ordinal);

    public string Where() => string.Join(", ", Frameworks);
}

internal sealed class ServerCoverage
{
    // Apple reuses names such as RequestInfo and jwsTransaction across frameworks; the library cannot.
    [JsonPropertyName("prefixes")] public Dictionary<string, string> Prefixes { get; set; } = new(StringComparer.Ordinal);

    [JsonPropertyName("types")] public Dictionary<string, string> Types { get; set; } = new(StringComparer.Ordinal);
    [JsonPropertyName("enums")] public Dictionary<string, string> Enums { get; set; } = new(StringComparer.Ordinal);
    [JsonPropertyName("endpoints")] public Dictionary<string, string> Endpoints { get; set; } = new(StringComparer.Ordinal);

    [JsonPropertyName("ignore")] public List<CoverageIgnore> Ignore { get; set; } = [];
    [JsonPropertyName("netOnly")] public List<CoverageNetOnly> NetOnly { get; set; } = [];

    private readonly HashSet<string> _claimed = new(StringComparer.Ordinal);

    public static ServerCoverage Load(string file, CheckReport report)
    {
        if (!File.Exists(file))
        {
            report.Notes.Add($"no `{Path.GetFileName(file)}`; every departure from Apple's shape will fail until one is written");

            return new ServerCoverage();
        }

        var coverage = JsonSerializer.Deserialize<ServerCoverage>(File.ReadAllText(file), Json.Options) ?? new ServerCoverage();

        report.Notes.Add($"{coverage.Types.Count + coverage.Enums.Count + coverage.Endpoints.Count} baseline mappings, "
            + $"{coverage.Ignore.Count} ignores and {coverage.NetOnly.Count} .NET-only symbols are in force.");

        return coverage;
    }

    public bool IsIgnored(DocSymbol symbol, string? member = null)
    {
        var suffix = member is null ? "" : $".{member}";

        foreach (var entry in Ignore)
        {
            if (Matches(entry.Symbol, symbol.Title + suffix))
            {
                Claim(entry.Symbol);

                return true;
            }

            foreach (var framework in symbol.Frameworks)
            {
                if (Matches(entry.Symbol, $"{framework}/{symbol.Title}{suffix}"))
                {
                    Claim(entry.Symbol);

                    return true;
                }
            }
        }

        return false;
    }

    public void ClaimMapping(string key) => Claim(key);

    public bool ClaimNetOnly(string name)
    {
        var entry = NetOnly.FirstOrDefault(candidate => string.Equals(candidate.Name, name, StringComparison.Ordinal));

        if (entry is null)
        {
            return false;
        }

        entry.Claimed = true;

        return true;
    }

    // A glob that currently catches nothing is a standing decision, not a stale one, so it is exempt.
    public IEnumerable<(string Key, string Kind)> UnclaimedKeys()
    {
        foreach (var key in Types.Keys.Concat(Enums.Keys).Concat(Endpoints.Keys).Where(key => !_claimed.Contains(key)))
        {
            yield return (key, "maps");
        }

        foreach (var entry in Ignore.Where(entry => !entry.Symbol.Contains('*', StringComparison.Ordinal) && !_claimed.Contains(entry.Symbol)))
        {
            yield return (entry.Symbol, "ignores");
        }
    }

    private void Claim(string key) => _claimed.Add(key);

    private static bool Matches(string pattern, string candidate)
    {
        if (!pattern.Contains('*', StringComparison.Ordinal))
        {
            return string.Equals(pattern, candidate, StringComparison.Ordinal);
        }

        var parts = pattern.Split('*');
        var at = 0;

        for (var index = 0; index < parts.Length; index++)
        {
            var part = parts[index];

            if (part.Length == 0)
            {
                continue;
            }

            var found = index == 0
                ? candidate.StartsWith(part, StringComparison.Ordinal) ? 0 : -1
                : candidate.IndexOf(part, at, StringComparison.Ordinal);

            if (found < 0)
            {
                return false;
            }

            at = found + part.Length;
        }

        return parts[^1].Length == 0 || candidate.EndsWith(parts[^1], StringComparison.Ordinal);
    }
}

internal sealed class CoverageIgnore
{
    [JsonPropertyName("symbol")] public string Symbol { get; set; } = "";
    [JsonPropertyName("reason")] public string Reason { get; set; } = "";
}

internal sealed class CoverageNetOnly
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("reason")] public string Reason { get; set; } = "";

    [JsonIgnore] public bool Claimed { get; set; }
}
