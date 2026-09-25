using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Images resource of the v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2/attributes"/>
public sealed class SubscriptionImageV2Attributes
{
    /// <summary>
    /// The size of the image file, in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The file name of the image.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// An opaque token that identifies the reserved asset.
    /// </summary>
    [JsonPropertyName("assetToken")]
    public string? AssetToken { get; set; }

    /// <summary>
    /// The image itself, once App Store Connect finished processing the upload.
    /// </summary>
    [JsonPropertyName("imageAsset")]
    public ImageAsset? ImageAsset { get; set; }

    /// <summary>
    /// The operations that upload the image's bytes. Pass them to
    /// <see cref="AppStoreConnectAPIClient.UploadAssetAsync(System.Collections.Generic.IEnumerable{UploadOperation}, byte[], System.Threading.CancellationToken)"/>.
    /// </summary>
    [JsonPropertyName("uploadOperations")]
    public UploadOperation[]? UploadOperations { get; set; }

    /// <summary>
    /// The delivery state of the asset, including any errors and warnings App Store Connect raised
    /// while it processed the image.
    /// </summary>
    [JsonPropertyName("assetDeliveryState")]
    public AppMediaAssetState? AssetDeliveryState { get; set; }
}
