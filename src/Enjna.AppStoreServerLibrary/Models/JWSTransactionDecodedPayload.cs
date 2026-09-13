using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Type = Enjna.AppStoreServerLibrary.Models.Enums.Type;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// A decoded payload containing transaction information.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/jwstransactiondecodedpayload"/>
public sealed class JWSTransactionDecodedPayload : DecodedSignedData
{
    /// <summary>
    /// The original transaction identifier of a purchase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/originaltransactionid"/>
    [JsonPropertyName("originalTransactionId")]
    public string? OriginalTransactionId { get; set; }

    /// <summary>
    /// The subscription's original transaction identifier, before you migrated the subscription.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/previousoriginaltransactionid"/>
    [JsonPropertyName("previousOriginalTransactionId")]
    public string? PreviousOriginalTransactionId { get; set; }

    /// <summary>
    /// The unique identifier for a transaction such as an in-app purchase, restored in-app purchase, or subscription renewal.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/transactionid"/>
    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// The unique identifier of subscription-purchase events across devices, including renewals.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/weborderlineitemid"/>
    [JsonPropertyName("webOrderLineItemId")]
    public string? WebOrderLineItemId { get; set; }

    /// <summary>
    /// The bundle identifier of an app.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/bundleid"/>
    [JsonPropertyName("bundleId")]
    public string? BundleId { get; set; }

    /// <summary>
    /// The unique identifier for the product, that you create in App Store Connect.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/productid"/>
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    /// <summary>
    /// The identifier of the subscription group that the subscription belongs to.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/subscriptiongroupidentifier"/>
    [JsonPropertyName("subscriptionGroupIdentifier")]
    public string? SubscriptionGroupIdentifier { get; set; }

    /// <summary>
    /// The time that the App Store charged the user's account for an in-app purchase, a restored in-app purchase, a subscription, or a subscription renewal after a lapse.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/purchasedate"/>
    [JsonPropertyName("purchaseDate")]
    public long? PurchaseDate { get; set; }

    /// <summary>
    /// The UTC date and time that the App Store charged the user's account,
    /// derived from <see cref="PurchaseDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? PurchaseDateUtc => PurchaseDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(PurchaseDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The purchase date of the transaction associated with the original transaction identifier.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/originalpurchasedate"/>
    [JsonPropertyName("originalPurchaseDate")]
    public long? OriginalPurchaseDate { get; set; }

    /// <summary>
    /// The UTC date and time of the purchase associated with the original transaction identifier,
    /// derived from <see cref="OriginalPurchaseDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? OriginalPurchaseDateUtc => OriginalPurchaseDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(OriginalPurchaseDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The UNIX time, in milliseconds, an auto-renewable subscription expires or renews.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/expiresdate"/>
    [JsonPropertyName("expiresDate")]
    public long? ExpiresDate { get; set; }

    /// <summary>
    /// The UTC date and time when an auto-renewable subscription expires or renews,
    /// derived from <see cref="ExpiresDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? ExpiresDateUtc => ExpiresDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(ExpiresDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The number of consumable products purchased.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/quantity"/>
    [JsonPropertyName("quantity")]
    public int? Quantity { get; set; }

    /// <summary>
    /// The type of the in-app purchase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/type"/>
    [JsonPropertyName("type")]
    public Type? Type { get; set; }

    /// <summary>
    /// The UUID that an app optionally generates to map a customer's in-app purchase with its resulting App Store transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/appaccounttoken"/>
    [JsonPropertyName("appAccountToken")]
    public string? AppAccountToken { get; set; }

    /// <summary>
    /// A string that describes whether the transaction was purchased by the user, or is available to them through Family Sharing.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/inappownershiptype"/>
    [JsonPropertyName("inAppOwnershipType")]
    public InAppOwnershipType? InAppOwnershipType { get; set; }

    /// <summary>
    /// The reason that the App Store refunded the transaction or revoked it from Family Sharing.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/revocationreason"/>
    [JsonPropertyName("revocationReason")]
    public RevocationReason? RevocationReason { get; set; }

    /// <summary>
    /// The UNIX time, in milliseconds, that Apple Support refunded a transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/revocationdate"/>
    [JsonPropertyName("revocationDate")]
    public long? RevocationDate { get; set; }

    /// <summary>
    /// The UTC date and time that Apple Support refunded a transaction,
    /// derived from <see cref="RevocationDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? RevocationDateUtc => RevocationDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(RevocationDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The Boolean value that indicates whether the user upgraded to another subscription.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/isupgraded"/>
    [JsonPropertyName("isUpgraded")]
    public bool? IsUpgraded { get; set; }

    /// <summary>
    /// A value that represents the promotional offer type.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offertype"/>
    [JsonPropertyName("offerType")]
    public OfferType? OfferType { get; set; }

    /// <summary>
    /// The identifier that contains the offer code or the promotional offer identifier.
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
    /// The three-letter code that represents the country or region associated with the App Store storefront for the purchase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/storefront"/>
    [JsonPropertyName("storefront")]
    public string? Storefront { get; set; }

    /// <summary>
    /// An Apple-defined value that uniquely identifies the App Store storefront associated with the purchase.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/storefrontid"/>
    [JsonPropertyName("storefrontId")]
    public string? StorefrontId { get; set; }

    /// <summary>
    /// The reason for the purchase transaction, which indicates whether it's a customer's purchase or a renewal for an auto-renewable subscription that the system initiates.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/transactionreason"/>
    [JsonPropertyName("transactionReason")]
    public TransactionReason? TransactionReason { get; set; }

    /// <summary>
    /// The three-letter ISO 4217 currency code for the price of the product.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/currency"/>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// The price, in milliunits, of the in-app purchase or subscription offer that you configured in App Store Connect.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/price"/>
    [JsonPropertyName("price")]
    public long? Price { get; set; }

    /// <summary>
    /// The payment mode you configure for the offer.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/offerdiscounttype"/>
    [JsonPropertyName("offerDiscountType")]
    public OfferDiscountType? OfferDiscountType { get; set; }

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
    /// The type of the refund or revocation that applies to the transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/revocationtype"/>
    [JsonPropertyName("revocationType")]
    public RevocationType? RevocationType { get; set; }

    /// <summary>
    /// The percentage, in milliunits, of the transaction that the App Store has refunded or revoked.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/revocationpercentage"/>
    [JsonPropertyName("revocationPercentage")]
    public long? RevocationPercentage { get; set; }

    /// <summary>
    /// Transaction information that is present only for Advanced Commerce SKUs.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/advancedcommercetransactioninfo"/>
    [JsonPropertyName("advancedCommerceInfo")]
    public AdvancedCommerceTransactionInfo? AdvancedCommerceInfo { get; set; }

    /// <summary>
    /// The billing plan type for the transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/billingplantype"/>
    [JsonPropertyName("billingPlanType")]
    public BillingPlanType? BillingPlanType { get; set; }

    /// <summary>
    /// Commitment information for the transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/transactioncommitmentinfo"/>
    [JsonPropertyName("commitmentInfo")]
    public TransactionCommitmentInfo? CommitmentInfo { get; set; }
}
