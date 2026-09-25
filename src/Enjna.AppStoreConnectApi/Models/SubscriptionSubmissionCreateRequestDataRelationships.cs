using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that submits a subscription to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionsubmissioncreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit a subscription version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class SubscriptionSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription to submit. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
