using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationcreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseLocalizationV2CreateRequest instead.")]
public sealed class InAppPurchaseLocalizationCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseLocalizationCreateRequestData Data { get; set; }
}
