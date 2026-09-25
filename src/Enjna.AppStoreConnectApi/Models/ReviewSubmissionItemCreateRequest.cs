using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to add an item to a review submission. An item stands for one thing to
/// review, such as an in-app purchase version, so add one item for each version you submit.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitemcreaterequest"/>
public sealed class ReviewSubmissionItemCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required ReviewSubmissionItemCreateRequestData Data { get; set; }
}
