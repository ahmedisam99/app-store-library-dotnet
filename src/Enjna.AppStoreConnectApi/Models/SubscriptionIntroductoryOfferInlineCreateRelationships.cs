using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of an introductory offer you create inline in a subscription update request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryofferinlinecreate/relationships"/>
public sealed class SubscriptionIntroductoryOfferInlineCreateRelationships
{
    /// <summary>
    /// The related Subscriptions resource the offer discounts.
    /// </summary>
    [JsonPropertyName("subscription")]
    public RelationshipDeclaration? Subscription { get; set; }

    /// <summary>
    /// The related Territories resource the offer applies to. Leave it out to apply the offer in
    /// every territory the subscription is for sale in.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The related Subscription Price Points resource that sets what the offer costs. A free trial
    /// doesn't need one.
    /// </summary>
    [JsonPropertyName("subscriptionPricePoint")]
    public RelationshipDeclaration? SubscriptionPricePoint { get; set; }
}
