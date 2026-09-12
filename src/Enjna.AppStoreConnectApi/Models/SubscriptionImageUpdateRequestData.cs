using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that commits a subscription promotional image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimageupdaterequest/data"/>
public sealed class SubscriptionImageUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionImages";

    /// <summary>
    /// The opaque resource ID of the reserved image. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that commit the upload.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionImageUpdateRequestDataAttributes? Attributes { get; set; }
}
