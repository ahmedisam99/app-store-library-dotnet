using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a Review Submission Items resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitemupdaterequest/data"/>
public sealed class ReviewSubmissionItemUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>reviewSubmissionItems</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reviewSubmissionItems";

    /// <summary>
    /// The opaque resource ID of the review submission item to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public ReviewSubmissionItemUpdateRequestDataAttributes? Attributes { get; set; }
}
