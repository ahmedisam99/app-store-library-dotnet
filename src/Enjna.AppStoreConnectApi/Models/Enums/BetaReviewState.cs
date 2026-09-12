using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of a beta app review submission, as it moves through Apple's review of a build you
/// want to distribute to external testers.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betareviewstate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<BetaReviewState>))]
public enum BetaReviewState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The submission is queued, and review hasn't started yet.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review is currently reviewing the build.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// App Review rejected the build, so it can't go out to external testers.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected,

    /// <summary>
    /// App Review approved the build for external testing.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved
}
