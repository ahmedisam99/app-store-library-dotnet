using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The eligibility of a customer for a subscription offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptioncustomereligibility"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionCustomerEligibility>))]
public enum SubscriptionCustomerEligibility
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A customer who has never subscribed to the subscription.
    /// </summary>
    [EnumMember(Value = "NEW")]
    New,

    /// <summary>
    /// A customer with an active subscription.
    /// </summary>
    [EnumMember(Value = "EXISTING")]
    Existing,

    /// <summary>
    /// A customer whose subscription lapsed.
    /// </summary>
    [EnumMember(Value = "EXPIRED")]
    Expired
}
