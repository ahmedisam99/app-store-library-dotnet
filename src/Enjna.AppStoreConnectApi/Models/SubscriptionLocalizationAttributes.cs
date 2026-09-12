using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalization/attributes"/>
public sealed class SubscriptionLocalizationAttributes
{
    /// <summary>
    /// The display name of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The locale this metadata is written for, such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The description of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The review state of this localized metadata.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionLocalizationState? State { get; set; }
}
