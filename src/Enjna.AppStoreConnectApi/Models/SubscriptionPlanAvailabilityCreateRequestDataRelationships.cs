using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that sets a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilitycreaterequest/data/relationships"/>
public sealed class SubscriptionPlanAvailabilityCreateRequestDataRelationships
{
    /// <summary>
    /// The related Subscriptions resource. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscription")]
    public required RelationshipDeclaration Subscription { get; set; }

    /// <summary>
    /// The related Territories resources the plan is for sale in. This relationship is required.
    /// </summary>
    [JsonPropertyName("availableTerritories")]
    public required RelationshipDeclarationList AvailableTerritories { get; set; }
}
