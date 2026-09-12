using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// Information that represents the customer's purchase of the app, cryptographically signed by the App Store.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction"/>
public sealed class AppTransaction
{
    /// <summary>
    /// The server environment that signs the app transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3963901-environment"/>
    [JsonPropertyName("receiptType")]
    public AppStoreEnvironment? ReceiptType { get; set; }

    /// <summary>
    /// The unique identifier the App Store uses to identify the app.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954436-appid"/>
    [JsonPropertyName("appAppleId")]
    public long? AppAppleId { get; set; }

    /// <summary>
    /// The bundle identifier that the app transaction applies to.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954439-bundleid"/>
    [JsonPropertyName("bundleId")]
    public string? BundleId { get; set; }

    /// <summary>
    /// The app version that the app transaction applies to.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954437-appversion"/>
    [JsonPropertyName("applicationVersion")]
    public string? ApplicationVersion { get; set; }

    /// <summary>
    /// The version external identifier of the app.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954438-appversionid"/>
    [JsonPropertyName("versionExternalIdentifier")]
    public long? VersionExternalIdentifier { get; set; }

    /// <summary>
    /// The date that the App Store signed the JWS app transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954449-signeddate"/>
    [JsonPropertyName("receiptCreationDate")]
    public long? ReceiptCreationDate { get; set; }

    /// <summary>
    /// The UTC date and time that the App Store signed the JWS app transaction,
    /// derived from <see cref="ReceiptCreationDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? ReceiptCreationDateUtc => ReceiptCreationDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(ReceiptCreationDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The date the user originally purchased the app from the App Store.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954448-originalpurchasedate"/>
    [JsonPropertyName("originalPurchaseDate")]
    public long? OriginalPurchaseDate { get; set; }

    /// <summary>
    /// The UTC date and time when the user originally purchased the app from the App Store,
    /// derived from <see cref="OriginalPurchaseDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? OriginalPurchaseDateUtc => OriginalPurchaseDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(OriginalPurchaseDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The app version that the user originally purchased from the App Store.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954447-originalappversion"/>
    [JsonPropertyName("originalApplicationVersion")]
    public string? OriginalApplicationVersion { get; set; }

    /// <summary>
    /// The Base64 device verification value to use to verify whether the app transaction belongs to the device.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954441-deviceverification"/>
    [JsonPropertyName("deviceVerification")]
    public string? DeviceVerification { get; set; }

    /// <summary>
    /// The UUID used to compute the device verification value.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/3954442-deviceverificationnonce"/>
    [JsonPropertyName("deviceVerificationNonce")]
    public string? DeviceVerificationNonce { get; set; }

    /// <summary>
    /// The date the customer placed an order for the app before it's available in the App Store.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/4013175-preorderdate"/>
    [JsonPropertyName("preorderDate")]
    public long? PreorderDate { get; set; }

    /// <summary>
    /// The UTC date and time when the customer placed an order for the app before it's available in the App Store,
    /// derived from <see cref="PreorderDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? PreorderDateUtc => PreorderDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(PreorderDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The unique identifier of the app download transaction.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/apptransactionid"/>
    [JsonPropertyName("appTransactionId")]
    public string? AppTransactionId { get; set; }

    /// <summary>
    /// The platform on which the customer originally purchased the app.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/storekit/apptransaction/originalplatform-4mogz"/>
    [JsonPropertyName("originalPlatform")]
    public PurchasePlatform? OriginalPlatform { get; set; }
}
