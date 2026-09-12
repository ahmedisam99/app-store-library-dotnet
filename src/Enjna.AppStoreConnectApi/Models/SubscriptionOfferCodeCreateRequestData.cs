using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecreaterequest"/>
public sealed class SubscriptionOfferCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodes";

    /// <summary>
    /// The attributes that describe the request that creates a Subscription Offer Codes resource.
    /// They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionOfferCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionOfferCodeCreateRequestDataRelationships Relationships { get; set; }
}
