using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a subscription group localization of the version-based
/// workflow.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2updaterequest"/>
public sealed class SubscriptionGroupLocalizationV2UpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupLocalizationV2UpdateRequestData Data { get; set; }
}
