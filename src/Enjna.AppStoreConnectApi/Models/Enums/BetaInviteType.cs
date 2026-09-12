using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The type of beta testing invitation.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betainvitetype"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BetaInviteType>))]
public enum BetaInviteType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The tester receives an email invitation.
    /// </summary>
    [EnumMember(Value = "EMAIL")]
    Email,

    /// <summary>
    /// The tester joins through a public link.
    /// </summary>
    [EnumMember(Value = "PUBLIC_LINK")]
    PublicLink
}
