using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A promotional offer price sent in the <c>included</c> array of a request, so that the <c>prices</c>
/// relationship of the same request body can point at it. Give it an ID of your own, such as
/// <c>${price-us}</c>, and reference that ID from the relationship.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferpriceinlinecreate"/>
public sealed class SubscriptionPromotionalOfferPriceInlineCreate
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPromotionalOfferPrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPromotionalOfferPrices";

    /// <summary>
    /// The ID that the <c>prices</c> relationship of the request uses to refer to this price.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The territory this price applies to, and the price point that sets the amount.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionPromotionalOfferPriceInlineCreateRelationships? Relationships { get; set; }
}
