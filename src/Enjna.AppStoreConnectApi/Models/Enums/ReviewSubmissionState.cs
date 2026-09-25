using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Where a review submission sits in App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmission/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<ReviewSubmissionState>))]
public enum ReviewSubmissionState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The submission is ready for you to submit to App Review.
    /// </summary>
    [EnumMember(Value = "READY_FOR_REVIEW")]
    ReadyForReview,

    /// <summary>
    /// The submission is waiting for App Review.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review is reviewing the submission.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// The submission has issues that aren't resolved yet.
    /// </summary>
    [EnumMember(Value = "UNRESOLVED_ISSUES")]
    UnresolvedIssues,

    /// <summary>
    /// The submission is being canceled.
    /// </summary>
    [EnumMember(Value = "CANCELING")]
    Canceling,

    /// <summary>
    /// The submission is being completed.
    /// </summary>
    [EnumMember(Value = "COMPLETING")]
    Completing,

    /// <summary>
    /// The submission is complete.
    /// </summary>
    [EnumMember(Value = "COMPLETE")]
    Complete
}
