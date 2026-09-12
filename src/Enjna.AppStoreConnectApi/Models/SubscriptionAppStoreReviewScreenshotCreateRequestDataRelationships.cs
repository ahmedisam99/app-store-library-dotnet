using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that reserves a subscription App Review screenshot.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotcreaterequest/data/relationships"/>
public sealed class SubscriptionAppStoreReviewScreenshotCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription the screenshot belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
