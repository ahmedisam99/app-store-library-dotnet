using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an In-App Purchase Images resource: the promotional image
/// the App Store shows for an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimage"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use InAppPurchaseImageV2 instead.")]
public sealed class InAppPurchaseImage : Resource<InAppPurchaseImageAttributes>
{
}
