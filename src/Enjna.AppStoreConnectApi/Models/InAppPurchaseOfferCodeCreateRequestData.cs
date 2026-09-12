using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodes";

    /// <summary>
    /// The attributes that describe the request that creates an In-App Purchase Offer Codes
    /// resource. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseOfferCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseOfferCodeCreateRequestDataRelationships Relationships { get; set; }
}
