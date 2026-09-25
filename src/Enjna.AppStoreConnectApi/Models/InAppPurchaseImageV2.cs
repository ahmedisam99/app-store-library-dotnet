namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a v2 In-App Purchase Images resource: a promotional image of
/// an in-app purchase, held by an in-app purchase version rather than by the in-app purchase itself.
/// It isn't the App Review screenshot, which is an
/// <see cref="InAppPurchaseAppStoreReviewScreenshot"/> on the in-app purchase.
/// </summary>
/// <remarks>
/// Apple's documentation disagrees with itself here. The 4.4.1 release notes say these endpoints
/// manage review screenshots. The version guide calls them the promotion image customers see on the
/// App Store product page, and the migration guide keeps the App Review screenshot on the in-app
/// purchase. This library follows the guides.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2"/>
public sealed class InAppPurchaseImageV2 : Resource<InAppPurchaseImageV2Attributes>
{
}
