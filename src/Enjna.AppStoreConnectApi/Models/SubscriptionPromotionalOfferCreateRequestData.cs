using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionaloffercreaterequest/data"/>
public sealed class SubscriptionPromotionalOfferCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPromotionalOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPromotionalOffers";

    /// <summary>
    /// The attributes that describe the request. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionPromotionalOfferCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionPromotionalOfferCreateRequestDataRelationships Relationships { get; set; }
}
