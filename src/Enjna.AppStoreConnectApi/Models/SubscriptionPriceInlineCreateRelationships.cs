using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a price you create inline in a subscription update request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpriceinlinecreate/relationships"/>
public sealed class SubscriptionPriceInlineCreateRelationships
{
    /// <summary>
    /// The related Subscriptions resource the price belongs to.
    /// </summary>
    [JsonPropertyName("subscription")]
    public RelationshipDeclaration? Subscription { get; set; }

    /// <summary>
    /// The related Territories resource the price applies to. Leave it out to set the price in
    /// every territory the subscription is for sale in.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The related Subscription Price Points resource that sets the amount.
    /// </summary>
    [JsonPropertyName("subscriptionPricePoint")]
    public RelationshipDeclaration? SubscriptionPricePoint { get; set; }
}
