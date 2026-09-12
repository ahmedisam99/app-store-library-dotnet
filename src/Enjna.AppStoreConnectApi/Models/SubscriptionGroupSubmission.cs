namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Group Submissions resource. Creating one
/// submits every subscription and localization in a subscription group that is ready to submit for
/// App Store review, without submitting a new app version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmission"/>
public sealed class SubscriptionGroupSubmission : Resource<SubscriptionGroupSubmissionAttributes>
{
}
