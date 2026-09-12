using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that reserves a subscription App Review screenshot.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotcreaterequest/data/attributes"/>
public sealed class SubscriptionAppStoreReviewScreenshotCreateRequestDataAttributes
{
    /// <summary>
    /// The size of the screenshot file, in bytes. This value is required.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public required long FileSize { get; set; }

    /// <summary>
    /// The file name of the screenshot you're about to upload. This value is required.
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; set; }
}
