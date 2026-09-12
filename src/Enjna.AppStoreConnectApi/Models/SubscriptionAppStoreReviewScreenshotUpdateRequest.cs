using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to commit a subscription App Review screenshot once you uploaded its
/// bytes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotupdaterequest"/>
public sealed class SubscriptionAppStoreReviewScreenshotUpdateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionAppStoreReviewScreenshotUpdateRequestData Data { get; set; }
}
