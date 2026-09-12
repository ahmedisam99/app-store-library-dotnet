using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an In-App Purchase Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeCreateRequestDataAttributes
{
    /// <summary>
    /// The alphanumeric code customers enter to redeem the offer. It is required, and must be
    /// unique across your offer codes.
    /// </summary>
    [JsonPropertyName("customCode")]
    public required string CustomCode { get; set; }

    /// <summary>
    /// The maximum number of times customers can redeem the code. It is required.
    /// </summary>
    [JsonPropertyName("numberOfCodes")]
    public required int NumberOfCodes { get; set; }

    /// <summary>
    /// The last date customers can redeem the code. Leave it unset for a code that doesn't expire.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public DateOnly? ExpirationDate { get; set; }
}
