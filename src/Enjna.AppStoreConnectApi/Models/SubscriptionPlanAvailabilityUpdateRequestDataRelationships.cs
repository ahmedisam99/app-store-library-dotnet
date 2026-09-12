using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that changes a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilityupdaterequest/data/relationships"/>
public sealed class SubscriptionPlanAvailabilityUpdateRequestDataRelationships
{
    /// <summary>
    /// The related Territories resources the plan is for sale in.
    /// </summary>
    [JsonPropertyName("availableTerritories")]
    public RelationshipDeclarationList? AvailableTerritories { get; set; }
}
