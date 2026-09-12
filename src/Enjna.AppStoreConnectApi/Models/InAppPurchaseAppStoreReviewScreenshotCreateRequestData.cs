using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that reserves an In-App Purchase App Store Review
/// Screenshots resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshotcreaterequest/data"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseAppStoreReviewScreenshots</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseAppStoreReviewScreenshots";

    /// <summary>
    /// The attributes that describe the screenshot to reserve. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseAppStoreReviewScreenshotCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseAppStoreReviewScreenshotCreateRequestDataRelationships Relationships { get; set; }
}
