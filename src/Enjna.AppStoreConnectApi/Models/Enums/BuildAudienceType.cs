using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The audience that can access a build.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildaudiencetype"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BuildAudienceType>))]
public enum BuildAudienceType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// Only internal testers can access the build.
    /// </summary>
    [EnumMember(Value = "INTERNAL_ONLY")]
    InternalOnly,

    /// <summary>
    /// The build is eligible for external testing and App Store submission.
    /// </summary>
    [EnumMember(Value = "APP_STORE_ELIGIBLE")]
    AppStoreEligible
}
