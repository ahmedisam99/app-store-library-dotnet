using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a v2 In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2createrequest/data/relationships"/>
public sealed class InAppPurchaseLocalizationV2CreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase version the localization belongs to, of type
    /// <c>inAppPurchaseVersions</c>. This relationship is required.
    /// </summary>
    [JsonPropertyName("version")]
    public required RelationshipDeclaration Version { get; set; }
}
