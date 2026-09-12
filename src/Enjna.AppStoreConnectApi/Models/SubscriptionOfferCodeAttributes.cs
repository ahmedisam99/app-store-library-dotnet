using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercode"/>
public sealed class SubscriptionOfferCodeAttributes
{
    /// <summary>
    /// The name of the offer code, which you use to identify it in App Store Connect. Customers
    /// never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The subscription history that makes a customer eligible to redeem the offer code.
    /// </summary>
    [JsonPropertyName("customerEligibilities")]
    public SubscriptionCustomerEligibility[]? CustomerEligibilities { get; set; }

    /// <summary>
    /// How the offer interacts with the subscription's introductory offer.
    /// </summary>
    [JsonPropertyName("offerEligibility")]
    public SubscriptionOfferEligibility? OfferEligibility { get; set; }

    /// <summary>
    /// The length of one discounted period.
    /// </summary>
    [JsonPropertyName("duration")]
    public SubscriptionOfferDuration? Duration { get; set; }

    /// <summary>
    /// How the customer pays for the offer: as a free trial, up front, or as they go.
    /// </summary>
    [JsonPropertyName("offerMode")]
    public SubscriptionOfferMode? OfferMode { get; set; }

    /// <summary>
    /// The number of discounted periods the offer runs for.
    /// </summary>
    [JsonPropertyName("numberOfPeriods")]
    public int? NumberOfPeriods { get; set; }

    /// <summary>
    /// The total number of codes the offer has generated, across both environments.
    /// </summary>
    [JsonPropertyName("totalNumberOfCodes")]
    public int? TotalNumberOfCodes { get; set; }

    /// <summary>
    /// The number of codes the offer has generated for the production environment.
    /// </summary>
    [JsonPropertyName("productionCodeCount")]
    public int? ProductionCodeCount { get; set; }

    /// <summary>
    /// The number of codes the offer has generated for the sandbox environment.
    /// </summary>
    [JsonPropertyName("sandboxCodeCount")]
    public int? SandboxCodeCount { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether customers can still redeem the offer's codes.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; set; }

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
