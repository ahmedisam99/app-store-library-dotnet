using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Group Localizations resource. A localization
/// carries the subscription group's display name, and optionally the app name, in one language.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalization"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2 instead.")]
public sealed class SubscriptionGroupLocalization : Resource<SubscriptionGroupLocalizationAttributes>
{
}
