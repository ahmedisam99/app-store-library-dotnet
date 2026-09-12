using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that schedules a subscription price.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricecreaterequest/data/relationships"/>
public sealed class SubscriptionPriceCreateRequestDataRelationships
{
    /// <summary>
    /// The related Subscriptions resource the price belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }

    /// <summary>
    /// The related Territories resource the price applies to. Leave it out to set the price in
    /// every territory the subscription is for sale in.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The related Subscription Price Points resource that sets the amount. This relationship is
    /// required.
    /// </summary>
    [JsonPropertyName("subscriptionPricePoint")]
    public required RelationshipDeclaration SubscriptionPricePoint { get; set; }
}
