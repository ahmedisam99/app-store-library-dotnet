namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Groups resource. A subscription group holds
/// the subscriptions a customer chooses between, so that only one subscription in the group is
/// active at a time and the customer can move up or down between its levels.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroup"/>
public sealed class SubscriptionGroup : Resource<SubscriptionGroupAttributes>
{
}
