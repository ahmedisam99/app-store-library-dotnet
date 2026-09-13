using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreServerLibrary.Models.Enums;

/// <summary>
/// The type of an external purchase custom link token.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/tokentype"/>
[JsonConverter(typeof(JsonEnumMemberConverter<TokenType>))]
public enum TokenType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A token type that indicates an initial acquisition.
    /// </summary>
    [EnumMember(Value = "ACQUISITION")]
    Acquisition,

    /// <summary>
    /// A token type that indicates usage of App Store services.
    /// </summary>
    [EnumMember(Value = "SERVICES")]
    Services
}
