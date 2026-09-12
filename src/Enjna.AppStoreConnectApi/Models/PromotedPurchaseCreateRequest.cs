using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to promote an in-app purchase or a subscription on your app's App
/// Store product page.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchasecreaterequest"/>
public sealed class PromotedPurchaseCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required PromotedPurchaseCreateRequestData Data { get; set; }
}
