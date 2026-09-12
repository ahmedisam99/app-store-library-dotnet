using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that creates an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptioncreaterequest/data"/>
public sealed class SubscriptionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptions";

    /// <summary>
    /// The attributes that describe the subscription to create. This value is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This value is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionCreateRequestDataRelationships Relationships { get; set; }
}
