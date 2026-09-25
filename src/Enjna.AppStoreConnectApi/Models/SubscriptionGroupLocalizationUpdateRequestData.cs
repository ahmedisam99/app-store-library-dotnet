using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that updates a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationupdaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2UpdateRequestData instead.")]
public sealed class SubscriptionGroupLocalizationUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupLocalizations";

    /// <summary>
    /// The opaque resource ID of the localization to update. It is required, and matches the ID in
    /// the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionGroupLocalizationUpdateRequestDataAttributes? Attributes { get; set; }
}
