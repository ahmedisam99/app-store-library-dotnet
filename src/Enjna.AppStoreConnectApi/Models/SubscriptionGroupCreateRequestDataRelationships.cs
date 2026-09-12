using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set on a request that creates a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupcreaterequest/data/relationships"/>
public sealed class SubscriptionGroupCreateRequestDataRelationships
{
    /// <summary>
    /// The app the subscription group belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }
}
