using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Introductory Offers resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryoffer/attributes"/>
public sealed class SubscriptionIntroductoryOfferAttributes
{
    /// <summary>
    /// The day the offer starts. An offer with no start date is already in effect.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The last day the offer is available. An offer with no end date runs until you delete it.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// The length of one period of the offer.
    /// </summary>
    [JsonPropertyName("duration")]
    public SubscriptionOfferDuration? Duration { get; set; }

    /// <summary>
    /// How the offer bills the customer: as a free trial, up front, or period by period.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public SubscriptionOfferMode? OfferMode { get; set; }

    /// <summary>
    /// The number of periods the offer lasts, which only a pay-as-you-go offer can set above one.
    /// </summary>
    [JsonPropertyName("numberOfPeriods")]
    public int? NumberOfPeriods { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. It's only set for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
