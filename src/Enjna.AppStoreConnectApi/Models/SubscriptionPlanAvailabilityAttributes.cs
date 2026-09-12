using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Plan Availabilities resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailability/attributes"/>
public sealed class SubscriptionPlanAvailabilityAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether the plan goes on sale automatically in territories
    /// the App Store adds after you set its availability.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public bool? AvailableInNewTerritories { get; set; }

    /// <summary>
    /// The billing plan the availability applies to.
    /// </summary>
    [JsonPropertyName("planType")]
    public SubscriptionPlanType? PlanType { get; set; }
}
