using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that reserves an In-App Purchase App Store Review
/// Screenshots resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshotcreaterequest/data/relationships"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase the screenshot belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public required RelationshipDeclaration InAppPurchaseV2 { get; set; }
}
