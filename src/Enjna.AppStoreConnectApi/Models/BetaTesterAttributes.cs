using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Beta Testers resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatester/attributes"/>
public sealed class BetaTesterAttributes
{
    /// <summary>
    /// The beta tester's first name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    /// The beta tester's last name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    /// <summary>
    /// The email address TestFlight sends the beta tester's invitation to.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The way the beta tester was invited to test, either by email or through a public link.
    /// </summary>
    [JsonPropertyName("inviteType")]
    public BetaInviteType? InviteType { get; set; }

    /// <summary>
    /// The state of the beta tester's invitation.
    /// </summary>
    [JsonPropertyName("state")]
    public BetaTesterState? State { get; set; }

    /// <summary>
    /// The devices the beta tester uses to test the app.
    /// </summary>
    [JsonPropertyName("appDevices")]
    public BetaTesterAppDevice[]? AppDevices { get; set; }
}
