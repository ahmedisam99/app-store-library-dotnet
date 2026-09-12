using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a Subscription Offer Codes resource. Declare a price for
/// every territory the offer is available in: reference each price from the <c>prices</c>
/// relationship, and describe it in <see cref="Included"/> under the same temporary identifier.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecreaterequest"/>
public sealed class SubscriptionOfferCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionOfferCodeCreateRequestData Data { get; set; }

    /// <summary>
    /// The prices you create along with the offer code, each identified by a temporary identifier
    /// that the <c>prices</c> relationship refers to.
    /// </summary>
    [JsonPropertyName("included")]
    public SubscriptionOfferCodePriceInlineCreate[]? Included { get; set; }
}
