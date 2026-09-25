using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Where a subscription group version sits in the App Review workflow.
/// </summary>
/// <remarks>
/// A version moves from <c>PREPARE_FOR_SUBMISSION</c> to <c>READY_FOR_REVIEW</c> when you add it to a
/// review submission, to <c>WAITING_FOR_REVIEW</c> when you submit that review submission, then to
/// <c>IN_REVIEW</c>, and finally to <c>APPROVED</c> or <c>REJECTED</c>. Apple documents
/// <c>ACCEPTED</c> and <c>APPROVED</c> together, as a version that passed review, without saying how
/// they differ.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupversion/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionGroupVersionState>))]
public enum SubscriptionGroupVersionState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The version is being edited, so its localizations can be added, changed, or removed.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// The version is attached to a review submission that isn't submitted yet.
    /// </summary>
    [EnumMember(Value = "READY_FOR_REVIEW")]
    ReadyForReview,

    /// <summary>
    /// The review submission was submitted, and the version is queued for App Review.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review is reviewing the version.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// The version passed review.
    /// </summary>
    [EnumMember(Value = "ACCEPTED")]
    Accepted,

    /// <summary>
    /// The version passed review.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// A newer version supersedes this one.
    /// </summary>
    [EnumMember(Value = "REPLACED_WITH_NEW_VERSION")]
    ReplacedWithNewVersion,

    /// <summary>
    /// App Review rejected the version.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected,

    /// <summary>
    /// The developer withdrew the version.
    /// </summary>
    [EnumMember(Value = "DEVELOPER_REJECTED")]
    DeveloperRejected
}
