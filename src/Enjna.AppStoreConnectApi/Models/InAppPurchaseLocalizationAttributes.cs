using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalization/attributes"/>
public sealed class InAppPurchaseLocalizationAttributes
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

    /// <summary>
    /// Where the localization sits in the App Review workflow.
    /// </summary>
    [JsonPropertyName("state")]
    public InAppPurchaseLocalizationState? State { get; set; }
}
