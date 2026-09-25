using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationcreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseLocalizationV2CreateRequestData instead.")]
public sealed class InAppPurchaseLocalizationCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseLocalizationCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseLocalizationCreateRequestDataRelationships Relationships { get; set; }
}
