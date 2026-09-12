using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that reserves a subscription App Review screenshot.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotcreaterequest/data"/>
public sealed class SubscriptionAppStoreReviewScreenshotCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionAppStoreReviewScreenshots</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionAppStoreReviewScreenshots";

    /// <summary>
    /// The attributes that describe the screenshot to reserve. This value is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionAppStoreReviewScreenshotCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This value is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionAppStoreReviewScreenshotCreateRequestDataRelationships Relationships { get; set; }
}
