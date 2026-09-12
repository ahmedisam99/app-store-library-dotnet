using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a Promoted Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchasecreaterequest/data/attributes"/>
public sealed class PromotedPurchaseCreateRequestDataAttributes
{
    /// <summary>
    /// Whether every customer sees the promotion, rather than only the customers your app reorders
    /// or hides it for through StoreKit. This attribute is required.
    /// </summary>
    [JsonPropertyName("visibleForAllUsers")]
    public required bool VisibleForAllUsers { get; set; }

    /// <summary>
    /// Whether the promotion is turned on.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
}
