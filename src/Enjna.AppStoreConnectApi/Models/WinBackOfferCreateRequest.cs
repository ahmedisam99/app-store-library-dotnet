using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a win-back offer, together with the prices it charges in each
/// territory.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffercreaterequest"/>
public sealed class WinBackOfferCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required WinBackOfferCreateRequestData Data { get; set; }

    /// <summary>
    /// The prices the <c>prices</c> relationship of this request points at. Create the offer and
    /// its prices in one request by listing them here.
    /// </summary>
    [JsonPropertyName("included")]
    public WinBackOfferPriceInlineCreate[]? Included { get; set; }
}
