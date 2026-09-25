using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that reserves a subscription promotional image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagecreaterequest/data/relationships"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionImageV2CreateRequestDataRelationships instead.")]
public sealed class SubscriptionImageCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription the image belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
