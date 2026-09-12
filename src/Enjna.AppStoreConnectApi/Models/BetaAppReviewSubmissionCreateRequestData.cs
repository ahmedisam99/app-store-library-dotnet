using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data of a request that creates a Beta App Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betaappreviewsubmissioncreaterequest/data"/>
public sealed class BetaAppReviewSubmissionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>betaAppReviewSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "betaAppReviewSubmissions";

    /// <summary>
    /// The relationships to other resources that you set with this request. These relationships
    /// are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required BetaAppReviewSubmissionCreateRequestDataRelationships Relationships { get; set; }
}
