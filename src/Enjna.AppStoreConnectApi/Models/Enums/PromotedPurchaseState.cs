using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Where a promoted purchase sits in the App Review workflow.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchase/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<PromotedPurchaseState>))]
public enum PromotedPurchaseState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The promoted purchase is editable and ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// App Review is reviewing the promoted purchase.
    /// </summary>
    [EnumMember(Value = "IN_REVIEW")]
    InReview,

    /// <summary>
    /// App Review approved the promoted purchase.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// App Review rejected the promoted purchase.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected
}
