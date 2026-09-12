using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you set when you create a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupcreaterequest/data/attributes"/>
public sealed class BetaGroupCreateRequestDataAttributes
{
    /// <summary>
    /// The name of the beta group. This property is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the group is internal. Only members of your team can
    /// join an internal group.
    /// </summary>
    [JsonPropertyName("isInternalGroup")]
    public bool? IsInternalGroup { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the group automatically receives every build of the app.
    /// </summary>
    [JsonPropertyName("hasAccessToAllBuilds")]
    public bool? HasAccessToAllBuilds { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the public link is enabled, letting anyone with the
    /// link join the group.
    /// </summary>
    [JsonPropertyName("publicLinkEnabled")]
    public bool? PublicLinkEnabled { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether a limit applies to the number of testers who can join
    /// the group through the public link.
    /// </summary>
    [JsonPropertyName("publicLinkLimitEnabled")]
    public bool? PublicLinkLimitEnabled { get; set; }

    /// <summary>
    /// The maximum number of testers that can join the group through the public link.
    /// </summary>
    [JsonPropertyName("publicLinkLimit")]
    public int? PublicLinkLimit { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether testers in the group can send feedback from TestFlight.
    /// </summary>
    [JsonPropertyName("feedbackEnabled")]
    public bool? FeedbackEnabled { get; set; }
}
