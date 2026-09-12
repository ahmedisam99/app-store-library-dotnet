using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that changes an introductory offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryofferupdaterequest/data"/>
public sealed class SubscriptionIntroductoryOfferUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionIntroductoryOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionIntroductoryOffers";

    /// <summary>
    /// The opaque resource ID of the introductory offer to change. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionIntroductoryOfferUpdateRequestDataAttributes? Attributes { get; set; }
}
