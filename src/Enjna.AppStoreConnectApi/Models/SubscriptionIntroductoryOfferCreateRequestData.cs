using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an introductory offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryoffercreaterequest/data"/>
public sealed class SubscriptionIntroductoryOfferCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionIntroductoryOffers</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionIntroductoryOffers";

    /// <summary>
    /// The attributes that describe the request. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionIntroductoryOfferCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionIntroductoryOfferCreateRequestDataRelationships Relationships { get; set; }
}
