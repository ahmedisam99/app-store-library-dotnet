using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalization"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2 instead.")]
public sealed class SubscriptionLocalization : Resource<SubscriptionLocalizationAttributes>
{
}
