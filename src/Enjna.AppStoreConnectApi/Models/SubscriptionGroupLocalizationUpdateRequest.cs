using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationupdaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2UpdateRequest instead.")]
public sealed class SubscriptionGroupLocalizationUpdateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupLocalizationUpdateRequestData Data { get; set; }
}
