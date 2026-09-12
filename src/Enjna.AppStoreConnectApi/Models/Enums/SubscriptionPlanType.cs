using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The billing plan of a subscription offer price.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplantype"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionPlanType>))]
public enum SubscriptionPlanType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The customer is billed monthly.
    /// </summary>
    [EnumMember(Value = "MONTHLY")]
    Monthly,

    /// <summary>
    /// The customer is billed once, up front.
    /// </summary>
    [EnumMember(Value = "UPFRONT")]
    Upfront
}
