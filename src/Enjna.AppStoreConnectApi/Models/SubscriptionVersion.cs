namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Versions resource: the draft container that
/// groups the localized metadata and promotional images of an auto-renewable subscription that go
/// through App Review together.
/// </summary>
/// <remarks>
/// The subscription holds what stays the same across versions, such as its product ID, duration,
/// group, and pricing. Each version holds the reviewable metadata of one review cycle.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionversion"/>
public sealed class SubscriptionVersion : Resource<SubscriptionVersionAttributes>
{
}
