using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Offer Code One-Time Use Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeonetimeusecodecreaterequest"/>
public sealed class SubscriptionOfferCodeOneTimeUseCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionOfferCodeOneTimeUseCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionOfferCodeOneTimeUseCodes";

    /// <summary>
    /// The attributes that describe the request that creates a Subscription Offer Code One-Time Use
    /// Codes resource. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionOfferCodeOneTimeUseCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionOfferCodeOneTimeUseCodeCreateRequestDataRelationships Relationships { get; set; }
}
