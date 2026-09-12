using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The App Store environment that a batch of one-time use offer codes belongs to. Sandbox codes are
/// redeemable only by sandbox testers, and never count against your production code allowance.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/offercodeenvironment"/>
[JsonConverter(typeof(JsonEnumMemberConverter<OfferCodeEnvironment>))]
public enum OfferCodeEnvironment
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The codes are redeemable by customers on the App Store.
    /// </summary>
    [EnumMember(Value = "PRODUCTION")]
    Production,

    /// <summary>
    /// The codes are redeemable only by sandbox testers.
    /// </summary>
    [EnumMember(Value = "SANDBOX")]
    Sandbox
}
