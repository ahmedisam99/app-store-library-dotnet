using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data of a request that updates a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalizationupdaterequest/data"/>
public sealed class BetaBuildLocalizationUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaBuildLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaBuildLocalizations";

    /// <summary>
    /// The opaque resource ID of the localization to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Beta Build Localizations resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public BetaBuildLocalizationUpdateRequestDataAttributes? Attributes { get; set; }
}
