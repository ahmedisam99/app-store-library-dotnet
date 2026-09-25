using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that reserves a Subscription Images resource of the v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2createrequest/data"/>
public sealed class SubscriptionImageV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionImages";

    /// <summary>
    /// The attributes that describe the image to reserve. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionImageV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionImageV2CreateRequestDataRelationships Relationships { get; set; }
}
