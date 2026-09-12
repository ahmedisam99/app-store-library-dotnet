using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AppleSpec;

public static class Normalizer
{
    private static readonly HashSet<string> PageKeys = new(StringComparer.Ordinal)
    {
        "abstract",
        "deprecationSummary",
        "hierarchy",
        "identifier",
        "kind",
        "legalNotices",
        "metadata",
        "primaryContentSections",
        "references",
        "schemaVersion",
        "sections",
        "seeAlsoSections",
        "topicSections",
        "variantOverrides",
        "variants"
    };

    private static readonly HashSet<string> MetadataKeys = new(StringComparer.Ordinal)
    {
        "externalID",
        "fragments",
        "images",
        "modules",
        "navigatorTitle",
        "platforms",
        "role",
        "roleHeading",
        "symbolKind",
        "title"
    };

    private static readonly HashSet<string> PlatformKeys = new(StringComparer.Ordinal)
    {
        "beta",
        "deprecated",
        "deprecatedAt",
        "introducedAt",
        "name",
        "unavailable"
    };

    private static readonly Dictionary<string, HashSet<string>> SectionKeys = new(StringComparer.Ordinal)
    {
        ["declarations"] = new(StringComparer.Ordinal) { "kind", "declarations" },
        ["properties"] = new(StringComparer.Ordinal) { "kind", "title", "items" },
        ["content"] = new(StringComparer.Ordinal) { "kind", "content" },
        ["mentions"] = new(StringComparer.Ordinal) { "kind", "mentions" },
        ["restEndpoint"] = new(StringComparer.Ordinal) { "kind", "title", "tokens" },
        ["possibleValues"] = new(StringComparer.Ordinal) { "kind", "title", "values" },
        ["restResponses"] = new(StringComparer.Ordinal) { "kind", "title", "items" },
        ["restParameters"] = new(StringComparer.Ordinal) { "kind", "title", "source", "items" },
        ["attributes"] = new(StringComparer.Ordinal) { "kind", "title", "attributes" },
        ["restBody"] = new(StringComparer.Ordinal) { "kind", "title", "mimeType", "content", "bodyContentType", "parameters" }
    };

    private static readonly HashSet<string> MemberKeys = new(StringComparer.Ordinal)
    {
        "attributes",
        "content",
        "deprecated",
        "introducedVersion",
        "name",
        "required",
        "type"
    };

    private static readonly HashSet<string> ResponseKeys = new(StringComparer.Ordinal)
    {
        "content",
        "mimeType",
        "reason",
        "status",
        "type"
    };

    private static readonly HashSet<string> ValueKeys = new(StringComparer.Ordinal)
    {
        "content",
        "deprecated",
        "introducedVersion",
        "name"
    };

    // minimumLength and default are absent from the App Store corpus, but DocC emits them, so
    // they are read rather than left to fail a future sync.
    private static readonly HashSet<string> AttributeKinds = new(StringComparer.Ordinal)
    {
        "allowedValues",
        "default",
        "maximum",
        "maximumLength",
        "minimum",
        "minimumLength"
    };

    private static readonly HashSet<string> AttributeKeys = new(StringComparer.Ordinal) { "kind", "value", "values" };

    private static readonly HashSet<string> DeclarationKeys = new(StringComparer.Ordinal) { "languages", "platforms", "tokens" };

    private static readonly HashSet<string> DeclarationTokenKinds = new(StringComparer.Ordinal) { "identifier", "text", "typeIdentifier" };

    private static readonly HashSet<string> TypeTokenKinds = new(StringComparer.Ordinal) { "text", "typeIdentifier" };

    private static readonly HashSet<string> EndpointTokenKinds = new(StringComparer.Ordinal) { "baseURL", "method", "parameter", "path", "text" };

    private static readonly HashSet<string> TopicKeys = new(StringComparer.Ordinal) { "anchor", "generated", "identifiers", "title" };

    public static SpecPage Normalize(JsonElement page, string framework)
    {
        try
        {
            return Build(page, framework);
        }
        catch (SpecFormatException exception)
        {
            throw new SpecFormatException($"{Inline.Text(Inline.Child(page, "identifier"), "url")} ({framework}): {exception.Message}");
        }
    }

