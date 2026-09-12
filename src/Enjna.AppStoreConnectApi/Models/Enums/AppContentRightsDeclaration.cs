using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Your declaration of whether an app displays or contains content that a third party owns.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/app/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<AppContentRightsDeclaration>))]
public enum AppContentRightsDeclaration
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The app contains no third-party content.
    /// </summary>
    [EnumMember(Value = "DOES_NOT_USE_THIRD_PARTY_CONTENT")]
    DoesNotUseThirdPartyContent,

    /// <summary>
    /// The app contains third-party content, and you hold the rights to use it.
    /// </summary>
    [EnumMember(Value = "USES_THIRD_PARTY_CONTENT")]
    UsesThirdPartyContent
}
