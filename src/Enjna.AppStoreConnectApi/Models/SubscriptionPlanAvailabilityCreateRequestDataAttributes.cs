using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that sets a subscription plan's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilitycreaterequest/data/attributes"/>
public sealed class SubscriptionPlanAvailabilityCreateRequestDataAttributes
{
    /// <summary>
    /// Whether the plan goes on sale automatically in territories the App Store adds later.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public bool? AvailableInNewTerritories { get; set; }

    /// <summary>
    /// The billing plan the availability applies to. This attribute is required.
    /// </summary>
    [JsonPropertyName("planType")]
    public required SubscriptionPlanType PlanType { get; set; }
}
