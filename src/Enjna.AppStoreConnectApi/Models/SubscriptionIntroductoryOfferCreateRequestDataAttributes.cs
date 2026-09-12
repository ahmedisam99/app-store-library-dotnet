using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an introductory offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryoffercreaterequest/data/attributes"/>
public sealed class SubscriptionIntroductoryOfferCreateRequestDataAttributes
{
    /// <summary>
    /// The day the offer starts. Leave it out to start the offer as soon as the App Store accepts it.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The last day the offer is available. Leave it out to run the offer until you delete it.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }

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
    [JsonPropertyName("numberOfPeriods")]
    public required int NumberOfPeriods { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. Set it only for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
