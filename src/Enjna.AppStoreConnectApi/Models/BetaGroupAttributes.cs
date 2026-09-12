using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Beta Groups resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroup/attributes"/>
public sealed class BetaGroupAttributes
{
    /// <summary>
    /// The name of the beta group.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The date and time the beta group was created.
    /// </summary>
    [JsonPropertyName("createdDate")]
    public DateTimeOffset? CreatedDate { get; set; }

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
    /// The ID portion of the public link's URL.
    /// </summary>
    [JsonPropertyName("publicLinkId")]
    public string? PublicLinkId { get; set; }

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
    /// The public link testers use to join the beta group.
    /// </summary>
    [JsonPropertyName("publicLink")]
    public string? PublicLink { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether testers in the group can send feedback from TestFlight.
    /// </summary>
    [JsonPropertyName("feedbackEnabled")]
    public bool? FeedbackEnabled { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the group's testers can install iOS builds of the app
    /// on a Mac with Apple silicon.
    /// </summary>
    [JsonPropertyName("iosBuildsAvailableForAppleSiliconMac")]
    public bool? IosBuildsAvailableForAppleSiliconMac { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the group's testers can install iOS builds of the app
    /// on Apple Vision Pro.
    /// </summary>
    [JsonPropertyName("iosBuildsAvailableForAppleVision")]
    public bool? IosBuildsAvailableForAppleVision { get; set; }
}
