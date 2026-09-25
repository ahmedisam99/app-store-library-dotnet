using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve an In-App Purchase Images resource before you upload the
/// image file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagecreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseImageV2CreateRequest instead.")]
public sealed class InAppPurchaseImageCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseImageCreateRequestData Data { get; set; }
}
