using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmission/attributes"/>
public sealed class ReviewSubmissionAttributes
{
    /// <summary>
    /// The platform the submission is for.
    /// </summary>
    [JsonPropertyName("platform")]
    public Platform? Platform { get; set; }

    /// <summary>
    /// The date and time the submission went to App Review.
    /// </summary>
    [JsonPropertyName("submittedDate")]
    public DateTimeOffset? SubmittedDate { get; set; }

    /// <summary>
    /// Where the submission sits in App Review.
    /// </summary>
    [JsonPropertyName("state")]
    public ReviewSubmissionState? State { get; set; }
}
