using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Availabilities resource, which holds the
/// territories a subscription is for sale in. Apple deprecated this resource in favor of the
/// Subscription Plan Availabilities resource, which models availability per subscription plan.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailability"/>
[Obsolete("Apple deprecated this resource. Use SubscriptionPlanAvailability instead.")]
public sealed class SubscriptionAvailability : Resource<SubscriptionAvailabilityAttributes>
{
}
