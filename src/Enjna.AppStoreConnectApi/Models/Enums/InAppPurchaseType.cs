using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The type of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasetype"/>
[JsonConverter(typeof(JsonEnumMemberConverter<InAppPurchaseType>))]
public enum InAppPurchaseType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A consumable in-app purchase, which a customer can buy more than once.
    /// </summary>
    [EnumMember(Value = "CONSUMABLE")]
    Consumable,

    /// <summary>
    /// A non-consumable in-app purchase, which a customer buys once.
    /// </summary>
    [EnumMember(Value = "NON_CONSUMABLE")]
    NonConsumable,

    /// <summary>
    /// A non-renewing subscription, which a customer renews manually.
    /// </summary>
    [EnumMember(Value = "NON_RENEWING_SUBSCRIPTION")]
    NonRenewingSubscription
}
