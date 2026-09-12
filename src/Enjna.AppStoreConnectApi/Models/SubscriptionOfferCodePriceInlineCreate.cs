using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A price you create inline, in the <c>included</c> array of the request body that creates a
/// Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecreaterequest"/>
public sealed class SubscriptionOfferCodePriceInlineCreate
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodePrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodePrices";

    /// <summary>
    /// A temporary identifier you invent for this price, such as <c>${price1}</c>, and refer to
    /// from the <c>prices</c> relationship of the offer code you're creating.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The relationships that place the price in a territory and at a price point.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionOfferCodePriceInlineCreateRelationships? Relationships { get; set; }
}
