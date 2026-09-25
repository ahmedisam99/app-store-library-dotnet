using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Submissions resource. Creating one submits a
/// subscription and its metadata to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionsubmission"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Submit a subscription version through a review submission instead.")]
public sealed class SubscriptionSubmission : Resource<SubscriptionSubmissionAttributes>
{
}
