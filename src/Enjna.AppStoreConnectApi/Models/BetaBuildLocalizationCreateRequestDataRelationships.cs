using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a request that creates a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalizationcreaterequest/data/relationships"/>
public sealed class BetaBuildLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The related Builds resource the localized text belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("build")]
    public required RelationshipDeclaration Build { get; set; }
}
