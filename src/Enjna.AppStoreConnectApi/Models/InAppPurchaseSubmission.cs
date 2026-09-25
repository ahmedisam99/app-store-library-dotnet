using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an In-App Purchase Submissions resource: the record of an
/// in-app purchase you sent to App Review on its own, without an app version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasesubmission"/>
[Obsolete("Apple deprecated this resource in App Store Connect API 4.4.1. Submit an in-app purchase version through a review submission instead.")]
public sealed class InAppPurchaseSubmission : Resource<InAppPurchaseSubmissionAttributes>
{
}
