using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseversioncreaterequest/data/relationships"/>
public sealed class InAppPurchaseVersionCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase the version belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchase")]
    public required RelationshipDeclaration InAppPurchase { get; set; }
}
