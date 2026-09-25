using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that submits a subscription to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionsubmissioncreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit a subscription version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class SubscriptionSubmissionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionSubmissions";

    /// <summary>
    /// The relationships to other resources that you set with this request. This value is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionSubmissionCreateRequestDataRelationships Relationships { get; set; }
}
