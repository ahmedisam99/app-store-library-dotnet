using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that sets a subscription's territory availability.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailabilitycreaterequest/data/attributes"/>
[Obsolete("Apple deprecated this request. Use SubscriptionPlanAvailabilityCreateRequestDataAttributes instead.")]
public sealed class SubscriptionAvailabilityCreateRequestDataAttributes
{
    /// <summary>
    /// Whether the subscription goes on sale automatically in territories the App Store adds later.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public required bool AvailableInNewTerritories { get; set; }
}
