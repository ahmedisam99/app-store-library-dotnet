using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a subscription group localization on a
/// subscription group version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2createrequest/data/attributes"/>
public sealed class SubscriptionGroupLocalizationV2CreateRequestDataAttributes
{
    /// <summary>
    /// The display name of the subscription group in this language. This attribute is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The optional custom app name that customers see for the subscription group in this language.
    /// </summary>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get; set; }

    /// <summary>
    /// The language the localization is written in, as a locale code such as <c>en-US</c>. This
    /// attribute is required, and you can't change it after you create the localization.
    /// </summary>
    [JsonPropertyName("locale")]
    public required string Locale { get; set; }
}
