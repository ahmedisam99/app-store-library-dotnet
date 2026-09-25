using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimage/attributes"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use InAppPurchaseImageV2Attributes instead.")]
public sealed class InAppPurchaseImageAttributes
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
    /// The MD5 checksum of the file you uploaded. Send it when you commit the upload so App Store
    /// Connect can confirm it received the file intact.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get; set; }

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
    /// Where the image sits in the upload and review workflow.
    /// </summary>
    [JsonPropertyName("state")]
    public InAppPurchaseImageState? State { get; set; }
}
