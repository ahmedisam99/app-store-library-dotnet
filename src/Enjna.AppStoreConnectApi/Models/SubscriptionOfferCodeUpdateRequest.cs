using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a Subscription Offer Codes resource. An offer code is
/// otherwise immutable: only its active state changes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeupdaterequest"/>
public sealed class SubscriptionOfferCodeUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionOfferCodeUpdateRequestData Data { get; set; }
}
