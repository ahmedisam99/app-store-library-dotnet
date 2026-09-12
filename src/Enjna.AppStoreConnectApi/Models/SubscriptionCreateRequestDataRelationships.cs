using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request that creates an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptioncreaterequest/data/relationships"/>
public sealed class SubscriptionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group the new subscription belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("group")]
    public required RelationshipDeclaration Group { get; set; }
}
