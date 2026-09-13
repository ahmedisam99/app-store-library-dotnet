using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreServerLibrary.Models.Enums;

/// <summary>
/// The approval state of an image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/retentionmessaging/imagestate"/>
[JsonConverter(typeof(JsonEnumMemberConverter<ImageState>))]
public enum ImageState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The image is awaiting approval.
    /// </summary>
    [EnumMember(Value = "PENDING")]
    Pending,

    /// <summary>
    /// The image is approved.
    /// </summary>
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// The image is rejected.
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected,

    /// <summary>
    /// The image is awaiting approval. The same value as <see cref="Pending"/>, under the name this
    /// library used while it mapped the wire value as PENDING_REVIEW.
    /// </summary>
    [EnumMember(Value = "PENDING")]
    [Obsolete("Use Pending. Apple documents the value as PENDING, and PENDING_REVIEW never arrives.")]
    PendingReview = Pending
}
