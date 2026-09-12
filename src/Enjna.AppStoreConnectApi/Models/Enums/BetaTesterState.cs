using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of a beta tester's invitation to a beta group or a build.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatesterstate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BetaTesterState>))]
public enum BetaTesterState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The tester isn't invited yet.
    /// </summary>
    [EnumMember(Value = "NOT_INVITED")]
    NotInvited,

    /// <summary>
    /// The tester received an invitation and hasn't accepted it yet.
    /// </summary>
    [EnumMember(Value = "INVITED")]
    Invited,

    /// <summary>
    /// The tester accepted the invitation.
    /// </summary>
    [EnumMember(Value = "ACCEPTED")]
    Accepted,

    /// <summary>
    /// The tester installed the build.
    /// </summary>
    [EnumMember(Value = "INSTALLED")]
    Installed,

    /// <summary>
    /// The tester's access is revoked.
    /// </summary>
    [EnumMember(Value = "REVOKED")]
    Revoked
}
