using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase App Store Review Screenshots resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseappstorereviewscreenshot/attributes"/>
public sealed class InAppPurchaseAppStoreReviewScreenshotAttributes
{
    /// <summary>
    /// The name of the screenshot file, including its extension.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// The size of the screenshot file, in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The MD5 checksum of the file you uploaded. Send it when you commit the upload so App Store
    /// Connect can confirm it received the file intact.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get; set; }

    /// <summary>
    /// The processed screenshot, once App Store Connect finishes with it.
    /// </summary>
    [JsonPropertyName("imageAsset")]
    public ImageAsset? ImageAsset { get; set; }

    /// <summary>
    /// The token that identifies the uploaded asset.
    /// </summary>
    [JsonPropertyName("assetToken")]
    public string? AssetToken { get; set; }

    /// <summary>
    /// The kind of asset the screenshot is.
    /// </summary>
    [JsonPropertyName("assetType")]
    public string? AssetType { get; set; }

    /// <summary>
    /// The parts to split the screenshot file into, and the request to send each one with. App
    /// Store Connect returns them when you reserve the screenshot.
    /// </summary>
    [JsonPropertyName("uploadOperations")]
    public UploadOperation[]? UploadOperations { get; set; }

    /// <summary>
    /// How far the upload got, and anything App Store Connect flagged while it processed the file.
    /// </summary>
    [JsonPropertyName("assetDeliveryState")]
    public AppMediaAssetState? AssetDeliveryState { get; set; }
}
