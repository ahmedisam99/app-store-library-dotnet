using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to change the localized metadata of an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationupdaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2UpdateRequest instead.")]
public sealed class SubscriptionLocalizationUpdateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionLocalizationUpdateRequestData Data { get; set; }
}
