using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Where one item of a review submission sits in App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitem/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<ReviewSubmissionItemState>))]
public enum ReviewSubmissionItemState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The item is ready for review.
    /// </summary>
    [EnumMember(Value = "READY_FOR_REVIEW")]
    ReadyForReview,

    /// <summary>
    /// App Review accepted the item.
    /// </summary>
    [EnumMember(Value = "ACCEPTED")]
    Accepted,

    /// <summary>
    /// App Review approved the item.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// App Review rejected the item.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected,

    /// <summary>
    /// The item is removed from the submission.
    /// </summary>
    [EnumMember(Value = "REMOVED")]
    Removed
}
