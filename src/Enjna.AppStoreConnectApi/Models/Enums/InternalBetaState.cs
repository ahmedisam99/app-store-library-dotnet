using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of a build with respect to internal TestFlight testing, which needs no beta review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/internalbetastate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<InternalBetaState>))]
public enum InternalBetaState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// App Store Connect is still processing the uploaded build.
    /// </summary>
    [EnumMember(Value = "PROCESSING")]
    Processing,

    /// <summary>
    /// Processing the build failed, so it can't be tested.
    /// </summary>
    [EnumMember(Value = "PROCESSING_EXCEPTION")]
    ProcessingException,

    /// <summary>
    /// The build is waiting on the export compliance information it needs before testing can start.
    /// </summary>
    [EnumMember(Value = "MISSING_EXPORT_COMPLIANCE")]
    MissingExportCompliance,

    /// <summary>
    /// The build is ready to hand to internal testers.
    /// </summary>
    [EnumMember(Value = "READY_FOR_BETA_TESTING")]
    ReadyForBetaTesting,

    /// <summary>
    /// Internal testers can install and test the build.
    /// </summary>
    [EnumMember(Value = "IN_BETA_TESTING")]
    InBetaTesting,

    /// <summary>
    /// The build expired, so testers can no longer install it.
    /// </summary>
    [EnumMember(Value = "EXPIRED")]
    Expired,

    /// <summary>
    /// Apple is reviewing the export compliance information you provided for the build.
    /// </summary>
    [EnumMember(Value = "IN_EXPORT_COMPLIANCE_REVIEW")]
    InExportComplianceReview
}
