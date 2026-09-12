using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to submit a subscription group for review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmissioncreaterequest"/>
public sealed class SubscriptionGroupSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupSubmissionCreateRequestData Data { get; set; }
}
