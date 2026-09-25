using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a Review Submission Items resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitemupdaterequest/data/attributes"/>
public sealed class ReviewSubmissionItemUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Set to <c>true</c> to mark the item resolved.
    /// </summary>
    [JsonPropertyName("resolved")]
    public bool? Resolved { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// Set to <c>true</c> to mark the item removed.
    /// </summary>
    [JsonPropertyName("removed")]
    public bool? Removed { get => Get<bool?>(); set => Set(value); }
}
