using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Group Versions resource. The
/// request has no attributes; the subscription group you point the relationship at is the whole
/// of it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupversioncreaterequest/data"/>
public sealed class SubscriptionGroupVersionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupVersions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupVersions";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionGroupVersionCreateRequestDataRelationships Relationships { get; set; }
}
