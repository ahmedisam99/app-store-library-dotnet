using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a win-back offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffercreaterequest/data"/>
public sealed class WinBackOfferCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>winBackOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "winBackOffers";

    /// <summary>
    /// The attributes that describe the request. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required WinBackOfferCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required WinBackOfferCreateRequestDataRelationships Relationships { get; set; }
}
