using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that changes a billing grace period.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongraceperiodupdaterequest/data"/>
public sealed class SubscriptionGracePeriodUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGracePeriods</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGracePeriods";

    /// <summary>
    /// The opaque resource ID of the billing grace period to change. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave one out to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionGracePeriodUpdateRequestDataAttributes? Attributes { get; set; }
}
