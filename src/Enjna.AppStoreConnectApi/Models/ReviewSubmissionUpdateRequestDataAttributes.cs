using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionupdaterequest/data/attributes"/>
public sealed class ReviewSubmissionUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The platform the submission is for. You can add it here if you left it out when you
    /// created the submission.
    /// </summary>
    [JsonPropertyName("platform")]
    public Platform? Platform { get => Get<Platform?>(); set => Set(value); }

    /// <summary>
    /// Set to <c>true</c> to submit the review submission to App Review, once you've added its items.
    /// </summary>
    [JsonPropertyName("submitted")]
    public bool? Submitted { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// Set to <c>true</c> to cancel the review submission.
    /// </summary>
    [JsonPropertyName("canceled")]
    public bool? Canceled { get => Get<bool?>(); set => Set(value); }
}
