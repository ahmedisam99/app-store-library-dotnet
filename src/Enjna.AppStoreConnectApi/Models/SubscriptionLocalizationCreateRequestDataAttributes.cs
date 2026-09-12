using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that creates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationcreaterequest/data/attributes"/>
public sealed class SubscriptionLocalizationCreateRequestDataAttributes
{
    /// <summary>
    /// The display name of the subscription that customers see on the App Store in this locale.
    /// This value is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The locale this metadata is written for, such as <c>en-US</c>. This value is required.
    /// </summary>
    [JsonPropertyName("locale")]
    public required string Locale { get; set; }

    /// <summary>
    /// The description of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
