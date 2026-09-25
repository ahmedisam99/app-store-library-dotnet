using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationcreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseLocalizationV2CreateRequestDataRelationships instead.")]
public sealed class InAppPurchaseLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public required RelationshipDeclaration InAppPurchaseV2 { get; set; }
}
