using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that creates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationcreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2CreateRequestDataRelationships instead.")]
public sealed class SubscriptionLocalizationCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription the localization belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
