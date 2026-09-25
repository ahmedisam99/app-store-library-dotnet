using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchase Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseversioncreaterequest/data"/>
public sealed class InAppPurchaseVersionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseVersions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseVersions";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseVersionCreateRequestDataRelationships Relationships { get; set; }
}
