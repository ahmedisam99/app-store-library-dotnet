using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A win-back offer price sent in the <c>included</c> array of a request, so that the <c>prices</c>
/// relationship of the same request body can point at it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackofferpriceinlinecreate"/>
public sealed class WinBackOfferPriceInlineCreate
{
    /// <summary>
    /// The resource type. The value is always <c>winBackOfferPrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "winBackOfferPrices";

    /// <summary>
    /// The ID that the <c>prices</c> relationship of the request uses to refer to this price.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
