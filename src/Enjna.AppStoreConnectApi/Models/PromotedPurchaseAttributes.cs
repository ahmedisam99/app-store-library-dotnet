using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Promoted Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchase/attributes"/>
public sealed class PromotedPurchaseAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether every customer sees the promotion, rather than only
    /// the customers your app reorders or hides it for through StoreKit.
    /// </summary>
    [JsonPropertyName("visibleForAllUsers")]
    public bool? VisibleForAllUsers { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the promotion is turned on.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Where the promotion sits in the App Review workflow.
    /// </summary>
    [JsonPropertyName("state")]
    public PromotedPurchaseState? State { get; set; }
}
