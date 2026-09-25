using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Subscription Localizations resource of the
/// v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationv2createrequest/data/relationships"/>
public sealed class SubscriptionLocalizationV2CreateRequestDataRelationships
{
    /// <summary>
    /// The subscription version the localization belongs to, of type <c>subscriptionVersions</c>.
    /// The version must be in <c>PREPARE_FOR_SUBMISSION</c>. This relationship is required.
    /// </summary>
    [JsonPropertyName("version")]
    public required RelationshipDeclaration Version { get; set; }
}
