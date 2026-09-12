using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodes";

    /// <summary>
    /// The opaque resource ID of the offer code you're updating. It is required, and matches the ID
    /// in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates an In-App Purchase Offer Codes
    /// resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseOfferCodeUpdateRequestDataAttributes? Attributes { get; set; }
}
