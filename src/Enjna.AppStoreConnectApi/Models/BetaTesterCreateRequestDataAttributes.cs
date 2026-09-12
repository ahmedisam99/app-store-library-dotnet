using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you set when you create a beta tester.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatestercreaterequest/data/attributes"/>
public sealed class BetaTesterCreateRequestDataAttributes
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
    /// The email address TestFlight sends the beta tester's invitation to. This property is required.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
