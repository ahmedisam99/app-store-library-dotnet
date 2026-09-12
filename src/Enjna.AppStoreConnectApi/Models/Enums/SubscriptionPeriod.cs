using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The length of a single billing period of an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscription/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionPeriod>))]
public enum SubscriptionPeriod
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The subscription renews every week.
    /// </summary>
    [EnumMember(Value = "ONE_WEEK")]
    OneWeek,

    /// <summary>
    /// The subscription renews every month.
    /// </summary>
    [EnumMember(Value = "ONE_MONTH")]
    OneMonth,

    /// <summary>
    /// The subscription renews every two months.
    /// </summary>
    [EnumMember(Value = "TWO_MONTHS")]
    TwoMonths,

    /// <summary>
    /// The subscription renews every three months.
    /// </summary>
    [EnumMember(Value = "THREE_MONTHS")]
    ThreeMonths,

    /// <summary>
    /// The subscription renews every six months.
    /// </summary>
    [EnumMember(Value = "SIX_MONTHS")]
    SixMonths,

    /// <summary>
    /// The subscription renews every year.
    /// </summary>
    [EnumMember(Value = "ONE_YEAR")]
    OneYear
}
