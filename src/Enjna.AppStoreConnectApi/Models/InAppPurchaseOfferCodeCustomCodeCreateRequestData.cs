using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchase Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodeCustomCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodeCustomCodes";

    /// <summary>
    /// The attributes that describe the request that creates an In-App Purchase Offer Code Custom
    /// Codes resource. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseOfferCodeCustomCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseOfferCodeCustomCodeCreateRequestDataRelationships Relationships { get; set; }
}
