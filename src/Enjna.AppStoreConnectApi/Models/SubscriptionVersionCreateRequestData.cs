using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Versions resource. The request
/// carries no attributes, only the subscription the version belongs to.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionversioncreaterequest/data"/>
public sealed class SubscriptionVersionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionVersions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionVersions";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionVersionCreateRequestDataRelationships Relationships { get; set; }
}
