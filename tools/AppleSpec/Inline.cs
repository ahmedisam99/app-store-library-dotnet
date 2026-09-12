using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace AppleSpec;

public static class Inline
{
    // An inline type that is not handled throws rather than being skipped: skipping would
    // delete words from the middle of a sentence and leave a plausible-looking one behind.
    private static readonly HashSet<string> InlineTypes = new(StringComparer.Ordinal)
    {
        "text",
        "codeVoice",
        "reference",
        "link",
        "emphasis",
        "strong",
        "inlineHead",
        "image",
        "newTerm",
        "superscript",
        "subscript",
        "strikethrough"
    };

    private static readonly HashSet<string> BlockTypes = new(StringComparer.Ordinal)
    {
        "paragraph",
        "heading",
        "unorderedList",
        "orderedList",
        "aside",
        "codeListing",
        "table",
        "termList",
        "tabNavigator",
        "row"
    };

    public static Prose Render(JsonElement content, JsonElement references) => Render(content, references, "inlineContent");

    public static string RenderBlocks(JsonElement content, JsonElement references) => RenderBlocks(content, references, "content").Text;

    internal static Prose Render(JsonElement content, JsonElement references, string path)
    {
        var sink = new Sink();
        WriteInline(content, references, sink, path);
        return sink.ToProse();
    }

    internal static Prose RenderBlocks(JsonElement content, JsonElement references, string path)
    {
        var sink = new Sink();
        WriteBlocks(content, references, sink, path);
        return sink.ToProse();
    }

    internal static Prose RenderBlock(JsonElement node, JsonElement references, string path)
    {
        var sink = new Sink();
        WriteBlock(node, references, sink, path);
        return sink.ToProse();
    }

    internal static string RenderCell(JsonElement content, JsonElement references, string path)
    {
        var text = RenderBlocks(content, references, path).Text;
        var cell = new StringBuilder(text.Length);

        foreach (var character in text)
        {
            cell.Append(character switch
            {
                '\n' => ' ',
                '|' => '\\',
                _ => character
            });

            if (character == '|')
            {
                cell.Append('|');
            }
        }

        return Collapse(cell.ToString());
    }

    internal static void CollectAsides(JsonElement content, JsonElement references, string path, List<Note> into)
    {
        foreach (var (node, index) in Enumerate(content))
        {
            if (NodeType(node, $"{path}[{index}]") == "aside")
            {
                into.Add(new Note
                {
                    Style = Text(node, "style"),
                    Prose = RenderBlocks(Child(node, "content"), references, $"{path}[{index}].content")
                });
            }

            foreach (var (nested, nestedPath) in NestedBlocks(node, $"{path}[{index}]"))
            {
                CollectAsides(nested, references, nestedPath, into);
            }
        }
    }

    internal static void CollectTables(JsonElement content, JsonElement references, string path, List<Table> into)
    {
        foreach (var (node, index) in Enumerate(content))
        {
            var nodePath = $"{path}[{index}]";

            if (NodeType(node, nodePath) == "table")
            {
                into.Add(ReadTable(node, references, nodePath));
            }

            foreach (var (nested, nestedPath) in NestedBlocks(node, nodePath))
            {
                CollectTables(nested, references, nestedPath, into);
            }
        }
    }

    private static Table ReadTable(JsonElement node, JsonElement references, string path)
    {
        var table = new Table();
        var headerIsFirstRow = Text(node, "header") == "row";

        foreach (var (row, index) in Enumerate(Child(node, "rows")))
        {
            var cells = new List<string>();

            foreach (var (cell, column) in Enumerate(row))
            {
                cells.Add(RenderCell(cell, references, $"{path}.rows[{index}][{column}]"));
            }

            if (index == 0 && headerIsFirstRow)
            {
                table.Headers = cells;
            }
            else
            {
                table.Rows.Add(cells);
            }
        }

        return table;
    }

