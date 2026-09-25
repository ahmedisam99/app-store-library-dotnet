using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Availabilities resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailability/attributes"/>
[Obsolete("Apple deprecated this resource. Use SubscriptionPlanAvailabilityAttributes instead.")]
public sealed class SubscriptionAvailabilityAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether the subscription goes on sale automatically in
    /// territories the App Store adds after you set its availability.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public bool? AvailableInNewTerritories { get; set; }
}
