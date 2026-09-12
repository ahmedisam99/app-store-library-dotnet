using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationupdaterequest"/>
public sealed class InAppPurchaseLocalizationUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseLocalizationUpdateRequestData Data { get; set; }
}
