using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a Subscription Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcodeupdaterequest"/>
public sealed class SubscriptionOfferCodeCustomCodeUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodeCustomCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodeCustomCodes";

    /// <summary>
    /// The opaque resource ID of the custom code you're updating. It is required, and matches the
    /// ID in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Subscription Offer Code Custom Codes
    /// resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionOfferCodeCustomCodeUpdateRequestDataAttributes? Attributes { get; set; }
}
