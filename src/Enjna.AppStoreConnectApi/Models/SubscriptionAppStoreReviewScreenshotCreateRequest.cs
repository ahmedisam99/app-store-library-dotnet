using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve an App Review screenshot for an auto-renewable
/// subscription. App Store Connect answers with the operations that upload the file's bytes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotcreaterequest"/>
public sealed class SubscriptionAppStoreReviewScreenshotCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionAppStoreReviewScreenshotCreateRequestData Data { get; set; }
}
