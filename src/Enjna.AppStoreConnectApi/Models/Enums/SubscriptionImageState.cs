using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The upload and review state of a promotional image for an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimage/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionImageState>))]
public enum SubscriptionImageState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// App Store Connect reserved the image, and waits for you to upload its bytes.
    /// </summary>
    [EnumMember(Value = "AWAITING_UPLOAD")]
    AwaitingUpload,

    /// <summary>
    /// The image uploaded successfully.
    /// </summary>
    [EnumMember(Value = "UPLOAD_COMPLETE")]
    UploadComplete,

    /// <summary>
    /// The upload failed, or App Store Connect couldn't process the image.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    Failed,

    /// <summary>
    /// The image is ready for you to submit for review.
    /// </summary>
    [EnumMember(Value = "PREPARE_FOR_SUBMISSION")]
    PrepareForSubmission,

    /// <summary>
    /// You submitted the image, and it's waiting for App Review to start.
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
