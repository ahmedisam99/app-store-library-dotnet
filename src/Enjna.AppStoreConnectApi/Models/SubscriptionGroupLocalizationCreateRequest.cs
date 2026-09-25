using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationcreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2CreateRequest instead.")]
public sealed class SubscriptionGroupLocalizationCreateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupLocalizationCreateRequestData Data { get; set; }
}
