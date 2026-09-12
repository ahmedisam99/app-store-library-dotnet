using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a Promoted Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchaseupdaterequest/data/attributes"/>
public sealed class PromotedPurchaseUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Whether every customer sees the promotion, rather than only the customers your app reorders
    /// or hides it for through StoreKit.
    /// </summary>
    [JsonPropertyName("visibleForAllUsers")]
    public bool? VisibleForAllUsers { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// Whether the promotion is turned on.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get => Get<bool?>(); set => Set(value); }
}
