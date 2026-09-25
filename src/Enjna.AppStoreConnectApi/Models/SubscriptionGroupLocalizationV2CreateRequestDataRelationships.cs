using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a subscription group localization on a
/// subscription group version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2createrequest/data/relationships"/>
public sealed class SubscriptionGroupLocalizationV2CreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group version the localization belongs to, of type
    /// <c>subscriptionGroupVersions</c>, rather than the subscription group itself. This
    /// relationship is required.
    /// </summary>
    [JsonPropertyName("version")]
    public required RelationshipDeclaration Version { get; set; }
}