    public static string FileName(SpecPage page)
    {
        // Length-preserving on purpose: collapsing runs would map "get:v2-history-{}" onto
        // "get-v2-history" and collide with a real sibling endpoint.
        var name = new StringBuilder(page.Id.Length + 5);

        foreach (var character in page.Id)
        {
            var safe = character is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9') or '.' or '_' or '-';
            name.Append(safe ? character : '-');
        }

        if (name.Length == 0 || name[0] is '.' or '-')
        {
            name.Insert(0, '_');
        }

        return name.Append(".json").ToString();
    }

    private static SpecPage Build(JsonElement page, string framework)
    {
        RequireKeys(page, PageKeys, "");

        var metadata = Inline.Child(page, "metadata");
        RequireKeys(metadata, MetadataKeys, "metadata");

        var references = Inline.Child(page, "references");
        var url = Inline.Text(Inline.Child(page, "identifier"), "url");
        var slug = Slug(page);

        // sections is empty on every page in the corpus, so a page that fills it fails the sync
        // rather than having the content dropped silently.
        var sections = Inline.Child(page, "sections");

        if (sections.ValueKind == JsonValueKind.Array && sections.GetArrayLength() > 0)
        {
            throw new SpecFormatException("sections: expected to be empty, but the page fills it.");
        }

        var platform = First(Inline.Child(metadata, "platforms"));
        RequireKeys(platform, PlatformKeys, "metadata.platforms[0]");

        var result = new SpecPage
        {
            // Identity keys off metadata.externalID, not the doc slug: Apple has reassigned slugs
            // three times. Articles and collections have none, so they fall back to "doc:" over the slug.
            Id = Optional(metadata, "externalID") ?? $"doc:{slug}",
            Title = Inline.Text(metadata, "title"),
            Kind = Optional(metadata, "symbolKind") ?? Inline.Text(page, "kind"),
            Role = Inline.Text(metadata, "role"),
            Framework = framework,
            Path = DocumentationPath(url),
            Slug = slug,
            IntroducedAt = Optional(platform, "introducedAt"),
            Beta = Flag(platform, "beta"),
            Deprecated = ReadDeprecation(page, platform, references, url),
            Abstract = Inline.Render(Inline.Child(page, "abstract"), references, "abstract")
        };

        ReadSections(page, references, result);
        ReadGroups(Inline.Child(page, "topicSections"), "topicSections", result.Topics);
        ReadGroups(Inline.Child(page, "seeAlsoSections"), "seeAlsoSections", result.SeeAlso);

        return result;
    }

    private static void ReadSections(JsonElement page, JsonElement references, SpecPage result)
    {
        var article = new List<ArticleBlock>();
        var hasContent = false;

        foreach (var (section, index) in Inline.Enumerate(Inline.Child(page, "primaryContentSections")))
        {
            var path = $"primaryContentSections[{index}]";
            var kind = Inline.Text(section, "kind");

            if (!SectionKeys.TryGetValue(kind, out var allowed))
            {
                throw new SpecFormatException($"{path}: content section kind \"{kind}\" is not handled.");
            }

            RequireKeys(section, allowed, path);

            switch (kind)
            {
                case "declarations":
                    result.Declaration = ReadDeclaration(section, path);
                    break;

                case "attributes":
                    ReadAttributes(Inline.Child(section, "attributes"), $"{path}.attributes", result.Constraints);
                    break;

                case "properties":
                    (result.Properties ??= []).AddRange(ReadMembers(section, references, path, memberOf: null));
                    break;

                case "restParameters":
                    (result.Parameters ??= []).AddRange(ReadMembers(section, references, path, Inline.Text(section, "title")));
                    break;

                case "possibleValues":
                    (result.Values ??= []).AddRange(ReadValues(section, references, path));
                    break;

                case "restEndpoint":
                    (result.Endpoints ??= []).Add(ReadEndpoint(section, path));
                    break;

                case "restResponses":
                    (result.Responses ??= []).AddRange(ReadResponses(section, references, path));
                    break;

                case "restBody":
                    result.RequestBody = ReadRequestBody(section, references, path, result.RequestBody);
                    break;

                case "content":
                    hasContent = true;
                    article.AddRange(ReadArticle(Inline.Child(section, "content"), references, $"{path}.content"));
                    break;

                case "mentions":
                    // A reverse index of the pages that link here; every one of them is vendored,
                    // so the fact is already in the snapshot from the other end.
                    break;
            }

            CollectProse(section, references, path, result);
        }

        result.Article = hasContent ? article : null;
    }

