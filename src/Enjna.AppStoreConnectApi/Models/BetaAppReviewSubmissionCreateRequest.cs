using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a Beta App Review Submissions resource, which submits a
/// build for the beta review that external testing needs.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betaappreviewsubmissioncreaterequest"/>
public sealed class BetaAppReviewSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BetaAppReviewSubmissionCreateRequestData Data { get; set; }
}
