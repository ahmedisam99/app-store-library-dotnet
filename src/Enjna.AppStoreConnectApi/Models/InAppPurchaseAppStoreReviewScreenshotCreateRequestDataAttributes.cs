using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that reserves an In-App Purchase App Store Review
/// Screenshots resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshotcreaterequest/data/attributes"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotCreateRequestDataAttributes
{
    /// <summary>
    /// The name of the screenshot file you are about to upload, including its extension. This
    /// attribute is required.
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; set; }

    /// <summary>
    /// The size of the screenshot file, in bytes. App Store Connect uses it to decide how to split
    /// the upload. This attribute is required.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public required long FileSize { get; set; }
}
