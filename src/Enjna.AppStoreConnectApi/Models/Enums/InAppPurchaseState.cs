using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasestate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<InAppPurchaseState>))]
public enum InAppPurchaseState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The in-app purchase is missing required metadata.
    /// </summary>
    [EnumMember(Value = "MISSING_METADATA")]
    MissingMetadata,

    /// <summary>
    /// The in-app purchase is waiting for you to upload required content.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_UPLOAD")]
    WaitingForUpload,

    /// <summary>
    /// App Store Connect is processing the in-app purchase content.
    /// </summary>
    [EnumMember(Value = "PROCESSING_CONTENT")]
    ProcessingContent,

    /// <summary>
    /// The in-app purchase is ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "READY_TO_SUBMIT")]
    ReadyToSubmit,

    /// <summary>
    /// The in-app purchase is waiting for App Review.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review is reviewing the in-app purchase.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// The in-app purchase requires an action from you.
    /// </summary>
    [EnumMember(Value = "DEVELOPER_ACTION_NEEDED")]
    DeveloperActionNeeded,

    /// <summary>
    /// The in-app purchase is waiting for the approval of an app version.
    /// </summary>
    [EnumMember(Value = "PENDING_BINARY_APPROVAL")]
    PendingBinaryApproval,

    /// <summary>
    /// App Review approved the in-app purchase.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// You removed the in-app purchase from sale.
    /// </summary>
    [EnumMember(Value = "DEVELOPER_REMOVED_FROM_SALE")]
    DeveloperRemovedFromSale,

    /// <summary>
    /// The in-app purchase is removed from sale.
    /// </summary>
    [EnumMember(Value = "REMOVED_FROM_SALE")]
    RemovedFromSale,

    /// <summary>
    /// App Review rejected the in-app purchase.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected
}
