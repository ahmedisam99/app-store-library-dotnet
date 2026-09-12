using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Beta App Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betaappreviewsubmission/attributes"/>
public sealed class BetaAppReviewSubmissionAttributes
{
    /// <summary>
    /// The state of the submission in Apple's beta review.
    /// </summary>
    [JsonPropertyName("betaReviewState")]
    public BetaReviewState? BetaReviewState { get; set; }

    /// <summary>
    /// The date and time you submitted the build for beta review.
    /// </summary>
    [JsonPropertyName("submittedDate")]
    public DateTimeOffset? SubmittedDate { get; set; }
}
