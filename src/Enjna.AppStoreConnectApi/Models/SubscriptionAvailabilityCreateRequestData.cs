using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that sets a subscription's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailabilitycreaterequest/data"/>
public sealed class SubscriptionAvailabilityCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionAvailabilities</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionAvailabilities";

    /// <summary>
    /// The attributes that describe the request. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionAvailabilityCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionAvailabilityCreateRequestDataRelationships Relationships { get; set; }
}