    private static void CollectProse(JsonElement section, JsonElement references, string path, SpecPage result)
    {
        foreach (var (content, contentPath) in ProseArrays(section, path))
        {
            Inline.CollectAsides(content, references, contentPath, result.Notes);
            Inline.CollectTables(content, references, contentPath, result.Tables);
        }
    }

    private static IEnumerable<(JsonElement Content, string Path)> ProseArrays(JsonElement section, string path)
    {
        yield return (Inline.Child(section, "content"), $"{path}.content");

        foreach (var collection in new[] { "items", "values" })
        {
            foreach (var (item, index) in Inline.Enumerate(Inline.Child(section, collection)))
            {
                yield return (Inline.Child(item, "content"), $"{path}.{collection}[{index}].content");
            }
        }
    }

    private static Deprecation? ReadDeprecation(JsonElement page, JsonElement platform, JsonElement references, string url)
    {
        var at = Optional(platform, "deprecatedAt");
        var summary = Inline.Child(page, "deprecationSummary");

        // metadata.platforms[0].deprecated reads false on every page in the corpus, including the
        // nine that are deprecated; deprecatedAt, deprecationSummary and the self-reference each fire,
        // none of them covering all nine.
        var flagged = Flag(Inline.Child(references, url), "deprecated");

        if (at is null && summary.ValueKind != JsonValueKind.Array && !flagged)
        {
            return null;
        }

        return new Deprecation
        {
            At = at,
            Summary = summary.ValueKind == JsonValueKind.Array
                ? Inline.RenderBlocks(summary, references, "deprecationSummary")
                : null
        };
    }

    private static Declaration ReadDeclaration(JsonElement section, string path)
    {
        var declarations = Inline.Child(section, "declarations");

        if (declarations.ValueKind != JsonValueKind.Array || declarations.GetArrayLength() != 1)
        {
            throw new SpecFormatException($"{path}.declarations: expected exactly one declaration.");
        }

        var declaration = First(declarations);
        RequireKeys(declaration, DeclarationKeys, $"{path}.declarations[0]");

        var rendered = new StringBuilder();
        var typeExpression = new StringBuilder();

        foreach (var (token, index) in Inline.Enumerate(Inline.Child(declaration, "tokens")))
        {
            var kind = Inline.Text(token, "kind");

            if (!DeclarationTokenKinds.Contains(kind))
            {
                throw new SpecFormatException($"{path}.declarations[0].tokens[{index}]: token kind \"{kind}\" is not handled.");
            }

            var text = Inline.Text(token, "text");
            rendered.Append(text);

            // The trailing "identifier" token is the symbol's own name; everything before it is the
            // type, which for a type alias is the only place the underlying primitive is written down.
            if (kind != "identifier")
            {
                typeExpression.Append(text);
            }
        }

        var baseType = typeExpression.ToString().Trim();
        var isArray = baseType.StartsWith('[') && baseType.EndsWith(']');

        return new Declaration
        {
            Tokens = rendered.ToString().Trim(),
            BaseType = baseType.Length > 0 ? baseType : null,
            IsArray = isArray,
            ElementType = isArray ? baseType[1..^1].Trim() : null
        };
    }

    private static void ReadAttributes(JsonElement attributes, string path, List<Constraint> into)
    {
        foreach (var (attribute, index) in Inline.Enumerate(attributes))
        {
            var attributePath = $"{path}[{index}]";
            RequireKeys(attribute, AttributeKeys, attributePath);

            var kind = Inline.Text(attribute, "kind");

            if (!AttributeKinds.Contains(kind))
            {
                throw new SpecFormatException($"{attributePath}: attribute kind \"{kind}\" is not handled.");
            }

            var values = Inline.Child(attribute, "values");

            into.Add(new Constraint
            {
                Kind = kind,
                Value = Optional(attribute, "value"),
                Values = values.ValueKind == JsonValueKind.Array ? Strings(values) : null
            });
        }
    }