    private static void WriteInline(JsonElement content, JsonElement references, Sink sink, string path)
    {
        foreach (var (node, index) in Enumerate(content))
        {
            var nodePath = $"{path}[{index}]";
            var type = NodeType(node, nodePath);

            if (!InlineTypes.Contains(type))
            {
                throw new SpecFormatException($"{nodePath}: inline content type \"{type}\" is not handled.");
            }

            switch (type)
            {
                case "text":
                    sink.SinkText.Append(Text(node, "text"));
                    break;

                case "codeVoice":
                    sink.SinkText.Append('`').Append(Text(node, "code")).Append('`');
                    break;

                case "reference":
                    WriteReference(node, sink);
                    break;

                case "link":
                    // Apple's current corpus routes every link through "reference"; this arm is unexercised.
                    sink.Ref(Text(node, "destination"));
                    sink.SinkText.Append(Text(node, "title") is { Length: > 0 } title ? title : Text(node, "destination"));
                    break;

                case "image":
                    sink.Ref(Text(node, "identifier"));
                    sink.SinkText.Append("![").Append(Text(node, "identifier")).Append(']');
                    break;

                case "strong":
                case "inlineHead":
                    Wrap(node, references, sink, nodePath, "**");
                    break;

                case "emphasis":
                case "newTerm":
                    Wrap(node, references, sink, nodePath, "*");
                    break;

                case "strikethrough":
                    Wrap(node, references, sink, nodePath, "~~");
                    break;

                case "superscript":
                    Wrap(node, references, sink, nodePath, "^");
                    break;

                case "subscript":
                    Wrap(node, references, sink, nodePath, "~");
                    break;
            }
        }
    }

    private static void Wrap(JsonElement node, JsonElement references, Sink sink, string path, string marker)
    {
        sink.SinkText.Append(marker);
        WriteInline(Child(node, "inlineContent"), references, sink, $"{path}.inlineContent");
        sink.SinkText.Append(marker);
    }

    // Renders the identifier's terminal segment, not the referenced page's title: a title is
    // copied onto every page that links to it, so resolving titles churns every page on a retitle.
    private static void WriteReference(JsonElement node, Sink sink)
    {
        var identifier = Text(node, "identifier");
        sink.Ref(identifier);

        // overridingTitle is set only on plain web links, and is stored on the linking page, so
        // honouring it churns nothing.
        var overriding = Text(node, "overridingTitle");

        if (overriding.Length > 0)
        {
            sink.SinkText.Append(overriding);
            return;
        }

        sink.SinkText.Append('`').Append(SymbolName(identifier)).Append('`');
    }

    internal static string SymbolName(string identifier)
    {
        if (!identifier.StartsWith("doc://", StringComparison.Ordinal))
        {
            return identifier;
        }

        var cut = identifier.LastIndexOf('/');
        return cut >= 0 && cut + 1 < identifier.Length ? identifier[(cut + 1)..] : identifier;
    }

    private static void WriteBlocks(JsonElement content, JsonElement references, Sink sink, string path)
    {
        var first = true;

        foreach (var (node, index) in Enumerate(content))
        {
            if (!first)
            {
                sink.SinkText.Append("\n\n");
            }

            first = false;
            WriteBlock(node, references, sink, $"{path}[{index}]");
        }
    }

    private static void WriteBlock(JsonElement node, JsonElement references, Sink sink, string path)
    {
        var type = NodeType(node, path);

        if (!BlockTypes.Contains(type))
        {
            throw new SpecFormatException($"{path}: block content type \"{type}\" is not handled.");
        }

        switch (type)
        {
            case "paragraph":
                WriteInline(Child(node, "inlineContent"), references, sink, $"{path}.inlineContent");
                break;

            case "heading":
                sink.SinkText.Append('#', Math.Clamp(Number(node, "level"), 1, 6)).Append(' ').Append(Text(node, "text"));
                break;

            case "unorderedList":
                WriteList(node, references, sink, path, ordered: false);
                break;

            case "orderedList":
                WriteList(node, references, sink, path, ordered: true);
                break;

            case "aside":
                WriteQuoted(node, references, sink, path);
                break;

            case "codeListing":
                WriteCode(node, sink);
                break;

            case "table":
                WriteTable(node, references, sink, path);
                break;

            case "termList":
                WriteTerms(node, references, sink, path);
                break;

            case "tabNavigator":
                WriteTabs(node, references, sink, path);
                break;

            case "row":
                WriteColumns(node, references, sink, path);
                break;
        }
    }

