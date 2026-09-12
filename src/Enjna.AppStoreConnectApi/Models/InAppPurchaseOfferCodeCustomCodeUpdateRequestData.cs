using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an In-App Purchase Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodeCustomCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodeCustomCodes";

    /// <summary>
    /// The opaque resource ID of the custom code you're updating. It is required, and matches the
    /// ID in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates an In-App Purchase Offer Code Custom
    /// Codes resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseOfferCodeCustomCodeUpdateRequestDataAttributes? Attributes { get; set; }
}