    private static IEnumerable<Member> ReadMembers(JsonElement section, JsonElement references, string path, string? memberOf)
    {
        foreach (var (item, index) in Inline.Enumerate(Inline.Child(section, "items")))
        {
            var itemPath = $"{path}.items[{index}]";
            RequireKeys(item, MemberKeys, itemPath);

            var member = new Member
            {
                In = memberOf,
                Name = Inline.Text(item, "name"),
                Required = Flag(item, "required"),
                Deprecated = Flag(item, "deprecated"),
                IntroducedVersion = Optional(item, "introducedVersion"),
                Abstract = Inline.RenderBlocks(Inline.Child(item, "content"), references, $"{itemPath}.content")
            };

            (member.Type, member.TypeId) = ReadType(Inline.Child(item, "type"), $"{itemPath}.type");
            ReadAttributes(Inline.Child(item, "attributes"), $"{itemPath}.attributes", member.Attributes);

            yield return member;
        }
    }

    private static IEnumerable<EnumValue> ReadValues(JsonElement section, JsonElement references, string path)
    {
        foreach (var (value, index) in Inline.Enumerate(Inline.Child(section, "values")))
        {
            var valuePath = $"{path}.values[{index}]";
            RequireKeys(value, ValueKeys, valuePath);

            yield return new EnumValue
            {
                Name = Inline.Text(value, "name"),
                Abstract = Inline.RenderBlocks(Inline.Child(value, "content"), references, $"{valuePath}.content"),
                Deprecated = Flag(value, "deprecated"),
                IntroducedVersion = Optional(value, "introducedVersion")
            };
        }
    }

    private static IEnumerable<Response> ReadResponses(JsonElement section, JsonElement references, string path)
    {
        foreach (var (item, index) in Inline.Enumerate(Inline.Child(section, "items")))
        {
            var itemPath = $"{path}.items[{index}]";
            RequireKeys(item, ResponseKeys, itemPath);

            var status = Inline.Child(item, "status");
            var response = new Response
            {
                Status = status.ValueKind == JsonValueKind.Number ? status.GetInt32() : 0,
                Reason = Optional(item, "reason"),
                MimeType = Optional(item, "mimeType"),
                Abstract = Inline.RenderBlocks(Inline.Child(item, "content"), references, $"{itemPath}.content")
            };

            var (type, typeId) = ReadType(Inline.Child(item, "type"), $"{itemPath}.type");
            response.Type = type.Length > 0 ? type : null;
            response.TypeId = typeId;

            yield return response;
        }
    }

    private static RequestBody ReadRequestBody(JsonElement section, JsonElement references, string path, RequestBody? existing)
    {
        if (existing is not null)
        {
            throw new SpecFormatException($"{path}: a second HTTP body section; the record holds only one.");
        }

        var parameters = Inline.Child(section, "parameters");

        if (parameters.ValueKind == JsonValueKind.Array && parameters.GetArrayLength() > 0)
        {
            throw new SpecFormatException($"{path}.parameters: expected to be empty, but the page fills it.");
        }

        var (type, typeId) = ReadType(Inline.Child(section, "bodyContentType"), $"{path}.bodyContentType");

        return new RequestBody
        {
            MimeType = Optional(section, "mimeType"),
            Type = type.Length > 0 ? type : null,
            TypeId = typeId,
            Abstract = Inline.RenderBlocks(Inline.Child(section, "content"), references, $"{path}.content")
        };
    }

    private static EndpointUrl ReadEndpoint(JsonElement section, string path)
    {
        var endpoint = new EndpointUrl { Title = Inline.Text(section, "title") };
        var method = new StringBuilder();
        var baseUrl = new StringBuilder();
        var template = new StringBuilder();

        foreach (var (token, index) in Inline.Enumerate(Inline.Child(section, "tokens")))
        {
            var kind = Inline.Text(token, "kind");

            if (!EndpointTokenKinds.Contains(kind))
            {
                throw new SpecFormatException($"{path}.tokens[{index}]: endpoint token kind \"{kind}\" is not handled.");
            }

            var text = Inline.Text(token, "text");

            switch (kind)
            {
                case "method":
                    method.Append(text);
                    break;

                case "baseURL":
                    baseUrl.Append(text);
                    break;

                case "parameter":
                    endpoint.Parameters.Add(text);
                    goto case "path";

                case "path":
                    template.Append(text);
                    break;

                case "text":
                    break;
            }
        }

        endpoint.Method = method.ToString();
        endpoint.BaseUrl = baseUrl.ToString();
        endpoint.Path = template.ToString();

        return endpoint;
    }

