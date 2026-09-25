namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Localizations resource of the v2 API: the
/// display name and description customers see for an auto-renewable subscription in one language,
/// held by a subscription version.
/// </summary>
/// <remarks>
/// Apple reuses the <c>subscriptionLocalizations</c> resource type of the deprecated v1 resource,
/// but this one has no state and relates to its version rather than to the subscription.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationv2"/>
public sealed class SubscriptionLocalizationV2 : Resource<SubscriptionLocalizationV2Attributes>
{
}
