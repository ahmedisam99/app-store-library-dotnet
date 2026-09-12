using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set on a request that creates a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationcreaterequest/data/relationships"/>
public sealed class SubscriptionGroupLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscriptionGroup")]
    public required RelationshipDeclaration SubscriptionGroup { get; set; }
}
