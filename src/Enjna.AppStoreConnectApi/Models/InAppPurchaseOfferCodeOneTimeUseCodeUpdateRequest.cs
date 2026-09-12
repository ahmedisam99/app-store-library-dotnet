using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an In-App Purchase Offer Code One-Time Use Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequestData Data { get; set; }
}
