using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to change the prices of a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferupdaterequest"/>
public sealed class SubscriptionPromotionalOfferUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionPromotionalOfferUpdateRequestData Data { get; set; }

    /// <summary>
    /// The prices the <c>prices</c> relationship of this request points at.
    /// </summary>
    [JsonPropertyName("included")]
    public SubscriptionPromotionalOfferPriceInlineCreate[]? Included { get; set; }
}
