using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The review and availability state of an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscription/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionState>))]
public enum SubscriptionState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The subscription is missing metadata that App Store Connect requires before you can submit it.
    /// </summary>
    [EnumMember(Value = "MISSING_METADATA")]
    MissingMetadata,

    /// <summary>
    /// The subscription is complete and ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "READY_TO_SUBMIT")]
    ReadyToSubmit,

    /// <summary>
    /// You submitted the subscription, and it's waiting for App Review to start.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review is reviewing the subscription.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// The subscription needs you to take an action before the review can continue.
    /// </summary>
    [EnumMember(Value = "DEVELOPER_ACTION_NEEDED")]
    DeveloperActionNeeded,

    /// <summary>
    /// The subscription is approved, and waits for the approval of the app version it ships with.
    /// </summary>
    [EnumMember(Value = "PENDING_BINARY_APPROVAL")]
    PendingBinaryApproval,

    /// <summary>
    /// App Review approved the subscription, and it's available for purchase.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// You removed the subscription from sale.
    /// </summary>
    [EnumMember(Value = "DEVELOPER_REMOVED_FROM_SALE")]
    DeveloperRemovedFromSale,

    /// <summary>
    /// The subscription is removed from sale.
    /// </summary>
    [EnumMember(Value = "REMOVED_FROM_SALE")]
    RemovedFromSale,

    /// <summary>
    /// App Review rejected the subscription.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected
}
