using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The kind of sales and trends report to download.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-salesreports"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SalesReportType>))]
public enum SalesReportType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// Units and proceeds for every app, in-app purchase, and app bundle you sell.
    /// </summary>
    [EnumMember(Value = "SALES")]
    Sales,

    /// <summary>
    /// Pre-orders customers placed, and the pre-orders they cancelled.
    /// </summary>
    [EnumMember(Value = "PRE_ORDER")]
    PreOrder,

    /// <summary>
    /// Newsstand issue downloads, for the publications that still use Newsstand.
    /// </summary>
    [EnumMember(Value = "NEWSSTAND")]
    Newsstand,

    /// <summary>
    /// A snapshot of your active, cancelled, and reactivated subscriptions for the report date.
    /// </summary>
    [EnumMember(Value = "SUBSCRIPTION")]
    Subscription,

    /// <summary>
    /// The subscription lifecycle events, such as renewals, cancellations, and plan changes.
    /// </summary>
    [EnumMember(Value = "SUBSCRIPTION_EVENT")]
    SubscriptionEvent,

    /// <summary>
    /// Anonymized, subscriber-level detail for each of your subscriptions.
    /// </summary>
    [EnumMember(Value = "SUBSCRIBER")]
    Subscriber,

    /// <summary>
    /// The offer codes customers redeemed for your subscriptions.
    /// </summary>
    [EnumMember(Value = "SUBSCRIPTION_OFFER_CODE_REDEMPTION")]
    SubscriptionOfferCodeRedemption,

    /// <summary>
    /// The installs of your apps, broken out by the install type and the source.
    /// </summary>
    [EnumMember(Value = "INSTALLS")]
    Installs,

    /// <summary>
    /// The subscriptions that reached their first year, and so moved to the lower commission rate.
    /// </summary>
    [EnumMember(Value = "FIRST_ANNUAL")]
    FirstAnnual,

    /// <summary>
    /// The subscribers who are eligible for a win-back offer.
    /// </summary>
    [EnumMember(Value = "WIN_BACK_ELIGIBILITY")]
    WinBackEligibility
}
