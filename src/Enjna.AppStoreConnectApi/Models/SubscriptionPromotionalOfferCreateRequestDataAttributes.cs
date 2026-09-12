using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a promotional offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionaloffercreaterequest/data/attributes"/>
public sealed class SubscriptionPromotionalOfferCreateRequestDataAttributes
{
    /// <summary>
    /// The length of one period of the offer. This attribute is required.
    /// </summary>
    [JsonPropertyName("duration")]
    public required SubscriptionOfferDuration Duration { get; set; }

    /// <summary>
    /// The name of the offer in App Store Connect and in your sales reports. Customers never see it.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The number of periods the offer lasts, which only a pay-as-you-go offer can set above one.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("numberOfPeriods")]
    public required int NumberOfPeriods { get; set; }

    /// <summary>
    /// The identifier your app passes to StoreKit, and signs, to present this offer to a customer.
    /// This attribute is required.
    /// </summary>
    [JsonPropertyName("offerCode")]
    public required string OfferCode { get; set; }

    /// <summary>
    /// How the offer bills the customer: as a free trial, up front, or period by period. This
    /// attribute is required.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public required SubscriptionOfferMode OfferMode { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. Set it only for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
