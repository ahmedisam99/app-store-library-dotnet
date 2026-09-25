using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a subscription group localization of the
/// version-based workflow.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2updaterequest/data"/>
public sealed class SubscriptionGroupLocalizationV2UpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupLocalizations";

    /// <summary>
    /// The opaque resource ID of the localization to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionGroupLocalizationV2UpdateRequestDataAttributes? Attributes { get; set; }
}
