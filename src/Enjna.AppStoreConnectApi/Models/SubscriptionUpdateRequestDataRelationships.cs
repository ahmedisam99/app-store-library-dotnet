using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that updates an auto-renewable subscription. Each one you set
/// replaces the whole existing set of related resources.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionupdaterequest/data/relationships"/>
public sealed class SubscriptionUpdateRequestDataRelationships
{
    /// <summary>
    /// The introductory offers of the subscription.
    /// </summary>
    [JsonPropertyName("introductoryOffers")]
    public RelationshipDeclarationList? IntroductoryOffers { get; set; }

    /// <summary>
    /// The promotional offers of the subscription.
    /// </summary>
    [JsonPropertyName("promotionalOffers")]
    public RelationshipDeclarationList? PromotionalOffers { get; set; }

    /// <summary>
    /// The prices of the subscription.
    /// </summary>
    [JsonPropertyName("prices")]
    public RelationshipDeclarationList? Prices { get; set; }
}
