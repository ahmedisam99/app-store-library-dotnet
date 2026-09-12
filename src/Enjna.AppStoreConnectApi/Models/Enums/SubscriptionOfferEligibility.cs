using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// How an offer interacts with the introductory offers of a subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffereligibility"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionOfferEligibility>))]
public enum SubscriptionOfferEligibility
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The offer applies in addition to an introductory offer.
    /// </summary>
    [EnumMember(Value = "STACK_WITH_INTRO_OFFERS")]
    StackWithIntroOffers,

    /// <summary>
    /// The offer replaces an introductory offer.
    /// </summary>
    [EnumMember(Value = "REPLACE_INTRO_OFFERS")]
    ReplaceIntroOffers
}
