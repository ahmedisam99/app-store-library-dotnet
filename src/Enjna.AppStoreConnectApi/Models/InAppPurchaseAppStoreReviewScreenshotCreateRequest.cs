using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve an In-App Purchase App Store Review Screenshots resource
/// before you upload the screenshot file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshotcreaterequest"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseAppStoreReviewScreenshotCreateRequestData Data { get; set; }
}
