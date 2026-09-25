using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to commit a v2 In-App Purchase Images resource once you finish
/// uploading the image file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2updaterequest"/>
public sealed class InAppPurchaseImageV2UpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseImageV2UpdateRequestData Data { get; set; }
}
