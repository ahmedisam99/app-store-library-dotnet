using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationcreaterequest/data/relationships"/>
public sealed class InAppPurchaseLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public required RelationshipDeclaration InAppPurchaseV2 { get; set; }
}
