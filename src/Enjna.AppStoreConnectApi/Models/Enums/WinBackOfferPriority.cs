using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The precedence of a win-back offer when a customer qualifies for more than one of them.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffer/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<WinBackOfferPriority>))]
public enum WinBackOfferPriority
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The offer takes precedence over every offer of normal priority.
    /// </summary>
    [EnumMember(Value = "HIGH")]
    High,

    /// <summary>
    /// The offer competes with the other offers of normal priority.
    /// </summary>
    [EnumMember(Value = "NORMAL")]
    Normal
}
