using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The payment mode of a subscription offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffermode"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionOfferMode>))]
public enum SubscriptionOfferMode
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The customer pays a discounted price each billing period of the offer.
    /// </summary>
    [EnumMember(Value = "PAY_AS_YOU_GO")]
    PayAsYouGo,

    /// <summary>
    /// The customer pays a discounted price once, up front, for the whole offer period.
    /// </summary>
    [EnumMember(Value = "PAY_UP_FRONT")]
    PayUpFront,

    /// <summary>
    /// The customer pays nothing during the offer period.
    /// </summary>
    [EnumMember(Value = "FREE_TRIAL")]
    FreeTrial
}
