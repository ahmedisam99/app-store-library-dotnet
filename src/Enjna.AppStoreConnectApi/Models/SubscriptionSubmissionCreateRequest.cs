using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to submit an auto-renewable subscription to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionsubmissioncreaterequest"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit a subscription version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class SubscriptionSubmissionCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionSubmissionCreateRequestData Data { get; set; }
}
