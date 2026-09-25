using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of an In-App Purchase Submissions resource. Apple describes the submission
/// entirely through the in-app purchase it points at, so it carries no attributes of its own today.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasesubmission"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Submit an in-app purchase version through a review submission instead.")]
public sealed class InAppPurchaseSubmissionAttributes
{
}
