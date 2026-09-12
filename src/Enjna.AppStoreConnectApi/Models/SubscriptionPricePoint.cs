namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Subscription Price Points resource: one price the App Store
/// offers for a subscription in one territory, together with what you earn from it. You set a
/// subscription's price by pointing at a price point rather than by naming an amount.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricepoint"/>
public sealed class SubscriptionPricePoint : Resource<SubscriptionPricePointAttributes>
{
}
