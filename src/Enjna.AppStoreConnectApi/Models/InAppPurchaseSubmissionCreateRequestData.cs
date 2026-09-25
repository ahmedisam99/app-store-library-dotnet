using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that submits an in-app purchase to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasesubmissioncreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit an in-app purchase version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class InAppPurchaseSubmissionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseSubmissions";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseSubmissionCreateRequestDataRelationships Relationships { get; set; }
}
