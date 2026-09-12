using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The platform of a device that a beta tester uses to test an app.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatester/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BetaTesterAppDevicePlatform>))]
public enum BetaTesterAppDevicePlatform
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The iOS platform, which includes iPadOS.
    /// </summary>
    [EnumMember(Value = "IOS")]
    Ios,

    /// <summary>
    /// The macOS platform.
    /// </summary>
    [EnumMember(Value = "MAC_OS")]
    MacOs,

    /// <summary>
    /// The tvOS platform.
    /// </summary>
    [EnumMember(Value = "TV_OS")]
    TvOs,

    /// <summary>
    /// The watchOS platform.
    /// </summary>
    [EnumMember(Value = "WATCH_OS")]
    WatchOs,

    /// <summary>
    /// The visionOS platform.
    /// </summary>
    [EnumMember(Value = "VISION_OS")]
    VisionOs
}
