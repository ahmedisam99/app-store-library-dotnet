using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that updates an app tag.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/apptagupdaterequest/data-data.dictionary"/>
public sealed class AppTagUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>appTags</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "appTags";

    /// <summary>
    /// The opaque resource ID of the app tag to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public AppTagUpdateRequestDataAttributes? Attributes { get; set; }
}
