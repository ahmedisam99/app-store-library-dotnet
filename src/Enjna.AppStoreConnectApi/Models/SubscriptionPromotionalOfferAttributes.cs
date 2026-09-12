using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Promotional Offers resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpromotionaloffer/attributes"/>
public sealed class SubscriptionPromotionalOfferAttributes
{
    /// <summary>
    /// The length of one period of the offer.
    /// </summary>
    [JsonPropertyName("duration")]
    public SubscriptionOfferDuration? Duration { get; set; }

    /// <summary>
    /// The name of the offer in App Store Connect and in your sales reports. Customers never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The number of periods the offer lasts, which only a pay-as-you-go offer can set above one.
    /// </summary>
    [JsonPropertyName("numberOfPeriods")]
    public int? NumberOfPeriods { get; set; }

    /// <summary>
    /// The identifier your app passes to StoreKit, and signs, to present this offer to a customer.
    /// </summary>
    [JsonPropertyName("offerCode")]
    public string? OfferCode { get; set; }

    /// <summary>
    /// How the offer bills the customer: as a free trial, up front, or period by period.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public SubscriptionOfferMode? OfferMode { get; set; }

    /// <summary>
    /// The billing plan of the subscription the offer applies to. It's only set for a subscription
    /// that sells more than one plan.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
