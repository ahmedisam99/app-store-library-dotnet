using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Win-Back Offers resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffer/attributes"/>
public sealed class WinBackOfferAttributes
{
    /// <summary>
    /// The name of the offer in App Store Connect and in your sales reports. Customers never see it.
    /// </summary>
    [JsonPropertyName("referenceName")]
    public string? ReferenceName { get; set; }

    /// <summary>
    /// The identifier your app and StoreKit use to refer to this offer.
    /// </summary>
    [JsonPropertyName("offerId")]
    public string? OfferId { get; set; }

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
    [JsonPropertyName("periodCount")]
    public int? PeriodCount { get; set; }

    /// <summary>
    /// The number of months a customer must have paid for the subscription before their lapse to
    /// qualify for the offer.
    /// </summary>
    [JsonPropertyName("customerEligibilityPaidSubscriptionDurationInMonths")]
    public int? CustomerEligibilityPaidSubscriptionDurationInMonths { get; set; }

    /// <summary>
    /// The window, in months since the customer's subscription lapsed, within which they qualify
    /// for the offer.
    /// </summary>
    [JsonPropertyName("customerEligibilityTimeSinceLastSubscribedInMonths")]
    public IntegerRange? CustomerEligibilityTimeSinceLastSubscribedInMonths { get; set; }

    /// <summary>
    /// The number of months that must pass before a customer who declined the offer can receive it
    /// again.
    /// </summary>
    [JsonPropertyName("customerEligibilityWaitBetweenOffersInMonths")]
    public int? CustomerEligibilityWaitBetweenOffersInMonths { get; set; }

    /// <summary>
    /// The day the offer starts.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The last day the offer is available. An offer with no end date runs until you delete it.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Which offer wins when a customer qualifies for more than one win-back offer at a time.
    /// </summary>
    [JsonPropertyName("priority")]
    public WinBackOfferPriority? Priority { get; set; }

    /// <summary>
    /// Whether the App Store promotes the offer for you, or leaves presenting it to your app.
    /// </summary>
    [JsonPropertyName("promotionIntent")]
    public WinBackOfferPromotionIntent? PromotionIntent { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. It's only set for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
