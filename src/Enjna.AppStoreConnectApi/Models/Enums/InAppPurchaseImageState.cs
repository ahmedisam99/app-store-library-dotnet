using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The state of an in-app purchase image, which covers both its upload and its review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimage/attributes"/>
[Obsolete("Apple deprecated the resource this state belongs to in App Store Connect API 4.4.1. InAppPurchaseImageV2 has no state, so read its AssetDeliveryState for the upload and the InAppPurchaseVersionState of its version for review instead.")]
[JsonConverter(typeof(JsonEnumMemberConverter<InAppPurchaseImageState>))]
public enum InAppPurchaseImageState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// App Store Connect reserved the image and is waiting for you to upload the file.
    /// </summary>
    [EnumMember(Value = "AWAITING_UPLOAD")]
    AwaitingUpload,

    /// <summary>
    /// You uploaded every part of the file and committed the image.
    /// </summary>
    [EnumMember(Value = "UPLOAD_COMPLETE")]
    UploadComplete,

    /// <summary>
    /// App Store Connect couldn't process the image you uploaded.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    Failed,

    /// <summary>
    /// The image is ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// The image is waiting for App Review.
    /// </summary>
    [EnumMember(Value = "WAITING_FOR_REVIEW")]
    WaitingForReview,

    /// <summary>
    /// App Review approved the image.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// App Review rejected the image.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected
}
