using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Price Points resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepricepoint/attributes"/>
public sealed class InAppPurchasePricePointAttributes
{
    /// <summary>
    /// The price the customer pays, in the currency of the price point's territory.
    /// </summary>
    [JsonPropertyName("customerPrice")]
    public string? CustomerPrice { get; set; }

    /// <summary>
    /// Your proceeds from the price, after the App Store's commission and applicable taxes.
    /// </summary>
    [JsonPropertyName("proceeds")]
    public string? Proceeds { get; set; }
}
