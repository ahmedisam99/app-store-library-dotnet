using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Localizations resource of the v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationv2/attributes"/>
public sealed class SubscriptionLocalizationV2Attributes
{
    /// <summary>
    /// The display name of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The locale this localization is written for, such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The description of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
