using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an In-App Purchase Offer Code Custom Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeCustomCodeUpdateRequestData Data { get; set; }
}
