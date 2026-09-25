using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that creates a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationcreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2CreateRequestData instead.")]
public sealed class SubscriptionGroupLocalizationCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionGroupLocalizationCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionGroupLocalizationCreateRequestDataRelationships Relationships { get; set; }
}
