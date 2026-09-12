using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data of a request that creates a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalizationcreaterequest/data"/>
public sealed class BetaBuildLocalizationCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaBuildLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaBuildLocalizations";

    /// <summary>
    /// The attributes that describe the request that creates a Beta Build Localizations resource.
    /// These attributes are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required BetaBuildLocalizationCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. These relationships
    /// are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required BetaBuildLocalizationCreateRequestDataRelationships Relationships { get; set; }
}
