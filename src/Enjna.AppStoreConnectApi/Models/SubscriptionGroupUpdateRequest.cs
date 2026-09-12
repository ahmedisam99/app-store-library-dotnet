using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupupdaterequest"/>
public sealed class SubscriptionGroupUpdateRequest
{
    /// <summary>
    /// The resource data. It is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionGroupUpdateRequestData Data { get; set; }
}
