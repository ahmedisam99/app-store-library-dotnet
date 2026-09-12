using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a price you create inline in a subscription update request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpriceinlinecreate/attributes"/>
public sealed class SubscriptionPriceInlineCreateAttributes
{
    /// <summary>
    /// The day the price takes effect. Leave it <c>null</c> to charge the price as soon as the App
    /// Store accepts it.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// Whether existing subscribers keep the price they pay today instead of moving to this one.
    /// </summary>
    [JsonPropertyName("preserveCurrentPrice")]
    public bool? PreserveCurrentPrice { get; set; }

    /// <summary>
    /// The billing plan the price applies to. Set it only for a subscription that sells more than
    /// one plan.
    /// </summary>
    [JsonPropertyName("planType")]
    public SubscriptionPlanType? PlanType { get; set; }
}
