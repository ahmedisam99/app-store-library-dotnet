using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that sets a subscription's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailabilitycreaterequest/data/relationships"/>
public sealed class SubscriptionAvailabilityCreateRequestDataRelationships
{
    /// <summary>
    /// The related Subscriptions resource. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }

    /// <summary>
    /// The related Territories resources the subscription is for sale in. This relationship is
    /// required.
    /// </summary>
    [JsonPropertyName("availableTerritories")]
    public required RelationshipDeclarationList AvailableTerritories { get; set; }
}
