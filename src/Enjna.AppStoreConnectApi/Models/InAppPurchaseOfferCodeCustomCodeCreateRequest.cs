using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an In-App Purchase Offer Code Custom Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeCustomCodeCreateRequestData Data { get; set; }
}
