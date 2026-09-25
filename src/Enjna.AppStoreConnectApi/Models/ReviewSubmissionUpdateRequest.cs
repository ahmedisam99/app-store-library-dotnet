using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a Review Submissions resource, which is how you submit a
/// review submission to App Review or cancel it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionupdaterequest"/>
public sealed class ReviewSubmissionUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required ReviewSubmissionUpdateRequestData Data { get; set; }
}
