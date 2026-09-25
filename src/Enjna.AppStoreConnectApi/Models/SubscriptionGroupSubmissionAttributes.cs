using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a Subscription Group Submissions resource. Apple describes none for it today:
/// a submission carries only its resource ID, and the state of what it submitted lives on the
/// subscriptions and localizations of the group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmission"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Submit a subscription group version through a review submission instead.")]
public sealed class SubscriptionGroupSubmissionAttributes
{
}
