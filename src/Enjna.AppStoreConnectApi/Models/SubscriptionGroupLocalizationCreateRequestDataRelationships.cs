using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set on a request that creates a subscription group localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationcreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionGroupLocalizationV2CreateRequestDataRelationships instead.")]
public sealed class SubscriptionGroupLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscriptionGroup")]
    public required RelationshipDeclaration SubscriptionGroup { get; set; }
}
