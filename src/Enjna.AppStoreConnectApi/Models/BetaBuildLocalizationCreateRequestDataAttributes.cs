using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a request that creates a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalizationcreaterequest/data/attributes"/>
public sealed class BetaBuildLocalizationCreateRequestDataAttributes
{
    /// <summary>
    /// A description of the changes and the features testers should try out in this build.
    /// </summary>
    [JsonPropertyName("whatsNew")]
    public string? WhatsNew { get; set; }

    /// <summary>
    /// The locale the text is written in, such as <c>en-US</c>. This value is required.
    /// </summary>
    [JsonPropertyName("locale")]
    public required string Locale { get; set; }
}
