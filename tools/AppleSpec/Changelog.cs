using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AppleSpec;

public static class Changelog
{
    private const string GeneratorNotice =
        "<!-- Generated from Apple's DocC render JSON by tools/AppleSpec. Do not edit. -->";

    private static readonly string[] TextBearingKeys = ["text", "code", "title", "name", "alt", "destination"];

    // Server-API headings lead with the version: "1.21 - 2026/04/27", "1.10.1  — 2024/03/12",
    // "1.0b1 — 2021/06/07". Apple writes a hyphen, an en dash, an em dash, or nothing at all
    // ("1.5 2026/04/27"), sometimes after a doubled space, so the date itself terminates the version.
    private static readonly Regex LeadingVersion =
        new(@"^\s*(\d+(?:\.\d+)*(?:[A-Za-z]+\d*)?)(?=\s*$|\s*[-–—]|\s+\d)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    // Notifications headings lead with the date and trail the version: "October 21, 2021 - version 2".
    // A heading that is only a date carries no version at all.
    private static readonly Regex TrailingVersion =
        new(@"[-–—]\s*version\s+(\d[\w.]*)\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex Whitespace = new(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static string Render(JsonElement page)
    {
        var references = Property(page, "references");
        var blocks = new List<string> { "# " + EscapeText(Title(page), references), Provenance(page) };

        // DocC keeps the abstract outside the content sections, and it is inline content, not blocks.
        var summary = Inline.Render(Property(page, "abstract"), references).Text;
        if (summary.Length > 0)
        {
            blocks.Add(summary);
        }

        foreach (var section in Elements(page, "primaryContentSections"))
        {
            var rendered = RenderSection(section, references);
            if (rendered.Length > 0)
            {
                blocks.Add(rendered);
            }
        }

        return string.Join("\n\n", blocks) + "\n";
    }

    public static string? LatestVersion(JsonElement page)
    {
        // Apple lists newest first, but the sequence is monotonic in neither version nor date.
        // "1.1 — 2022/10/21" sits below "1.2 — 2022/02/24", so sorting would answer with a typo.
        foreach (var heading in Headings(page))
        {
            var version = ParseVersion(heading);
            if (version is not null)
            {
                return version;
            }
        }

        return null;
    }

    private static string? ParseVersion(string heading)
    {
        var leading = LeadingVersion.Match(heading);
        if (leading.Success)
        {
            return leading.Groups[1].Value;
        }

        var trailing = TrailingVersion.Match(heading);
        return trailing.Success ? trailing.Groups[1].Value : null;
    }

    private static IEnumerable<string> Headings(JsonElement page)
    {
        foreach (var section in Elements(page, "primaryContentSections"))
        {
            foreach (var heading in HeadingsIn(section))
            {
                yield return heading;
            }
        }
    }

    private static IEnumerable<string> HeadingsIn(JsonElement node)
    {
        switch (node.ValueKind)
        {
            case JsonValueKind.Object:
                if (String(node, "type") == "heading" && String(node, "text") is { } text)
                {
                    yield return text;
                }

                foreach (var property in node.EnumerateObject())
                {
                    foreach (var heading in HeadingsIn(property.Value))
                    {
                        yield return heading;
                    }
                }

                break;

            case JsonValueKind.Array:
                foreach (var item in node.EnumerateArray())
                {
                    foreach (var heading in HeadingsIn(item))
                    {
                        yield return heading;
                    }
                }

                break;
        }
    }

    private static string RenderSection(JsonElement section, JsonElement references)
    {
        var parts = new List<string>();
        var title = String(section, "title");

        if (!string.IsNullOrEmpty(title))
        {
            parts.Add("## " + EscapeText(title, references));
        }

        var content = Property(section, "content");
        if (content.ValueKind == JsonValueKind.Array)
        {
            var body = Inline.RenderBlocks(content, references);
            if (body.Length > 0)
            {
                parts.Add(body);
            }
        }
        else
        {
            parts.Add(Unhandled(section, "section", String(section, "kind") ?? "?"));
        }

        return string.Join("\n\n", parts);
    }

    private static string Provenance(JsonElement page)
    {
        var path = Elements(page, "variants").Select(variant => Elements(variant, "paths").FirstOrDefault())
            .Select(first => first.ValueKind == JsonValueKind.String ? first.GetString() : null)
            .FirstOrDefault(value => !string.IsNullOrEmpty(value));

        return path is null
            ? GeneratorNotice
            : GeneratorNotice
                + $"\n<!-- page: https://developer.apple.com{path} -->"
                + $"\n<!-- data: https://developer.apple.com/tutorials/data{path}.json -->";
    }

    // A shape Apple adds later has to show up in the diff; one that rendered as nothing would
    // look like no change at all, forever.
    private static string Unhandled(JsonElement node, string kind, string name)
    {
        var keys = node.ValueKind == JsonValueKind.Object
            ? string.Join(",", node.EnumerateObject().Select(property => property.Name))
            : "";

        var recovered = Whitespace.Replace(string.Join(" ", Salvage(node)), " ").Trim();
        var comment = $"<!-- docc:unhandled {kind}={name} keys={keys} -->";

        return recovered.Length == 0 ? comment : $"{comment} {recovered}";
    }

    private static IEnumerable<string> Salvage(JsonElement node)
    {
        switch (node.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var key in TextBearingKeys)
                {
                    if (String(node, key) is { Length: > 0 } value)
                    {
                        yield return value;
                    }
                }

                foreach (var property in node.EnumerateObject())
                {
                    foreach (var value in Salvage(property.Value))
                    {
                        yield return value;
                    }
                }

                break;

            case JsonValueKind.Array:
                foreach (var item in node.EnumerateArray())
                {
                    foreach (var value in Salvage(item))
                    {
                        yield return value;
                    }
                }

                break;
        }
    }

    private static string EscapeText(string text, JsonElement references)
    {
        using var document = JsonDocument.Parse(JsonSerializer.Serialize(new[] { new { type = "text", text } }));
        return Inline.Render(document.RootElement, references).Text;
    }

    private static string Title(JsonElement page)
    {
        var metadata = Property(page, "metadata");
        return String(metadata, "title") is { Length: > 0 } title ? title : "Untitled";
    }

    private static JsonElement Property(JsonElement node, string name) =>
        node.ValueKind == JsonValueKind.Object && node.TryGetProperty(name, out var value) ? value : default;

    private static IEnumerable<JsonElement> Elements(JsonElement node, string name)
    {
        var value = Property(node, name);
        return value.ValueKind == JsonValueKind.Array ? value.EnumerateArray() : [];
    }

    private static string? String(JsonElement node, string name)
    {
        var value = Property(node, name);
        return value.ValueKind == JsonValueKind.String ? value.GetString() : null;
    }
}
