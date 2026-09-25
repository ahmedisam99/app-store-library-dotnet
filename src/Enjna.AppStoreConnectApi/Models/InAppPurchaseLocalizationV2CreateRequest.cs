using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a v2 In-App Purchase Localizations resource on an in-app
/// purchase version.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2createrequest"/>
public sealed class InAppPurchaseLocalizationV2CreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseLocalizationV2CreateRequestData Data { get; set; }
}
