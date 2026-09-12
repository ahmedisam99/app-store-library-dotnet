using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to set the territory availability of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseavailabilitycreaterequest"/>
public sealed class InAppPurchaseAvailabilityCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchaseAvailabilityCreateRequestData Data { get; set; }
}
