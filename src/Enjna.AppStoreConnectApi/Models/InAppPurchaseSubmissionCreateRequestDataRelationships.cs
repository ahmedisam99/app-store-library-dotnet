using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that submits an in-app purchase to App Review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasesubmissioncreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Submit an in-app purchase version through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
public sealed class InAppPurchaseSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase to submit. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public required RelationshipDeclaration InAppPurchaseV2 { get; set; }
}
