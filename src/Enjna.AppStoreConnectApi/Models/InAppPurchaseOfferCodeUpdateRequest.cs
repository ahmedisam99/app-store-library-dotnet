using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an In-App Purchase Offer Codes resource. An offer code is
/// otherwise immutable: only its active state changes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeUpdateRequestData Data { get; set; }
}
