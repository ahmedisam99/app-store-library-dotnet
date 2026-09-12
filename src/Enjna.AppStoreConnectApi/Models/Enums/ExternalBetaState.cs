using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of a build with respect to external TestFlight testing, which Apple reviews before
/// the build reaches testers outside your team.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/externalbetastate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<ExternalBetaState>))]
public enum ExternalBetaState
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
    /// The build passed beta review and is ready to hand to external testers.
    /// </summary>
    [EnumMember(Value = "READY_FOR_BETA_TESTING")]
    ReadyForBetaTesting,

    /// <summary>
    /// External testers can install and test the build.
    /// </summary>
    [EnumMember(Value = "IN_BETA_TESTING")]
    InBetaTesting,

    /// <summary>
    /// The build expired, so testers can no longer install it.
    /// </summary>
    [EnumMember(Value = "EXPIRED")]
    Expired,

    /// <summary>
    /// The build is ready for you to submit it for beta review.
    /// </summary>
    [EnumMember(Value = "READY_FOR_BETA_SUBMISSION")]
    ReadyForBetaSubmission,

    /// <summary>
    /// Apple is reviewing the export compliance information you provided for the build.
    /// </summary>
    [EnumMember(Value = "IN_EXPORT_COMPLIANCE_REVIEW")]
    InExportComplianceReview,

    /// <summary>
    /// You submitted the build for beta review, and it's queued.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_BETA_REVIEW")]
    WaitingForBetaReview,

    /// <summary>
    /// App Review is currently reviewing the build.
    /// </summary>
    [EnumMember(Value = "IN_BETA_REVIEW")]
    InBetaReview,

    /// <summary>
    /// App Review rejected the build for external testing.
    /// </summary>
    [EnumMember(Value = "BETA_REJECTED")]
    BetaRejected,

    /// <summary>
    /// App Review approved the build for external testing.
    /// </summary>
    [EnumMember(Value = "BETA_APPROVED")]
    BetaApproved,

    /// <summary>
    /// External testing doesn't apply to the build.
    /// </summary>
    [EnumMember(Value = "NOT_APPLICABLE")]
    NotApplicable
}
