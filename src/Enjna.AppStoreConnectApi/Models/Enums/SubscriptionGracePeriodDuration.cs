using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The length of a subscription billing grace period.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiodduration"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionGracePeriodDuration>))]
public enum SubscriptionGracePeriodDuration
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A grace period of three days.
    /// </summary>
    [EnumMember(Value = "THREE_DAYS")]
    ThreeDays,

    /// <summary>
    /// A grace period of sixteen days.
    /// </summary>
    [EnumMember(Value = "SIXTEEN_DAYS")]
    SixteenDays,

    /// <summary>
    /// A grace period of twenty-eight days.
    /// </summary>
    [EnumMember(Value = "TWENTY_EIGHT_DAYS")]
    TwentyEightDays
}
