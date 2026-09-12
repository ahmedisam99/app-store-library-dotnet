using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2/attributes"/>
public sealed class InAppPurchaseV2Attributes
{
    /// <summary>
    /// The reference name of the in-app purchase. It appears in App Store Connect and in Sales and
    /// Trends reports, and customers never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The product identifier your app passes to StoreKit to request the in-app purchase. It is
    /// unique across your app and you can't change it once you create the in-app purchase.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    /// <summary>
    /// The kind of in-app purchase, which decides how customers can buy it and how often.
    /// </summary>
    [JsonPropertyName("inAppPurchaseType")]
    public InAppPurchaseType? InAppPurchaseType { get; set; }

    /// <summary>
    /// Where the in-app purchase sits in the App Store Connect submission and review workflow.
    /// </summary>
    [JsonPropertyName("state")]
    public InAppPurchaseState? State { get; set; }

    /// <summary>
    /// The note you write for App Review, explaining how to reach and test the in-app purchase.
    /// </summary>
    [JsonPropertyName("reviewNote")]
    public string? ReviewNote { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the in-app purchase with the
    /// rest of their family group through Family Sharing.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether Apple hosts the downloadable content that comes with
    /// the in-app purchase.
    /// </summary>
    [JsonPropertyName("contentHosting")]
    public bool? ContentHosting { get; set; }
}
