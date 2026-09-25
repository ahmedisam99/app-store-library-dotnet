using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Subscription Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionversioncreaterequest/data/relationships"/>
public sealed class SubscriptionVersionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription whose metadata the version holds, of type <c>subscriptions</c>. This
    /// relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }
}
