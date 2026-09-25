using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissioncreaterequest/data"/>
public sealed class ReviewSubmissionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>reviewSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reviewSubmissions";

    /// <summary>
    /// The attributes that describe the submission to create.
    /// </summary>
    [JsonPropertyName("attributes")]
    public ReviewSubmissionCreateRequestDataAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required ReviewSubmissionCreateRequestDataRelationships Relationships { get; set; }
}
