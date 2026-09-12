using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Price Points resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricepoint/attributes"/>
public sealed class SubscriptionPricePointAttributes
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

    /// <summary>
    /// Your proceeds from the price once a subscriber completes one year of paid service and the
    /// lower commission rate applies.
    /// </summary>
    [JsonPropertyName("proceedsYear2")]
    public string? ProceedsYear2 { get; set; }
}
