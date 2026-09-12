using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a Subscription Offer Code One-Time Use Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeonetimeusecodeupdaterequest"/>
public sealed class SubscriptionOfferCodeOneTimeUseCodeUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionOfferCodeOneTimeUseCodeUpdateRequestData Data { get; set; }
}
