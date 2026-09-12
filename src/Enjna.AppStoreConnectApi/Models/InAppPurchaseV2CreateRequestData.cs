using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2createrequest/data"/>
public sealed class InAppPurchaseV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchases</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchases";

    /// <summary>
    /// The attributes that describe the in-app purchase to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseV2CreateRequestDataRelationships Relationships { get; set; }
}
