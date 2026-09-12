using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of a subscription group localization in the App Store review process.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalization/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionGroupLocalizationState>))]
public enum SubscriptionGroupLocalizationState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The localization is a draft that you haven't submitted for review yet.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// You submitted the localization, and App Review hasn't started reviewing it yet.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review approved the localization.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// App Review rejected the localization.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected
}
