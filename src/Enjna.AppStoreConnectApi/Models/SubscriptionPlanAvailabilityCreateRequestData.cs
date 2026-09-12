using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that sets a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilitycreaterequest/data"/>
public sealed class SubscriptionPlanAvailabilityCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPlanAvailabilities</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPlanAvailabilities";

    /// <summary>
    /// The attributes that describe the request. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionPlanAvailabilityCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionPlanAvailabilityCreateRequestDataRelationships Relationships { get; set; }
}
