using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set when you create a beta tester.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatestercreaterequest/data/relationships"/>
public sealed class BetaTesterCreateRequestDataRelationships
{
    /// <summary>
    /// The related Beta Groups resources the beta tester joins.
    /// </summary>
    [JsonPropertyName("betaGroups")]
    public RelationshipDeclarationList? BetaGroups { get; set; }

    /// <summary>
    /// The related Builds resources the beta tester can test individually.
    /// </summary>
    [JsonPropertyName("builds")]
    public RelationshipDeclarationList? Builds { get; set; }
}
