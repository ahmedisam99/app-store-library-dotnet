using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that schedules a subscription price.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricecreaterequest/data"/>
public sealed class SubscriptionPriceCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPrices";

    /// <summary>
    /// The attributes that describe the request.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionPriceCreateRequestDataAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionPriceCreateRequestDataRelationships Relationships { get; set; }
}
