using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a promotional offer you create inline in a subscription update request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferinlinecreate/relationships"/>
public sealed class SubscriptionPromotionalOfferInlineCreateRelationships
{
    /// <summary>
    /// The related Subscriptions resource the offer discounts.
    /// </summary>
    [JsonPropertyName("subscription")]
    public RelationshipDeclaration? Subscription { get; set; }

    /// <summary>
    /// The prices the offer charges, one per territory. Point at the IDs you gave the prices in
    /// the request's <c>included</c> array.
    /// </summary>
    [JsonPropertyName("prices")]
    public RelationshipDeclarationList? Prices { get; set; }
}
