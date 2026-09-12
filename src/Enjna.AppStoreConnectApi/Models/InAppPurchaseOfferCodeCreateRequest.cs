using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an In-App Purchase Offer Codes resource. Declare a price for
/// every territory the offer is available in: reference each price from the <c>prices</c>
/// relationship, and describe it in <see cref="Included"/> under the same temporary identifier.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeCreateRequestData Data { get; set; }

    /// <summary>
    /// The prices you create along with the offer code, each identified by a temporary identifier
    /// that the <c>prices</c> relationship refers to.
    /// </summary>
    [JsonPropertyName("included")]
    public InAppPurchaseOfferPriceInlineCreate[]? Included { get; set; }
}
