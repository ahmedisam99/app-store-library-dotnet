using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an In-App Purchase Offer Code One-Time Use Codes resource,
/// which generates a batch of unique codes that each customer redeems once.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestData Data { get; set; }
}
