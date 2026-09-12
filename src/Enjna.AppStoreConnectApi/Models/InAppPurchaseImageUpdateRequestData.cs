using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that commits an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimageupdaterequest/data"/>
public sealed class InAppPurchaseImageUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseImages";

    /// <summary>
    /// The opaque resource ID of the image to commit. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseImageUpdateRequestDataAttributes? Attributes { get; set; }
}
