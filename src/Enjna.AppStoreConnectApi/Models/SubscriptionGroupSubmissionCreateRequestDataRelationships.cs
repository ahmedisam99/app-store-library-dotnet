using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set on a request that submits a subscription group for review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmissioncreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit a subscription group version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class SubscriptionGroupSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group to submit for review. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscriptionGroup")]
    public required RelationshipDeclaration SubscriptionGroup { get; set; }
}
