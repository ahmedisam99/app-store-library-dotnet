using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription App Store Review Screenshots resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshot/attributes"/>
public sealed class SubscriptionAppStoreReviewScreenshotAttributes
{
    /// <summary>
    /// The size of the screenshot file, in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The file name of the screenshot.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// The MD5 checksum of the screenshot file that you sent when you committed the upload.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get; set; }

    /// <summary>
    /// The screenshot itself, once App Store Connect finished processing the upload.
    /// </summary>
    [JsonPropertyName("imageAsset")]
    public ImageAsset? ImageAsset { get; set; }

    /// <summary>
    /// An opaque token that identifies the reserved asset.
    /// </summary>
    [JsonPropertyName("assetToken")]
    public string? AssetToken { get; set; }

    /// <summary>
    /// The type of the asset.
    /// </summary>
    [JsonPropertyName("assetType")]
    public string? AssetType { get; set; }

    /// <summary>
    /// The operations that upload the screenshot's bytes. Pass them to
    /// <see cref="AppStoreConnectAPIClient.UploadAssetAsync(System.Collections.Generic.IEnumerable{UploadOperation}, byte[], System.Threading.CancellationToken)"/>.
    /// </summary>
    [JsonPropertyName("uploadOperations")]
    public UploadOperation[]? UploadOperations { get; set; }

    /// <summary>
    /// The delivery state of the asset, including any errors and warnings App Store Connect raised
    /// while it processed the screenshot.
    /// </summary>
    [JsonPropertyName("assetDeliveryState")]
    public AppMediaAssetState? AssetDeliveryState { get; set; }
}
