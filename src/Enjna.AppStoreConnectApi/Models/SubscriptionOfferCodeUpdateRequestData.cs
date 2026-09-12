using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeupdaterequest"/>
public sealed class SubscriptionOfferCodeUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodes";

    /// <summary>
    /// The opaque resource ID of the offer code you're updating. It is required, and matches the ID
    /// in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Subscription Offer Codes resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionOfferCodeUpdateRequestDataAttributes? Attributes { get; set; }
}
