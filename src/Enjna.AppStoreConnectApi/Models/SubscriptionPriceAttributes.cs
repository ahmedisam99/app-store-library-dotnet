using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Prices resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionprice/attributes"/>
public sealed class SubscriptionPriceAttributes
{
    /// <summary>
    /// The day the price takes effect. A price with no start date is the current price.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether existing subscribers keep the price they pay today
    /// instead of moving to this one.
    /// </summary>
    [JsonPropertyName("preserved")]
    public bool? Preserved { get; set; }

    /// <summary>
    /// The billing plan the price applies to. It's only set for a subscription that sells more
    /// than one plan.
    /// </summary>
    [JsonPropertyName("planType")]
    public SubscriptionPlanType? PlanType { get; set; }
}
