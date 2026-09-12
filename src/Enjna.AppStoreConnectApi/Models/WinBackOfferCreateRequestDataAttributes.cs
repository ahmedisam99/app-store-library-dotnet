using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a win-back offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffercreaterequest/data/attributes"/>
public sealed class WinBackOfferCreateRequestDataAttributes
{
    /// <summary>
    /// The name of the offer in App Store Connect and in your sales reports. Customers never see it.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("referenceName")]
    public required string ReferenceName { get; set; }

    /// <summary>
    /// The identifier your app and StoreKit use to refer to this offer. This attribute is required.
    /// </summary>
    [JsonPropertyName("offerId")]
    public required string OfferId { get; set; }

    /// <summary>
    /// The length of one period of the offer. This attribute is required.
    /// </summary>
    [JsonPropertyName("duration")]
    public required SubscriptionOfferDuration Duration { get; set; }

    /// <summary>
    /// How the offer bills the customer: as a free trial, up front, or period by period. This
    /// attribute is required.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public required SubscriptionOfferMode OfferMode { get; set; }

    /// <summary>
    /// The number of periods the offer lasts, which only a pay-as-you-go offer can set above one.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("periodCount")]
    public required int PeriodCount { get; set; }

    /// <summary>
    /// The number of months a customer must have paid for the subscription before their lapse to
    /// qualify for the offer. This attribute is required.
    /// </summary>
    [JsonPropertyName("customerEligibilityPaidSubscriptionDurationInMonths")]
    public required int CustomerEligibilityPaidSubscriptionDurationInMonths { get; set; }

    /// <summary>
    /// The window, in months since the customer's subscription lapsed, within which they qualify
    /// for the offer. This attribute is required.
    /// </summary>
    [JsonPropertyName("customerEligibilityTimeSinceLastSubscribedInMonths")]
    public required IntegerRange CustomerEligibilityTimeSinceLastSubscribedInMonths { get; set; }

    /// <summary>
    /// The number of months that must pass before a customer who declined the offer can receive it
    /// again.
    /// </summary>
    [JsonPropertyName("customerEligibilityWaitBetweenOffersInMonths")]
    public int? CustomerEligibilityWaitBetweenOffersInMonths { get; set; }

    /// <summary>
    /// The day the offer starts. This attribute is required.
    /// </summary>
    [JsonPropertyName("startDate")]
    public required DateOnly StartDate { get; set; }

    /// <summary>
    /// The last day the offer is available. Leave it out to run the offer until you delete it.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Which offer wins when a customer qualifies for more than one win-back offer at a time. This
    /// attribute is required.
    /// </summary>
    [JsonPropertyName("priority")]
    public required WinBackOfferPriority Priority { get; set; }

    /// <summary>
    /// Whether the App Store promotes the offer for you, or leaves presenting it to your app.
    /// </summary>
    [JsonPropertyName("promotionIntent")]
    public WinBackOfferPromotionIntent? PromotionIntent { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. Set it only for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
