using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionupdaterequest/data"/>
public sealed class ReviewSubmissionUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>reviewSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "reviewSubmissions";

    /// <summary>
    /// The opaque resource ID of the review submission to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public ReviewSubmissionUpdateRequestDataAttributes? Attributes { get; set; }
}
