using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an In-App Purchase Localizations resource: the display name
/// and description customers see for an in-app purchase in one language.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalization"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Use InAppPurchaseLocalizationV2 instead.")]
public sealed class InAppPurchaseLocalization : Resource<InAppPurchaseLocalizationAttributes>
{
}
