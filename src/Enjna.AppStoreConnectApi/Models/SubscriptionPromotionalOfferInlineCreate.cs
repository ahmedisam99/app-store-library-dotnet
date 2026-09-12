using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A promotional offer you create inline, in the <c>included</c> array of the request body that
/// updates a subscription, and refer to from the request's <c>promotionalOffers</c> relationship by
/// the placeholder ID you give it here.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferinlinecreate"/>
public sealed class SubscriptionPromotionalOfferInlineCreate : ISubscriptionUpdateRequestIncludedResource
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPromotionalOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPromotionalOffers";

    /// <summary>
    /// A temporary identifier you invent for this offer, such as <c>${offer1}</c>, and refer to
    /// from the <c>promotionalOffers</c> relationship of the subscription you're updating.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The name and offer code of the offer, how long it lasts, and how it bills. This member is
    /// required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionPromotionalOfferInlineCreateAttributes Attributes { get; set; }

    /// <summary>
    /// The subscription the offer discounts and the prices it charges.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionPromotionalOfferInlineCreateRelationships? Relationships { get; set; }
}
