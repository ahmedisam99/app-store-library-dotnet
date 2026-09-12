using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that commits a subscription App Review screenshot.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotupdaterequest/data"/>
public sealed class SubscriptionAppStoreReviewScreenshotUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionAppStoreReviewScreenshots</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionAppStoreReviewScreenshots";

    /// <summary>
    /// The opaque resource ID of the reserved screenshot. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that commit the upload.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionAppStoreReviewScreenshotUpdateRequestDataAttributes? Attributes { get; set; }
}
