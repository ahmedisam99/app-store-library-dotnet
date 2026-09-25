using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a Review Submissions resource for an app.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissioncreaterequest"/>
public sealed class ReviewSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required ReviewSubmissionCreateRequestData Data { get; set; }
}
