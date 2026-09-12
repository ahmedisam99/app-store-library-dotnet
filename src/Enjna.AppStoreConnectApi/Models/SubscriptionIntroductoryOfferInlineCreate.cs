using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// An introductory offer you create inline, in the <c>included</c> array of the request body that
/// updates a subscription, and refer to from the request's <c>introductoryOffers</c> relationship
/// by the placeholder ID you give it here.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryofferinlinecreate"/>
public sealed class SubscriptionIntroductoryOfferInlineCreate : ISubscriptionUpdateRequestIncludedResource
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionIntroductoryOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionIntroductoryOffers";

    /// <summary>
    /// A temporary identifier you invent for this offer, such as <c>${offer1}</c>, and refer to
    /// from the <c>introductoryOffers</c> relationship of the subscription you're updating.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The dates the offer runs between, how long it lasts, and how it bills. This member is
    /// required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionIntroductoryOfferInlineCreateAttributes Attributes { get; set; }

    /// <summary>
    /// The subscription the offer discounts, the territory it applies in, and the price point that
    /// sets what it costs.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionIntroductoryOfferInlineCreateRelationships? Relationships { get; set; }
}
