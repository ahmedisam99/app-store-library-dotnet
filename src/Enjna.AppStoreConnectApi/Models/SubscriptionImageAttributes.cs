using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimage/attributes"/>
public sealed class SubscriptionImageAttributes
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
    /// The MD5 checksum of the image file that you sent when you committed the upload.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get; set; }

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
    /// The upload and review state of the image.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionImageState? State { get; set; }
}
