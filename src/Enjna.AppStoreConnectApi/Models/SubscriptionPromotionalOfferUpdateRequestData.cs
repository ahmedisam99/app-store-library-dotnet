using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that changes a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionalofferupdaterequest/data"/>
public sealed class SubscriptionPromotionalOfferUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPromotionalOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPromotionalOffers";

    /// <summary>
    /// The opaque resource ID of the promotional offer to change. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The relationships to change.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionPromotionalOfferUpdateRequestDataRelationships? Relationships { get; set; }
}
