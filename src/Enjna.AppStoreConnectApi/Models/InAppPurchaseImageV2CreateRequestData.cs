using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that reserves a v2 In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2createrequest/data"/>
public sealed class InAppPurchaseImageV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseImages";

    /// <summary>
    /// The attributes that describe the image to reserve. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseImageV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseImageV2CreateRequestDataRelationships Relationships { get; set; }
}
