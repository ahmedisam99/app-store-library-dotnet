using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcodecreaterequest"/>
public sealed class SubscriptionOfferCodeCustomCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodeCustomCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodeCustomCodes";

    /// <summary>
    /// The attributes that describe the request that creates a Subscription Offer Code Custom Codes
    /// resource. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionOfferCodeCustomCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionOfferCodeCustomCodeCreateRequestDataRelationships Relationships { get; set; }
}
