using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that creates an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptioncreaterequest/data/attributes"/>
public sealed class SubscriptionCreateRequestDataAttributes
{
    /// <summary>
    /// The reference name of the subscription, which appears in App Store Connect and in your sales
    /// and trends reports. This value is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The unique product identifier your app passes to StoreKit. You can't change it later. This
    /// value is required.
    /// </summary>
    [JsonPropertyName("productId")]
    public required string ProductId { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the subscription with the
    /// members of their Family Sharing group.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get; set; }

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
    /// The rank of the subscription within its subscription group, where level 1 is the highest
    /// level of service.
    /// </summary>
    [JsonPropertyName("groupLevel")]
    public int? GroupLevel { get; set; }
}
