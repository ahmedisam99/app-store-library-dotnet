using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that creates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationcreaterequest/data/relationships"/>
public sealed class SubscriptionLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
