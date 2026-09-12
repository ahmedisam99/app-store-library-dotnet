using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupcreaterequest/data"/>
public sealed class BetaGroupCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaGroups</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaGroups";

    /// <summary>
    /// The attributes that describe the request that creates a Beta Groups resource. This property is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required BetaGroupCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you can set with this request. This property is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required BetaGroupCreateRequestDataRelationships Relationships { get; set; }
}
