using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that creates a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupcreaterequest/data"/>
public sealed class SubscriptionGroupCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroups</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroups";

    /// <summary>
    /// The attributes that describe the subscription group to create. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionGroupCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionGroupCreateRequestDataRelationships Relationships { get; set; }
}
