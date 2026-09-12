using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionaloffercreaterequest/data/relationships"/>
public sealed class SubscriptionPromotionalOfferCreateRequestDataRelationships
{
    /// <summary>
    /// The related Subscriptions resource the offer discounts. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }

    /// <summary>
    /// The prices the offer charges, one per territory. Point at the IDs you gave the prices in
    /// the request's <c>included</c> array. This relationship is required.
    /// </summary>
    [JsonPropertyName("prices")]
    public required RelationshipDeclarationList Prices { get; set; }
}
