using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissioncreaterequest/data/attributes"/>
public sealed class ReviewSubmissionCreateRequestDataAttributes
{
    /// <summary>
    /// The platform the submission is for. Apple no longer requires it when you create a
    /// submission, and you can add it later when you update the submission.
    /// </summary>
    [JsonPropertyName("platform")]
    public Platform? Platform { get; set; }
}
