using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that reserves an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagecreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseImageV2CreateRequestDataRelationships instead.")]
public sealed class InAppPurchaseImageCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase the image belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchase")]
    public required RelationshipDeclaration InAppPurchase { get; set; }
}
