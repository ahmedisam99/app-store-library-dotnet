using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that changes a billing grace period.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiodupdaterequest/data/attributes"/>
public sealed class SubscriptionGracePeriodUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Whether to turn the billing grace period on in production.
    /// </summary>
    [JsonPropertyName("optIn")]
    public bool? OptIn { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// Whether to turn the billing grace period on in the sandbox environment.
    /// </summary>
    [JsonPropertyName("sandboxOptIn")]
    public bool? SandboxOptIn { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// How long service continues while the App Store retries the failed renewal.
    /// </summary>
    [JsonPropertyName("duration")]
    public SubscriptionGracePeriodDuration? Duration { get => Get<SubscriptionGracePeriodDuration?>(); set => Set(value); }

    /// <summary>
    /// The renewals the billing grace period applies to.
    /// </summary>
    [JsonPropertyName("renewalType")]
    public SubscriptionGracePeriodRenewalType? RenewalType { get => Get<SubscriptionGracePeriodRenewalType?>(); set => Set(value); }
}
