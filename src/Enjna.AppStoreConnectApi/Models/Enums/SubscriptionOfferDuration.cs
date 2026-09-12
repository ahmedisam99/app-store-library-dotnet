using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The length of a subscription offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionofferduration"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionOfferDuration>))]
public enum SubscriptionOfferDuration
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// An offer that lasts three days.
    /// </summary>
    [EnumMember(Value = "THREE_DAYS")]
    ThreeDays,

    /// <summary>
    /// An offer that lasts one week.
    /// </summary>
    [EnumMember(Value = "ONE_WEEK")]
    OneWeek,

    /// <summary>
    /// An offer that lasts two weeks.
    /// </summary>
    [EnumMember(Value = "TWO_WEEKS")]
    TwoWeeks,

    /// <summary>
    /// An offer that lasts one month.
    /// </summary>
    [EnumMember(Value = "ONE_MONTH")]
    OneMonth,

    /// <summary>
    /// An offer that lasts two months.
    /// </summary>
    [EnumMember(Value = "TWO_MONTHS")]
    TwoMonths,

    /// <summary>
    /// An offer that lasts three months.
    /// </summary>
    [EnumMember(Value = "THREE_MONTHS")]
    ThreeMonths,

    /// <summary>
    /// An offer that lasts six months.
    /// </summary>
    [EnumMember(Value = "SIX_MONTHS")]
    SixMonths,

    /// <summary>
    /// An offer that lasts one year.
    /// </summary>
    [EnumMember(Value = "ONE_YEAR")]
    OneYear
}
