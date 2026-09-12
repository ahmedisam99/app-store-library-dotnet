using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of App Store Connect's processing of an uploaded build.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/build/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BuildProcessingState>))]
public enum BuildProcessingState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// App Store Connect is still processing the build.
    /// </summary>
    [EnumMember(Value = "PROCESSING")]
    Processing,

    /// <summary>
    /// Processing the build failed.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    Failed,

    /// <summary>
    /// The build is invalid and can't be used.
    /// </summary>
    [EnumMember(Value = "INVALID")]
    Invalid,

    /// <summary>
    /// Processing finished, and the build is valid.
    /// </summary>
    [EnumMember(Value = "VALID")]
    Valid
}
