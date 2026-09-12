using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to schedule what a subscription costs in one territory.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricecreaterequest"/>
public sealed class SubscriptionPriceCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionPriceCreateRequestData Data { get; set; }
}
