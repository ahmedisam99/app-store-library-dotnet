using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that changes a win-back offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackofferupdaterequest/data"/>
public sealed class WinBackOfferUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>winBackOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "winBackOffers";

    /// <summary>
    /// The opaque resource ID of the win-back offer to change. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave one out to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public WinBackOfferUpdateRequestDataAttributes? Attributes { get; set; }
}
