using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that creates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationcreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2CreateRequestData instead.")]
public sealed class SubscriptionLocalizationCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. This value is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionLocalizationCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This value is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionLocalizationCreateRequestDataRelationships Relationships { get; set; }
}
