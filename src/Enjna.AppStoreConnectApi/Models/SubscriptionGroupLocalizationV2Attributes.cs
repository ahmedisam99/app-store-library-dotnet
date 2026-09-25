using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Group Localizations resource of the version-based
/// workflow. Unlike the deprecated localization, it has no state of its own; read the state of the
/// version it belongs to instead.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2/attributes"/>
public sealed class SubscriptionGroupLocalizationV2Attributes
{
    /// <summary>
    /// The display name of the subscription group in this language, which shows up in your app when
    /// someone reviews or purchases one of the group's subscriptions.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The optional custom app name that customers see for the subscription group in this language.
    /// </summary>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get; set; }

    /// <summary>
    /// The language the localization is written in, as a locale code such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
}
