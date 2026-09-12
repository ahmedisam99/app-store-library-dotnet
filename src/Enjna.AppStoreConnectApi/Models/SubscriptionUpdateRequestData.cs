using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that updates an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionupdaterequest/data"/>
public sealed class SubscriptionUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptions";

    /// <summary>
    /// The opaque resource ID of the subscription to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionUpdateRequestDataAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to replace with this request.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionUpdateRequestDataRelationships? Relationships { get; set; }
}
