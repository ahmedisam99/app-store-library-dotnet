using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a subscription group localization on a
/// subscription group version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2createrequest/data"/>
public sealed class SubscriptionGroupLocalizationV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionGroupLocalizationV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionGroupLocalizationV2CreateRequestDataRelationships Relationships { get; set; }
}
