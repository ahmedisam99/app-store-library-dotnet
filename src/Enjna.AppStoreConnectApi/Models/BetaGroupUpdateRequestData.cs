using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupupdaterequest/data"/>
public sealed class BetaGroupUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaGroups</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaGroups";

    /// <summary>
    /// The opaque resource ID of the beta group you're updating. This property is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Beta Groups resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public BetaGroupUpdateRequestDataAttributes? Attributes { get; set; }
}
