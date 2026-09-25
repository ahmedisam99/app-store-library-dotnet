using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a v2 In-App Purchase Localizations resource. Unlike the v1
/// localization, it carries no review state of its own; the version it belongs to does.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2/attributes"/>
public sealed class InAppPurchaseLocalizationV2Attributes
{
    /// <summary>
    /// The display name of the in-app purchase in this language, which customers see on the App
    /// Store and in your app.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The locale this localization is written for, such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The description of the in-app purchase in this language.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
