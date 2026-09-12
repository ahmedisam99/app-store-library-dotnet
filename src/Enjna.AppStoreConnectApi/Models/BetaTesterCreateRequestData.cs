using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a beta tester.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatestercreaterequest/data"/>
public sealed class BetaTesterCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaTesters</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaTesters";

    /// <summary>
    /// The attributes that describe the request that creates a Beta Testers resource. This property is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required BetaTesterCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you can set with this request.
    /// </summary>
    [JsonPropertyName("relationships")]
    public BetaTesterCreateRequestDataRelationships? Relationships { get; set; }
}
