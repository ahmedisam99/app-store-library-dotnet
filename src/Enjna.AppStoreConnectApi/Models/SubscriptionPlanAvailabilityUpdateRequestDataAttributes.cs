using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that changes a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilityupdaterequest/data/attributes"/>
public sealed class SubscriptionPlanAvailabilityUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Whether the plan goes on sale automatically in territories the App Store adds later.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public bool? AvailableInNewTerritories { get => Get<bool?>(); set => Set(value); }
}
