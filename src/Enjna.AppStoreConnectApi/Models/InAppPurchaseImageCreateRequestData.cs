using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that reserves an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagecreaterequest/data"/>
public sealed class InAppPurchaseImageCreateRequestData
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
    public required InAppPurchaseImageCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseImageCreateRequestDataRelationships Relationships { get; set; }
}
