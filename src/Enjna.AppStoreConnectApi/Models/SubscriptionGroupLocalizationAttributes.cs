using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Group Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalization/attributes"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2Attributes instead.")]
public sealed class SubscriptionGroupLocalizationAttributes
{
    /// <summary>
    /// The display name of the subscription group in this language, which customers see when they
    /// manage their subscription.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The app name to show in this language instead of the name on the App Store, for apps whose
    /// store name differs from the name customers know the subscription by.
    /// </summary>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get; set; }

    /// <summary>
    /// The language the localization is written in, as a locale code such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The state of the localization in the App Store review process.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionGroupLocalizationState? State { get; set; }
}
