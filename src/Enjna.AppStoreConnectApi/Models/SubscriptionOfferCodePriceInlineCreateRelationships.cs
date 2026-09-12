using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a price you create inline with a Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecreaterequest"/>
public sealed class SubscriptionOfferCodePriceInlineCreateRelationships
{
    /// <summary>
    /// The Territories resource the price applies in.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The Subscription Price Points resource that sets the discounted price in that territory.
    /// </summary>
    [JsonPropertyName("subscriptionPricePoint")]
    public RelationshipDeclaration? SubscriptionPricePoint { get; set; }
}
