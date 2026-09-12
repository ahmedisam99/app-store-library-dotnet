using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set when you create a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupcreaterequest/data/relationships"/>
public sealed class BetaGroupCreateRequestDataRelationships
{
    /// <summary>
    /// The related Apps resource, which is the app the beta group tests. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }

    /// <summary>
    /// The related Builds resources the beta group has access to.
    /// </summary>
    [JsonPropertyName("builds")]
    public RelationshipDeclarationList? Builds { get; set; }

    /// <summary>
    /// The related Beta Testers resources that join the beta group.
    /// </summary>
    [JsonPropertyName("betaTesters")]
    public RelationshipDeclarationList? BetaTesters { get; set; }
}
