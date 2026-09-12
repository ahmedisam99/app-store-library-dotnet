using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an In-App Purchase Offer Code One-Time Use
/// Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodeOneTimeUseCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodeOneTimeUseCodes";

    /// <summary>
    /// The opaque resource ID of the batch of codes you're updating. It is required, and matches
    /// the ID in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates an In-App Purchase Offer Code One-Time
    /// Use Codes resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequestDataAttributes? Attributes { get; set; }
}
