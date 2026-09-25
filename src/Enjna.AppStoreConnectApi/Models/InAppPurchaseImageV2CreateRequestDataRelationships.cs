using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that reserves a v2 In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2createrequest/data/relationships"/>
public sealed class InAppPurchaseImageV2CreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase version the image belongs to, of type <c>inAppPurchaseVersions</c>. This
    /// relationship is required.
    /// </summary>
    [JsonPropertyName("version")]
    public required RelationshipDeclaration Version { get; set; }
}
