using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Subscription Localizations resource of the
/// v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationv2createrequest/data"/>
public sealed class SubscriptionLocalizationV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionLocalizationV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionLocalizationV2CreateRequestDataRelationships Relationships { get; set; }
}
