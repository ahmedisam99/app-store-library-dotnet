using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that sends a beta tester a TestFlight invitation.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatesterinvitationcreaterequest/data/relationships"/>
public sealed class BetaTesterInvitationCreateRequestDataRelationships
{
    /// <summary>
    /// The app to invite the beta tester to test. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }

    /// <summary>
    /// The beta tester to invite. Apple deprecated this relationship.
    /// </summary>
    [JsonPropertyName("betaTester")]
    public RelationshipDeclaration? BetaTester { get; set; }
}
