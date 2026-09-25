using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a v2 In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2createrequest/data/attributes"/>
public sealed class InAppPurchaseLocalizationV2CreateRequestDataAttributes
{
    /// <summary>
    /// The display name customers see for the in-app purchase in this language. This attribute is
    /// required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The locale to write the localization for, such as <c>en-US</c>. This attribute is required.
    /// </summary>
    [JsonPropertyName("locale")]
    public required string Locale { get; set; }

    /// <summary>
    /// The description customers see for the in-app purchase in this language.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
