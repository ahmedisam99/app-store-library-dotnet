using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to commit an In-App Purchase Images resource once you finish uploading
/// the image file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimageupdaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseImageV2UpdateRequest instead.")]
public sealed class InAppPurchaseImageUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseImageUpdateRequestData Data { get; set; }
}
