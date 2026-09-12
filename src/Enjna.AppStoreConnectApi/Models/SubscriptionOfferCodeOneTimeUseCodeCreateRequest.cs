using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a Subscription Offer Code One-Time Use Codes resource, which
/// generates a batch of unique codes that each customer redeems once.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeonetimeusecodecreaterequest"/>
public sealed class SubscriptionOfferCodeOneTimeUseCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionOfferCodeOneTimeUseCodeCreateRequestData Data { get; set; }
}
