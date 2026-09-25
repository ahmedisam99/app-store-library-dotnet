using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to add localized metadata to an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationcreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2CreateRequest instead.")]
public sealed class SubscriptionLocalizationCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionLocalizationCreateRequestData Data { get; set; }
}
