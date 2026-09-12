using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that sends a beta tester a TestFlight invitation.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatesterinvitationcreaterequest/data"/>
public sealed class BetaTesterInvitationCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaTesterInvitations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaTesterInvitations";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required BetaTesterInvitationCreateRequestDataRelationships Relationships { get; set; }
}
