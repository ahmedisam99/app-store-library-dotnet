namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Grace Periods resource, which holds an app's
/// billing grace period settings. A billing grace period keeps a subscriber's service running while
/// the App Store retries a renewal that failed to bill.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiod"/>
public sealed class SubscriptionGracePeriod : Resource<SubscriptionGracePeriodAttributes>
{
}
