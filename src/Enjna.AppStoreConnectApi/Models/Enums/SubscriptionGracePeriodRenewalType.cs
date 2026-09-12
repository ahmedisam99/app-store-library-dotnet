using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The renewals a subscription billing grace period applies to.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiod/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionGracePeriodRenewalType>))]
public enum SubscriptionGracePeriodRenewalType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The grace period applies to every renewal that fails to bill, including a renewal that
    /// follows a free trial or an introductory offer.
    /// </summary>
    [EnumMember(Value = "ALL_RENEWALS")]
    AllRenewals,

    /// <summary>
    /// The grace period applies only to a renewal of a subscription the customer already pays for.
    /// </summary>
    [EnumMember(Value = "PAID_TO_PAID_ONLY")]
    PaidToPaidOnly
}
