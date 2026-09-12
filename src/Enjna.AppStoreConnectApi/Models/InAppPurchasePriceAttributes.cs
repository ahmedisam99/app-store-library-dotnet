using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Prices resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseprice/attributes"/>
public sealed class InAppPurchasePriceAttributes
{
    /// <summary>
    /// The date the price takes effect. A price with no start date is the one the schedule starts
    /// from, and it applies until another price replaces it.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The last date the price applies on. A price with no end date stays in effect until the next
    /// scheduled price change.
    /// </summary>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether you set this price for the territory yourself, rather
    /// than the App Store deriving it from the price in the base territory.
    /// </summary>
    [JsonPropertyName("manual")]
    public bool? Manual { get; set; }
}
