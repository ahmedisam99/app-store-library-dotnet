namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Images resource of the v2 API: a promotional
/// image of an auto-renewable subscription, held by a subscription version.
/// </summary>
/// <remarks>
/// Apple reuses the <c>subscriptionImages</c> resource type of the deprecated v1 resource, but this
/// one has no state and no source file checksum. Read the upload's progress from
/// <see cref="SubscriptionImageV2Attributes.AssetDeliveryState"/>, and the review's progress from
/// the state of the version.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2"/>
public sealed class SubscriptionImageV2 : Resource<SubscriptionImageV2Attributes>
{
}
