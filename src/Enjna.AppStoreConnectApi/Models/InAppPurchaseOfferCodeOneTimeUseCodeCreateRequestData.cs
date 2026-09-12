using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an In-App Purchase Offer Code One-Time Use
/// Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseOfferCodeOneTimeUseCodes</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseOfferCodeOneTimeUseCodes";

    /// <summary>
    /// The attributes that describe the request that creates an In-App Purchase Offer Code One-Time
    /// Use Codes resource. They are required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestDataRelationships Relationships { get; set; }
}
