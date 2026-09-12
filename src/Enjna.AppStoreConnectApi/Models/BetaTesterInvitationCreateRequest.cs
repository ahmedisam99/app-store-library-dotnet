using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to send a beta tester a TestFlight invitation.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatesterinvitationcreaterequest"/>
public sealed class BetaTesterInvitationCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BetaTesterInvitationCreateRequestData Data { get; set; }
}
