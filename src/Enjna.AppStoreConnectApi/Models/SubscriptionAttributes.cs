using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscriptions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscription/attributes"/>
public sealed class SubscriptionAttributes
{
    /// <summary>
    /// The reference name of the subscription. It appears in App Store Connect and in your sales
    /// and trends reports, and customers never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The unique product identifier of the subscription, which your app passes to StoreKit and
    /// which you can't change once you create the subscription.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the subscription with the
    /// members of their Family Sharing group.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get; set; }

    /// <summary>
    /// The review and availability state of the subscription.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionState? State { get; set; }

    /// <summary>
    /// The length of a single billing period of the subscription.
    /// </summary>
    [JsonPropertyName("subscriptionPeriod")]
    public SubscriptionPeriod? SubscriptionPeriod { get; set; }

    /// <summary>
    /// A note for App Review that explains how to reach and verify the subscription in your app.
    /// </summary>
    [JsonPropertyName("reviewNote")]
    public string? ReviewNote { get; set; }

    /// <summary>
    /// The rank of the subscription within its subscription group. Level 1 is the highest level of
    /// service, so a customer who moves to a higher level number downgrades.
    /// </summary>
    [JsonPropertyName("groupLevel")]
    public int? GroupLevel { get; set; }
}
