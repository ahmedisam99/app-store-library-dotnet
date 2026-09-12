using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptioncreaterequest"/>
public sealed class SubscriptionCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionCreateRequestData Data { get; set; }
}
