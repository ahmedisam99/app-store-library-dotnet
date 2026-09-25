using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to submit a subscription group for review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmissioncreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit a subscription group version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class SubscriptionGroupSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupSubmissionCreateRequestData Data { get; set; }
}
