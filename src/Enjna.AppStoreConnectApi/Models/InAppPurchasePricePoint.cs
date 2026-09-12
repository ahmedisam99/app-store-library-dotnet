namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an In-App Purchase Price Points resource: one price the App Store
/// offers for an in-app purchase in one territory, together with what you earn from it. You set an
/// in-app purchase's price by pointing at a price point rather than by naming an amount.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepricepoint"/>
public sealed class InAppPurchasePricePoint : Resource<InAppPurchasePricePointAttributes>
{
}