    private static void WriteList(JsonElement node, JsonElement references, Sink sink, string path, bool ordered)
    {
        var first = true;

        foreach (var (item, index) in Enumerate(Child(node, "items")))
        {
            if (!first)
            {
                sink.SinkText.Append('\n');
            }

            first = false;

            var marker = ordered ? $"{(index + 1).ToString(CultureInfo.InvariantCulture)}. " : "- ";
            sink.SinkText.Append(marker);
            AppendIndented(sink, Continue(sink, references, Child(item, "content"), $"{path}.items[{index}].content"), new string(' ', marker.Length));
        }
    }

    private static void WriteQuoted(JsonElement node, JsonElement references, Sink sink, string path)
    {
        sink.SinkText.Append("> **").Append(Text(node, "name")).Append("**\n>\n> ");
        AppendIndented(sink, Continue(sink, references, Child(node, "content"), $"{path}.content"), "> ");
    }

    private static void WriteCode(JsonElement node, Sink sink)
    {
        sink.SinkText.Append("```").Append(Text(node, "syntax")).Append('\n');

        foreach (var line in Enumerate(Child(node, "code")))
        {
            sink.SinkText.Append(line.Node.GetString()).Append('\n');
        }

        sink.SinkText.Append("```");
    }

    private static void WriteTable(JsonElement node, JsonElement references, Sink sink, string path)
    {
        var table = ReadTable(node, references, path);

        if (table.Headers.Count == 0 && table.Rows.Count == 0)
        {
            return;
        }

        var width = table.Headers.Count;

        foreach (var row in table.Rows)
        {
            width = Math.Max(width, row.Count);
        }

        if (table.Headers.Count > 0)
        {
            WriteRow(sink, table.Headers, width);
            sink.SinkText.Append('|');

            for (var column = 0; column < width; column++)
            {
                sink.SinkText.Append(" --- |");
            }

            sink.SinkText.Append('\n');
        }

        foreach (var row in table.Rows)
        {
            WriteRow(sink, row, width);
        }

        sink.SinkText.Length--;
    }

    private static void WriteRow(Sink sink, List<string> cells, int width)
    {
        sink.SinkText.Append('|');

        for (var column = 0; column < width; column++)
        {
            sink.SinkText.Append(' ').Append(column < cells.Count ? cells[column] : "").Append(" |");
        }

        sink.SinkText.Append('\n');
    }

    private static void WriteTerms(JsonElement node, JsonElement references, Sink sink, string path)
    {
        var first = true;

        foreach (var (item, index) in Enumerate(Child(node, "items")))
        {
            if (!first)
            {
                sink.SinkText.Append('\n');
            }

            first = false;

            var itemPath = $"{path}.items[{index}]";
            sink.SinkText.Append("- **");
            WriteInline(Child(Child(item, "term"), "inlineContent"), references, sink, $"{itemPath}.term.inlineContent");
            sink.SinkText.Append("** — ");
            AppendIndented(sink, Continue(sink, references, Child(Child(item, "definition"), "content"), $"{itemPath}.definition.content"), "  ");
        }
    }

    private static void WriteTabs(JsonElement node, JsonElement references, Sink sink, string path)
    {
        var first = true;

        foreach (var (tab, index) in Enumerate(Child(node, "tabs")))
        {
            if (!first)
            {
                sink.SinkText.Append("\n\n");
            }

            first = false;
            sink.SinkText.Append("**").Append(Text(tab, "title")).Append("**\n\n");
            sink.SinkText.Append(Continue(sink, references, Child(tab, "content"), $"{path}.tabs[{index}].content"));
        }
    }

