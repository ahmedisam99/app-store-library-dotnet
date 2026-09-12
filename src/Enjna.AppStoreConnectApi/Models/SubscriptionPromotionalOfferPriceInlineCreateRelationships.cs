using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a promotional offer price sent in the <c>included</c> array of a request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferpriceinlinecreate"/>
public sealed class SubscriptionPromotionalOfferPriceInlineCreateRelationships
{
    /// <summary>
    /// The related Territories resource the price applies to.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The related Subscription Price Points resource that sets the amount.
    /// </summary>
    [JsonPropertyName("subscriptionPricePoint")]
    public RelationshipDeclaration? SubscriptionPricePoint { get; set; }
}
