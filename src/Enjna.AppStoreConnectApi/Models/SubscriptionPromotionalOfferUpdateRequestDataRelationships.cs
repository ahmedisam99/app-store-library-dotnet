using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that changes a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferupdaterequest/data/relationships"/>
public sealed class SubscriptionPromotionalOfferUpdateRequestDataRelationships
{
    /// <summary>
    /// The prices the offer charges, one per territory. Point at the IDs you gave the prices in
    /// the request's <c>included</c> array.
    /// </summary>
    [JsonPropertyName("prices")]
    public RelationshipDeclarationList? Prices { get; set; }
}
