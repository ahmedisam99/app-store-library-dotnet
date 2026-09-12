using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationship between a resource and one or more related resources.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/relationshiplinks"/>
public sealed class Relationship
{
    /// <summary>
    /// The links to the relationship itself and to the related resources.
    /// </summary>
    [JsonPropertyName("links")]
    public RelationshipLinks? Links { get; set; }

    /// <summary>
    /// The paging information for a to-many relationship.
    /// </summary>
    [JsonPropertyName("meta")]
    public PagingMeta? Meta { get; set; }

    /// <summary>
    /// The raw relationship data, which Apple calls the linkage. It is a single resource identifier
    /// for a to-one relationship, an array of resource identifiers for a to-many relationship,
    /// <see cref="JsonValueKind.Null"/> when the relationship is included but links to nothing, and
    /// <c>null</c> when the response doesn't include the relationship at all.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonConverter(typeof(LinkageConverter))]
    public JsonElement? Data { get; set; }

    /// <summary>
    /// A value indicating whether the response carried linkage for this relationship, that is,
    /// whether it carried a <c>data</c> member at all. Apple omits <c>data</c> from a relationship
    /// the request didn't ask for, so this is <c>false</c> for a relationship that simply wasn't
    /// requested and <c>true</c> for one the response actually answered, including the
    /// <c>data: null</c> answer that <see cref="HasNullLinkage"/> reports.
    /// </summary>
    [JsonIgnore]
    public bool IncludesLinkage => Data.HasValue;

    /// <summary>
    /// A value indicating whether the response carried an explicitly null linkage, that is,
    /// <c>data: null</c>. Apple sends that for a to-one relationship that exists on the resource but
    /// isn't set yet, such as an in-app purchase without an availability, so this is the answer to
    /// check before creating the related resource. It is <c>false</c> when the response doesn't
    /// include the relationship at all, which <see cref="IncludesLinkage"/> reports.
    /// </summary>
    [JsonIgnore]
    public bool HasNullLinkage => Data is { ValueKind: JsonValueKind.Null };

    /// <summary>
    /// Reads the relationship as a to-one relationship.
    /// </summary>
    /// <returns>
    /// The single related resource identifier, or <c>null</c> for every other wire state: the
    /// response didn't include the relationship, the response included it as <c>data: null</c>, or
    /// the relationship is a to-many one. Check <see cref="IncludesLinkage"/> and
    /// <see cref="HasNullLinkage"/>, or read <see cref="Data"/> directly, to tell those apart —
    /// <c>null</c> alone doesn't mean the related resource doesn't exist.
    /// </returns>
    public ResourceIdentifier? ToOne()
    {
        if (Data is not { ValueKind: JsonValueKind.Object } data)
        {
            return null;
        }

        return data.Deserialize<ResourceIdentifier>();
    }

    /// <summary>
    /// Reads the relationship as a to-many relationship.
    /// </summary>
    /// <returns>
    /// The related resource identifiers, which is an empty array for every other wire state: the
    /// response didn't include the relationship, the response included it as an empty array or as
    /// <c>data: null</c>, or the relationship is a to-one one. Check <see cref="IncludesLinkage"/>
    /// and <see cref="HasNullLinkage"/>, or read <see cref="Data"/> directly, to tell those apart —
    /// an empty array alone doesn't mean there are no related resources.
    /// </returns>
    public ResourceIdentifier[] ToMany()
    {
        if (Data is not { ValueKind: JsonValueKind.Array } data)
        {
            return Array.Empty<ResourceIdentifier>();
        }

        var identifiers = new List<ResourceIdentifier>();

        foreach (var element in data.EnumerateArray())
        {
            var identifier = element.Deserialize<ResourceIdentifier>();

            if (identifier is not null)
            {
                identifiers.Add(identifier);
            }
        }

        return identifiers.ToArray();
    }

    /// <summary>
    /// Reads <c>data</c> into a <see cref="JsonElement"/> that keeps an explicit null as
    /// <see cref="JsonValueKind.Null"/>. Without it, the serializer turns both an explicit
    /// <c>data: null</c> and an absent <c>data</c> member into a <see cref="JsonElement"/> without
    /// a value, which makes "links to nothing" indistinguishable from "wasn't requested".
    /// </summary>
    private sealed class LinkageConverter : JsonConverter<JsonElement?>
    {
        public override bool HandleNull => true;

        public override JsonElement? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return JsonElement.ParseValue(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, JsonElement? value, JsonSerializerOptions options)
        {
            if (value is { ValueKind: not JsonValueKind.Undefined } element)
            {
                element.WriteTo(writer);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
