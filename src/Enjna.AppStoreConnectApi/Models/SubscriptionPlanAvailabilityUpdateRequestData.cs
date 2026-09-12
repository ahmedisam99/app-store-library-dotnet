using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that changes a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilityupdaterequest/data"/>
public sealed class SubscriptionPlanAvailabilityUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPlanAvailabilities</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPlanAvailabilities";

    /// <summary>
    /// The opaque resource ID of the plan availability to change. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionPlanAvailabilityUpdateRequestDataAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to change.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionPlanAvailabilityUpdateRequestDataRelationships? Relationships { get; set; }
}
