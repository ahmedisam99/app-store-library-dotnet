using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The review state of the localized metadata of an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalization/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionLocalizationState>))]
public enum SubscriptionLocalizationState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The localization is ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// You submitted the localization, and it's waiting for App Review to start.
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
