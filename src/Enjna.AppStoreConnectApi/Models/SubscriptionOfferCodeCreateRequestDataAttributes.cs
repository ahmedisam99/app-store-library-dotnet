using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates a Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecreaterequest"/>
public sealed class SubscriptionOfferCodeCreateRequestDataAttributes
{
    /// <summary>
    /// The name that identifies the offer code in App Store Connect. It is required, and customers
    /// never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The subscription history that makes a customer eligible to redeem the offer code. It is
    /// required, and takes at least one value.
    /// </summary>
    [JsonPropertyName("customerEligibilities")]
    public required SubscriptionCustomerEligibility[] CustomerEligibilities { get; set; }

    /// <summary>
    /// How the offer interacts with the subscription's introductory offer. It is required.
    /// </summary>
    [JsonPropertyName("offerEligibility")]
    public required SubscriptionOfferEligibility OfferEligibility { get; set; }

    /// <summary>
    /// The length of one discounted period. It is required.
    /// </summary>
    [JsonPropertyName("duration")]
    public required SubscriptionOfferDuration Duration { get; set; }

    /// <summary>
    /// How the customer pays for the offer: as a free trial, up front, or as they go. It is
    /// required.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public required SubscriptionOfferMode OfferMode { get; set; }

    /// <summary>
    /// The number of discounted periods the offer runs for. It is required.
    /// </summary>
    [JsonPropertyName("numberOfPeriods")]
    public required int NumberOfPeriods { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the subscription renews at the standard price when
    /// the offer's discounted periods end.
    /// </summary>
    [JsonPropertyName("autoRenewEnabled")]
    public bool? AutoRenewEnabled { get; set; }

    /// <summary>
    /// The billing plan the offer applies to, for a subscription that offers more than one.
    /// </summary>
    [JsonPropertyName("targetSubscriptionPlanType")]
    public SubscriptionPlanType? TargetSubscriptionPlanType { get; set; }
}
