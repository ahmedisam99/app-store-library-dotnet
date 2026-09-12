using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2updaterequest/data"/>
public sealed class InAppPurchaseV2UpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchases</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchases";

    /// <summary>
    /// The opaque resource ID of the in-app purchase to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseV2UpdateRequestDataAttributes? Attributes { get; set; }
}
