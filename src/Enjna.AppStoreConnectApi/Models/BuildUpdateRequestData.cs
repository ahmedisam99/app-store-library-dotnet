using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data of a request that updates a Builds resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildupdaterequest/data"/>
public sealed class BuildUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>builds</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "builds";

    /// <summary>
    /// The opaque resource ID of the build to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Builds resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public BuildUpdateRequestDataAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you can set with this request.
    /// </summary>
    [JsonPropertyName("relationships")]
    public BuildUpdateRequestDataRelationships? Relationships { get; set; }
}