    private static void WriteColumns(JsonElement node, JsonElement references, Sink sink, string path)
    {
        var first = true;

        foreach (var (column, index) in Enumerate(Child(node, "columns")))
        {
            if (!first)
            {
                sink.SinkText.Append("\n\n");
            }

            first = false;
            sink.SinkText.Append(Continue(sink, references, Child(column, "content"), $"{path}.columns[{index}].content"));
        }
    }

    private static string Continue(Sink sink, JsonElement references, JsonElement content, string path)
    {
        var nested = new Sink();
        WriteBlocks(content, references, nested, path);
        sink.Absorb(nested);
        return nested.SinkText.ToString();
    }

    private static void AppendIndented(Sink sink, string text, string indent)
    {
        sink.SinkText.Append(text.Replace("\n", "\n" + indent, StringComparison.Ordinal));
    }

    private static IEnumerable<(JsonElement Content, string Path)> NestedBlocks(JsonElement node, string path)
    {
        switch (NodeType(node, path))
        {
            case "aside":
                yield return (Child(node, "content"), $"{path}.content");
                break;

            case "unorderedList":
            case "orderedList":
                foreach (var (item, index) in Enumerate(Child(node, "items")))
                {
                    yield return (Child(item, "content"), $"{path}.items[{index}].content");
                }

                break;

            case "table":
                foreach (var (row, rowIndex) in Enumerate(Child(node, "rows")))
                {
                    foreach (var (cell, column) in Enumerate(row))
                    {
                        yield return (cell, $"{path}.rows[{rowIndex}][{column}]");
                    }
                }

                break;

            case "termList":
                foreach (var (item, index) in Enumerate(Child(node, "items")))
                {
                    yield return (Child(Child(item, "definition"), "content"), $"{path}.items[{index}].definition.content");
                }

                break;

            case "tabNavigator":
                foreach (var (tab, index) in Enumerate(Child(node, "tabs")))
                {
                    yield return (Child(tab, "content"), $"{path}.tabs[{index}].content");
                }

                break;

            case "row":
                foreach (var (column, index) in Enumerate(Child(node, "columns")))
                {
                    yield return (Child(column, "content"), $"{path}.columns[{index}].content");
                }

                break;
        }
    }

    private static string NodeType(JsonElement node, string path)
    {
        var type = Text(node, "type");

        if (type.Length == 0)
        {
            throw new SpecFormatException($"{path}: content node has no \"type\".");
        }

        return type;
    }

    internal static IEnumerable<(JsonElement Node, int Index)> Enumerate(JsonElement array)
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            yield break;
        }

        var index = 0;

        foreach (var node in array.EnumerateArray())
        {
            yield return (node, index++);
        }
    }

    internal static JsonElement Child(JsonElement node, string name)
        => node.ValueKind == JsonValueKind.Object && node.TryGetProperty(name, out var child) ? child : default;

    internal static string Text(JsonElement node, string name)
    {
        var child = Child(node, name);
        return child.ValueKind == JsonValueKind.String ? child.GetString() ?? "" : "";
    }

    private static int Number(JsonElement node, string name)
    {
        var child = Child(node, name);
        return child.ValueKind == JsonValueKind.Number && child.TryGetInt32(out var value) ? value : 1;
    }

    private static string Collapse(string text)
    {
        var collapsed = new StringBuilder(text.Length);
        var space = false;

        foreach (var character in text)
        {
            if (character == ' ')
            {
                space = true;
                continue;
            }

            if (space && collapsed.Length > 0)
            {
                collapsed.Append(' ');
            }

            space = false;
            collapsed.Append(character);
        }

        return collapsed.ToString();
    }

    private sealed class Sink
    {
        public readonly StringBuilder SinkText = new();

        private readonly List<string> _refs = [];
        private readonly HashSet<string> _seen = new(StringComparer.Ordinal);

        public void Ref(string identifier)
        {
            if (identifier.Length > 0 && _seen.Add(identifier))
            {
                _refs.Add(identifier);
            }
        }

        public void Absorb(Sink nested)
        {
            foreach (var identifier in nested._refs)
            {
                Ref(identifier);
            }
        }

        public Prose ToProse() => new() { Text = SinkText.ToString(), Refs = _refs };
    }
}