    private static (string Type, string? TypeId) ReadType(JsonElement tokens, string path)
    {
        var rendered = new StringBuilder();
        var identifiers = new List<string>();

        foreach (var (token, index) in Inline.Enumerate(tokens))
        {
            var kind = Inline.Text(token, "kind");

            if (!TypeTokenKinds.Contains(kind))
            {
                throw new SpecFormatException($"{path}[{index}]: type token kind \"{kind}\" is not handled.");
            }

            rendered.Append(Inline.Text(token, "text"));

            if (Optional(token, "preciseIdentifier") is { } precise)
            {
                identifiers.Add(precise);
            }
        }

        // A response type is often a union of error types, so TypeId joins every preciseIdentifier
        // rather than holding one.
        return (rendered.ToString(), identifiers.Count > 0 ? string.Join("|", identifiers) : null);
    }

    private static List<ArticleBlock> ReadArticle(JsonElement content, JsonElement references, string path)
    {
        var blocks = new List<ArticleBlock>();
        var current = new ArticleBlock();

        foreach (var (node, index) in Inline.Enumerate(content))
        {
            var nodePath = $"{path}[{index}]";
            var type = Inline.Text(node, "type");

            if (type == "heading")
            {
                if (current.Heading is not null || current.Paragraphs.Count > 0 || current.ListItems.Count > 0)
                {
                    blocks.Add(current);
                }

                var level = Inline.Child(node, "level");

                current = new ArticleBlock
                {
                    Level = level.ValueKind == JsonValueKind.Number ? level.GetInt32() : 0,
                    Heading = Inline.Text(node, "text")
                };

                continue;
            }

            if (type is "unorderedList" or "orderedList")
            {
                foreach (var (item, itemIndex) in Inline.Enumerate(Inline.Child(node, "items")))
                {
                    current.ListItems.Add(Inline.RenderBlocks(Inline.Child(item, "content"), references, $"{nodePath}.items[{itemIndex}].content"));
                }

                continue;
            }

            current.Paragraphs.Add(Inline.RenderBlock(node, references, nodePath));
        }

        if (current.Heading is not null || current.Paragraphs.Count > 0 || current.ListItems.Count > 0)
        {
            blocks.Add(current);
        }

        return blocks;
    }

    private static void ReadGroups(JsonElement sections, string path, List<TopicGroup> into)
    {
        foreach (var (section, index) in Inline.Enumerate(sections))
        {
            RequireKeys(section, TopicKeys, $"{path}[{index}]");

            into.Add(new TopicGroup
            {
                Title = Inline.Text(section, "title"),
                Members = Strings(Inline.Child(section, "identifiers"))
            });
        }
    }

    private static void RequireKeys(JsonElement node, HashSet<string> allowed, string path)
    {
        if (node.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        foreach (var property in node.EnumerateObject())
        {
            if (!allowed.Contains(property.Name))
            {
                throw new SpecFormatException($"{(path.Length > 0 ? path + "." : "")}{property.Name}: key is not handled.");
            }
        }
    }

    private static string Slug(JsonElement page)
    {
        var first = First(Inline.Child(First(Inline.Child(page, "variants")), "paths"));
        var path = first.ValueKind == JsonValueKind.String ? first.GetString() ?? "" : "";

        const string prefix = "/documentation/";
        return path.StartsWith(prefix, StringComparison.Ordinal) ? path[prefix.Length..] : path;
    }

    private static string DocumentationPath(string url)
    {
        var cut = url.IndexOf("/documentation/", StringComparison.Ordinal);
        return cut >= 0 ? url[cut..] : url;
    }

    private static JsonElement First(JsonElement array)
        => array.ValueKind == JsonValueKind.Array && array.GetArrayLength() > 0 ? array[0] : default;

    private static string? Optional(JsonElement node, string name)
    {
        var child = Inline.Child(node, name);
        return child.ValueKind == JsonValueKind.String ? child.GetString() : null;
    }

    private static bool Flag(JsonElement node, string name) => Inline.Child(node, name).ValueKind == JsonValueKind.True;

    private static List<string> Strings(JsonElement array)
    {
        var values = new List<string>();

        foreach (var (element, _) in Inline.Enumerate(array))
        {
            values.Add(element.ValueKind == JsonValueKind.String ? element.GetString() ?? "" : element.GetRawText());
        }

        return values;
    }
}
