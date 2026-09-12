using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Grace Periods resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiod/attributes"/>
public sealed class SubscriptionGracePeriodAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether the billing grace period is turned on in production.
    /// </summary>
    [JsonPropertyName("optIn")]
    public bool? OptIn { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the billing grace period is turned on in the sandbox
    /// environment.
    /// </summary>
    [JsonPropertyName("sandboxOptIn")]
    public bool? SandboxOptIn { get; set; }

    /// <summary>
    /// How long service continues while the App Store retries the failed renewal.
    /// </summary>
    [JsonPropertyName("duration")]
    public SubscriptionGracePeriodDuration? Duration { get; set; }

    /// <summary>
    /// The renewals the billing grace period applies to.
    /// </summary>
    [JsonPropertyName("renewalType")]
    public SubscriptionGracePeriodRenewalType? RenewalType { get; set; }
}
