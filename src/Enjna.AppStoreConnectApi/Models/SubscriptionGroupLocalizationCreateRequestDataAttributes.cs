using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you set on a request that creates a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationcreaterequest/data/attributes"/>
public sealed class SubscriptionGroupLocalizationCreateRequestDataAttributes
{
    /// <summary>
    /// The display name of the subscription group in this language. It is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The app name to show in this language instead of the name on the App Store.
    /// </summary>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get; set; }

    /// <summary>
    /// The language the localization is written in, as a locale code such as <c>en-US</c>. It is
    /// required, and you can't change it after you create the localization.
    /// </summary>
    [JsonPropertyName("locale")]
    public required string Locale { get; set; }
}
