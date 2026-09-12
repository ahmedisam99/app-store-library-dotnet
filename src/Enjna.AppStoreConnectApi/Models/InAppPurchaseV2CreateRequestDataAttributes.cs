using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2createrequest/data/attributes"/>
public sealed class InAppPurchaseV2CreateRequestDataAttributes
{
    /// <summary>
    /// The reference name that identifies the in-app purchase in App Store Connect and in Sales and
    /// Trends reports. This attribute is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The product identifier your app passes to StoreKit. You can't change it later, so choose it
    /// carefully. This attribute is required.
    /// </summary>
    [JsonPropertyName("productId")]
    public required string ProductId { get; set; }

    /// <summary>
    /// The kind of in-app purchase to create. This attribute is required.
    /// </summary>
    [JsonPropertyName("inAppPurchaseType")]
    public required InAppPurchaseType InAppPurchaseType { get; set; }

    /// <summary>
    /// The note that tells App Review how to reach and test the in-app purchase.
    /// </summary>
    [JsonPropertyName("reviewNote")]
    public string? ReviewNote { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the in-app purchase with their
    /// family group through Family Sharing.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get; set; }
}
