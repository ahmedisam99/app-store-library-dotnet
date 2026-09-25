using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that updates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationupdaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2UpdateRequestData instead.")]
public sealed class SubscriptionLocalizationUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionLocalizations";

    /// <summary>
    /// The opaque resource ID of the localization to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value. The locale
    /// isn't among them, because you can't move a localization to another locale.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionLocalizationUpdateRequestDataAttributes? Attributes { get; set; }
}
