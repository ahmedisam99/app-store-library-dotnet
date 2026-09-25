using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that reserves an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagecreaterequest/data/attributes"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseImageV2CreateRequestDataAttributes instead.")]
public sealed class InAppPurchaseImageCreateRequestDataAttributes
{
    /// <summary>
    /// The name of the image file you are about to upload, including its extension. This attribute
    /// is required.
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; set; }

    /// <summary>
    /// The size of the image file, in bytes. App Store Connect uses it to decide how to split the
    /// upload. This attribute is required.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public required long FileSize { get; set; }
}
