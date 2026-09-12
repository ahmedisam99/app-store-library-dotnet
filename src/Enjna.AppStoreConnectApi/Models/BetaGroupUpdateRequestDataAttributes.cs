using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you set when you update a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupupdaterequest/data/attributes"/>
public sealed class BetaGroupUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The name of the beta group.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether the public link is enabled, letting anyone with the
    /// link join the group.
    /// </summary>
    [JsonPropertyName("publicLinkEnabled")]
    public bool? PublicLinkEnabled { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether a limit applies to the number of testers who can join
    /// the group through the public link.
    /// </summary>
    [JsonPropertyName("publicLinkLimitEnabled")]
    public bool? PublicLinkLimitEnabled { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// The maximum number of testers that can join the group through the public link.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("publicLinkLimit")]
    public int? PublicLinkLimit { get => Get<int?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether testers in the group can send feedback from TestFlight.
    /// </summary>
    [JsonPropertyName("feedbackEnabled")]
    public bool? FeedbackEnabled { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether the group's testers can install iOS builds of the app
    /// on a Mac with Apple silicon.
    /// </summary>
    [JsonPropertyName("iosBuildsAvailableForAppleSiliconMac")]
    public bool? IosBuildsAvailableForAppleSiliconMac { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether the group's testers can install iOS builds of the app
    /// on Apple Vision Pro.
    /// </summary>
    [JsonPropertyName("iosBuildsAvailableForAppleVision")]
    public bool? IosBuildsAvailableForAppleVision { get => Get<bool?>(); set => Set(value); }
}
