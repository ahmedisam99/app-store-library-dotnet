using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalization/attributes"/>
public sealed class BetaBuildLocalizationAttributes
{
    /// <summary>
    /// A description of the changes and the features testers should try out in this build.
    /// </summary>
    [JsonPropertyName("whatsNew")]
    public string? WhatsNew { get; set; }

    /// <summary>
    /// The locale the text is written in, such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
}
