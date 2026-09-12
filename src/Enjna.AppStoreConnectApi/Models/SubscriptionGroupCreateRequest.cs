using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupcreaterequest"/>
public sealed class SubscriptionGroupCreateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupCreateRequestData Data { get; set; }
}
