using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The past spending behavior that makes a customer eligible for an in-app purchase offer code.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercode"/>
[JsonConverter(typeof(JsonEnumMemberConverter<InAppPurchaseOfferCodeCustomerEligibility>))]
public enum InAppPurchaseOfferCodeCustomerEligibility
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A customer who has never bought an in-app purchase in your app.
    /// </summary>
    [EnumMember(Value = "NON_SPENDER")]
    NonSpender,

    /// <summary>
    /// A customer who has bought an in-app purchase in your app recently.
    /// </summary>
    [EnumMember(Value = "ACTIVE_SPENDER")]
    ActiveSpender,

    /// <summary>
    /// A customer who used to buy in-app purchases in your app, but stopped.
    /// </summary>
    [EnumMember(Value = "CHURNED_SPENDER")]
    ChurnedSpender
}
