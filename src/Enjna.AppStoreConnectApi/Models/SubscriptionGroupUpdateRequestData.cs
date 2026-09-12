using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that updates a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupupdaterequest/data"/>
public sealed class SubscriptionGroupUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroups</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroups";

    /// <summary>
    /// The opaque resource ID of the subscription group to update. It is required, and matches the
    /// ID in the request path.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionGroupUpdateRequestDataAttributes? Attributes { get; set; }
}
