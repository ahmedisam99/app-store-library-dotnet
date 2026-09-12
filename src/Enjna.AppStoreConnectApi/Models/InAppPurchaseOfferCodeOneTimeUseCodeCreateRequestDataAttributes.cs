using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an In-App Purchase Offer Code One-Time Use Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestDataAttributes
{
    /// <summary>
    /// The number of unique codes to generate. It is required.
    /// </summary>
    [JsonPropertyName("numberOfCodes")]
    public required int NumberOfCodes { get; set; }

    /// <summary>
    /// The last date customers can redeem a code from the batch. It is required.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public required DateOnly ExpirationDate { get; set; }

    /// <summary>
    /// The environment the codes are redeemable in. Leave it unset to generate production codes.
    /// </summary>
    [JsonPropertyName("environment")]
    public OfferCodeEnvironment? Environment { get; set; }
}
