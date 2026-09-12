using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a Subscription Offer Code Custom Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcodecreaterequest"/>
public sealed class SubscriptionOfferCodeCustomCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionOfferCodeCustomCodeCreateRequestData Data { get; set; }
}
