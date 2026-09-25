using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// Where an in-app purchase localization sits in the App Review workflow.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalization/attributes"/>
[Obsolete("Apple deprecated the resource this state belongs to in App Store Connect API 4.4.1. InAppPurchaseLocalizationV2 has no state, so read the InAppPurchaseVersionState of its version instead.")]
[JsonConverter(typeof(JsonEnumMemberConverter<InAppPurchaseLocalizationState>))]
public enum InAppPurchaseLocalizationState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The localization is editable and ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// The localization is waiting for App Review.
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
