using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppleSpec;

public sealed class SpecPage
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";

    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("kind")] public string Kind { get; set; } = "";
    [JsonPropertyName("role")] public string Role { get; set; } = "";
    [JsonPropertyName("framework")] public string Framework { get; set; } = "";

    [JsonPropertyName("path")] public string Path { get; set; } = "";

    [JsonPropertyName("slug")] public string Slug { get; set; } = "";

    [JsonPropertyName("introducedAt")] public string? IntroducedAt { get; set; }
    [JsonPropertyName("deprecated")] public Deprecation? Deprecated { get; set; }
    [JsonPropertyName("beta")] public bool Beta { get; set; }
    [JsonPropertyName("abstract")] public Prose Abstract { get; set; } = Prose.Empty;

    [JsonPropertyName("declaration")] public Declaration? Declaration { get; set; }

    [JsonPropertyName("constraints")] public List<Constraint> Constraints { get; set; } = new();

    [JsonPropertyName("values")] public List<EnumValue>? Values { get; set; }
    [JsonPropertyName("properties")] public List<Member>? Properties { get; set; }
    [JsonPropertyName("endpoints")] public List<EndpointUrl>? Endpoints { get; set; }
    [JsonPropertyName("parameters")] public List<Member>? Parameters { get; set; }
    [JsonPropertyName("requestBody")] public RequestBody? RequestBody { get; set; }
    [JsonPropertyName("responses")] public List<Response>? Responses { get; set; }

    [JsonPropertyName("notes")] public List<Note> Notes { get; set; } = new();

    [JsonPropertyName("tables")] public List<Table> Tables { get; set; } = new();

    [JsonPropertyName("article")] public List<ArticleBlock>? Article { get; set; }

    [JsonPropertyName("topics")] public List<TopicGroup> Topics { get; set; } = new();
    [JsonPropertyName("seeAlso")] public List<TopicGroup> SeeAlso { get; set; } = new();
}

public sealed class Deprecation
{
    [JsonPropertyName("at")] public string? At { get; set; }
    [JsonPropertyName("summary")] public Prose? Summary { get; set; }
}

public sealed class Prose
{
    public static readonly Prose Empty = new();

    [JsonPropertyName("text")] public string Text { get; set; } = "";
    [JsonPropertyName("refs")] public List<string> Refs { get; set; } = new();
}

public sealed class Declaration
{
    [JsonPropertyName("tokens")] public string Tokens { get; set; } = "";

    [JsonPropertyName("baseType")] public string? BaseType { get; set; }
    [JsonPropertyName("isArray")] public bool IsArray { get; set; }
    [JsonPropertyName("elementType")] public string? ElementType { get; set; }
}

public sealed class Constraint
{
    [JsonPropertyName("kind")] public string Kind { get; set; } = "";
    [JsonPropertyName("value")] public string? Value { get; set; }
    [JsonPropertyName("values")] public List<string>? Values { get; set; }
}

public sealed class EnumValue
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("abstract")] public Prose Abstract { get; set; } = Prose.Empty;
    [JsonPropertyName("deprecated")] public bool Deprecated { get; set; }
    [JsonPropertyName("introducedVersion")] public string? IntroducedVersion { get; set; }
}

public sealed class Member
{
    [JsonPropertyName("in")] public string? In { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("type")] public string Type { get; set; } = "";
    [JsonPropertyName("typeId")] public string? TypeId { get; set; }
    [JsonPropertyName("required")] public bool Required { get; set; }
    [JsonPropertyName("deprecated")] public bool Deprecated { get; set; }
    [JsonPropertyName("introducedVersion")] public string? IntroducedVersion { get; set; }
    [JsonPropertyName("attributes")] public List<Constraint> Attributes { get; set; } = new();
    [JsonPropertyName("abstract")] public Prose Abstract { get; set; } = Prose.Empty;
}

public sealed class EndpointUrl
{
    [JsonPropertyName("title")] public string Title { get; set; } = "";

    [JsonPropertyName("method")] public string Method { get; set; } = "";
    [JsonPropertyName("baseUrl")] public string BaseUrl { get; set; } = "";
    [JsonPropertyName("path")] public string Path { get; set; } = "";
    [JsonPropertyName("parameters")] public List<string> Parameters { get; set; } = new();
}

public sealed class RequestBody
{
    [JsonPropertyName("mimeType")] public string? MimeType { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("typeId")] public string? TypeId { get; set; }
    [JsonPropertyName("abstract")] public Prose Abstract { get; set; } = Prose.Empty;
}

public sealed class Response
{
    [JsonPropertyName("status")] public int Status { get; set; }
    [JsonPropertyName("reason")] public string? Reason { get; set; }
    [JsonPropertyName("mimeType")] public string? MimeType { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("typeId")] public string? TypeId { get; set; }
    [JsonPropertyName("abstract")] public Prose Abstract { get; set; } = Prose.Empty;
}

public sealed class Note
{
    [JsonPropertyName("style")] public string Style { get; set; } = "";
    [JsonPropertyName("prose")] public Prose Prose { get; set; } = Prose.Empty;
}

public sealed class Table
{
    [JsonPropertyName("headers")] public List<string> Headers { get; set; } = new();
    [JsonPropertyName("rows")] public List<List<string>> Rows { get; set; } = new();
}

public sealed class ArticleBlock
{
    [JsonPropertyName("level")] public int Level { get; set; }

    [JsonPropertyName("heading")] public string? Heading { get; set; }
    [JsonPropertyName("paragraphs")] public List<Prose> Paragraphs { get; set; } = new();
    [JsonPropertyName("listItems")] public List<Prose> ListItems { get; set; } = new();
}

// Apple's "Errors to retry" group is the only statement of which errors are retryable; otherwise
// it is inferable only from a name suffix, which is a convention rather than a guarantee.
public sealed class TopicGroup
{
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("members")] public List<string> Members { get; set; } = new();
}

public sealed class Manifest
{
    [JsonPropertyName("sources")] public List<ManifestSource> Sources { get; set; } = new();
}

public sealed class ManifestSource
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("kind")] public string Kind { get; set; } = "";
    [JsonPropertyName("url")] public string Url { get; set; } = "";

    [JsonPropertyName("version")] public string? Version { get; set; }

    [JsonPropertyName("lastModified")] public string? LastModified { get; set; }
    [JsonPropertyName("etag")] public string? ETag { get; set; }
    [JsonPropertyName("pages")] public int Pages { get; set; }

    [JsonPropertyName("sha256")] public string Sha256 { get; set; } = "";
}

public static class Json
{
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };
}
