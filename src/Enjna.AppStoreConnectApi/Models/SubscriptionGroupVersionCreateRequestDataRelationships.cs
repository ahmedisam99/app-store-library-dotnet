using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Subscription Group Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupversioncreaterequest/data/relationships"/>
public sealed class SubscriptionGroupVersionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group whose metadata you're updating, of type <c>subscriptionGroups</c>.
    /// This relationship is required.
    /// </summary>
    [JsonPropertyName("subscriptionGroup")]
    public required RelationshipDeclaration SubscriptionGroup { get; set; }
}
