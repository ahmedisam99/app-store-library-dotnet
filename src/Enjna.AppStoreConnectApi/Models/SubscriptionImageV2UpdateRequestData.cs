using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that commits a Subscription Images resource of the v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2updaterequest/data"/>
public sealed class SubscriptionImageV2UpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionImages";

    /// <summary>
    /// The opaque resource ID of the reserved image. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that commit the upload.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionImageV2UpdateRequestDataAttributes? Attributes { get; set; }
}
