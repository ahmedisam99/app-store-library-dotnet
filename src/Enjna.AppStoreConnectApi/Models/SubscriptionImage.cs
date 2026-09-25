using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimage"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use SubscriptionImageV2 instead.")]
public sealed class SubscriptionImage : Resource<SubscriptionImageAttributes>
{
}
