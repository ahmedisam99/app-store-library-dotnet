using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to commit an In-App Purchase App Store Review Screenshots resource
/// once you finish uploading the screenshot file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshotupdaterequest"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseAppStoreReviewScreenshotUpdateRequestData Data { get; set; }
}
