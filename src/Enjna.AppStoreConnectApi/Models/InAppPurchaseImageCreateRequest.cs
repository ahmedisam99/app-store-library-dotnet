using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve an In-App Purchase Images resource before you upload the
/// image file.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagecreaterequest"/>
public sealed class InAppPurchaseImageCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseImageCreateRequestData Data { get; set; }
}
