using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// A decoded payload containing subscription renewal information for an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/jwsrenewalinfodecodedpayload"/>
public sealed class JWSRenewalInfoDecodedPayload : DecodedSignedData
{
    /// <summary>
    /// The reason the subscription expired.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/expirationintent"/>
    [JsonPropertyName("expirationIntent")]
    public ExpirationIntent? ExpirationIntent { get; set; }

    /// <summary>
    /// The original transaction identifier of a purchase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/originaltransactionid"/>
    [JsonPropertyName("originalTransactionId")]
    public string? OriginalTransactionId { get; set; }

    /// <summary>
    /// The product identifier of the product that will renew at the next billing period.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/autorenewproductid"/>
    [JsonPropertyName("autoRenewProductId")]
    public string? AutoRenewProductId { get; set; }

    /// <summary>
    /// The unique identifier for the product, that you create in App Store Connect.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/productid"/>
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    /// <summary>
    /// The renewal status of the auto-renewable subscription.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/autorenewstatus"/>
    [JsonPropertyName("autoRenewStatus")]
    public AutoRenewStatus? AutoRenewStatus { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the App Store is attempting to automatically renew an expired subscription.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/isinbillingretryperiod"/>
    [JsonPropertyName("isInBillingRetryPeriod")]
    public bool? IsInBillingRetryPeriod { get; set; }

    /// <summary>
    /// The status that indicates whether the auto-renewable subscription is subject to a price increase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/priceincreasestatus"/>
    [JsonPropertyName("priceIncreaseStatus")]
    public PriceIncreaseStatus? PriceIncreaseStatus { get; set; }

    /// <summary>
    /// The time when the billing grace period for subscription renewals expires.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/graceperiodexpiresdate"/>
    [JsonPropertyName("gracePeriodExpiresDate")]
    public long? GracePeriodExpiresDate { get; set; }

    /// <summary>
    /// The UTC date and time when the billing grace period for subscription renewals expires,
    /// derived from <see cref="GracePeriodExpiresDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? GracePeriodExpiresDateUtc => GracePeriodExpiresDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(GracePeriodExpiresDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The type of subscription offer.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offertype"/>
    [JsonPropertyName("offerType")]
    public OfferType? OfferType { get; set; }

    /// <summary>
    /// The offer code or the promotional offer identifier.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offeridentifier"/>
    [JsonPropertyName("offerIdentifier")]
    public string? OfferIdentifier { get; set; }

    /// <summary>
    /// The server environment, either sandbox or production.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/environment"/>
    [JsonPropertyName("environment")]
    public AppStoreEnvironment? Environment { get; set; }

    /// <summary>
    /// The earliest start date of a subscription in a series of auto-renewable subscription purchases that ignores all lapses of paid service shorter than 60 days.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/recentsubscriptionstartdate"/>
    [JsonPropertyName("recentSubscriptionStartDate")]
    public long? RecentSubscriptionStartDate { get; set; }

    /// <summary>
    /// The UTC date and time of the earliest start date of a subscription in a series of auto-renewable subscription purchases,
    /// derived from <see cref="RecentSubscriptionStartDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? RecentSubscriptionStartDateUtc => RecentSubscriptionStartDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(RecentSubscriptionStartDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The UNIX time, in milliseconds, when the most recent auto-renewable subscription purchase expires.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/renewaldate"/>
    [JsonPropertyName("renewalDate")]
    public long? RenewalDate { get; set; }

    /// <summary>
    /// The UTC date and time when the most recent auto-renewable subscription purchase expires,
    /// derived from <see cref="RenewalDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? RenewalDateUtc => RenewalDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(RenewalDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The currency code for the renewalPrice of the subscription.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/currency"/>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// The renewal price, in milliunits, of the auto-renewable subscription that renews at the next billing period.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/renewalprice"/>
    [JsonPropertyName("renewalPrice")]
    public long? RenewalPrice { get; set; }

    /// <summary>
    /// The payment mode you configure for the offer.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offerdiscounttype"/>
    [JsonPropertyName("offerDiscountType")]
    public OfferDiscountType? OfferDiscountType { get; set; }

    /// <summary>
    /// An array of win-back offer identifiers that a customer is eligible to redeem, which sorts the identifiers to present the better offers first.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/eligiblewinbackofferids"/>
    [JsonPropertyName("eligibleWinBackOfferIds")]
    public string[]? EligibleWinBackOfferIds { get; set; }

    /// <summary>
    /// The UUID that an app optionally generates to map a customer's in-app purchase with its resulting App Store transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/appaccounttoken"/>
    [JsonPropertyName("appAccountToken")]
    public string? AppAccountToken { get; set; }

    /// <summary>
    /// The unique identifier of the app download transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/appTransactionId"/>
    [JsonPropertyName("appTransactionId")]
    public string? AppTransactionId { get; set; }

    /// <summary>
    /// The duration of the offer.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offerPeriod"/>
    [JsonPropertyName("offerPeriod")]
    public string? OfferPeriod { get; set; }

    /// <summary>
    /// Renewal information that is present only for Advanced Commerce SKUs.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/advancedcommercerenewalinfo"/>
    [JsonPropertyName("advancedCommerceInfo")]
    public AdvancedCommerceRenewalInfo? AdvancedCommerceInfo { get; set; }

    /// <summary>
    /// Commitment information that applies at the next renewal.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/renewalcommitmentinfo"/>
    [JsonPropertyName("commitmentInfo")]
    public RenewalCommitmentInfo? CommitmentInfo { get; set; }

    /// <summary>
    /// The billing plan type that applies at the next renewal.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/renewalbillingplantype"/>
    [JsonPropertyName("renewalBillingPlanType")]
    public RenewalBillingPlanType? RenewalBillingPlanType { get; set; }
}
