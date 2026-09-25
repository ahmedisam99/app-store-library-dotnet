using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a v2 In-App Purchase Images resource. Unlike the v1 image, it
/// carries no review state and no checksum; its upload progress is in
/// <see cref="AssetDeliveryState"/>.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2/attributes"/>
public sealed class InAppPurchaseImageV2Attributes
{
    /// <summary>
    /// The name of the image file, including its extension.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// The size of the image file, in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The token that identifies the uploaded asset.
    /// </summary>
    [JsonPropertyName("assetToken")]
    public string? AssetToken { get; set; }

    /// <summary>
    /// The processed image, once App Store Connect finishes with it.
    /// </summary>
    [JsonPropertyName("imageAsset")]
    public ImageAsset? ImageAsset { get; set; }

    /// <summary>
    /// The parts to split the image file into, and the request to send each one with. App Store
    /// Connect returns them when you reserve the image.
    /// </summary>
    [JsonPropertyName("uploadOperations")]
    public UploadOperation[]? UploadOperations { get; set; }

    /// <summary>
    /// How far the upload got, and anything App Store Connect flagged while it processed the file.
    /// </summary>
    [JsonPropertyName("assetDeliveryState")]
    public AppMediaAssetState? AssetDeliveryState { get; set; }
}
