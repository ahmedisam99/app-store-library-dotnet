using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The stage an uploaded media asset reached.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appmediaassetstate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<AppMediaAssetStateState>))]
public enum AppMediaAssetStateState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// App Store Connect reserved the asset and is waiting for you to upload the file.
    /// </summary>
    [EnumMember(Value = "AWAITING_UPLOAD")]
    AwaitingUpload,

    /// <summary>
    /// You uploaded every part of the file, and App Store Connect is processing it.
    /// </summary>
    [EnumMember(Value = "UPLOAD_COMPLETE")]
    UploadComplete,

    /// <summary>
    /// App Store Connect finished processing the asset and accepted it.
    /// </summary>
    [EnumMember(Value = "COMPLETE")]
    Complete,

    /// <summary>
    /// App Store Connect couldn't process the asset. Read <c>errors</c> to find out why.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    Failed
}
