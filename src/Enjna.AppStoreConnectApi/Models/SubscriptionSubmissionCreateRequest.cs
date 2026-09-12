using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to submit an auto-renewable subscription to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionsubmissioncreaterequest"/>
public sealed class SubscriptionSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionSubmissionCreateRequestData Data { get; set; }
}
