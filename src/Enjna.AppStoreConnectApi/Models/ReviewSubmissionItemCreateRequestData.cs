using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that adds an item to a review submission. An item has no
/// attributes of its own; its relationships name the submission and the thing to review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitemcreaterequest/data"/>
public sealed class ReviewSubmissionItemCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>reviewSubmissionItems</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reviewSubmissionItems";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required ReviewSubmissionItemCreateRequestDataRelationships Relationships { get; set; }
}
