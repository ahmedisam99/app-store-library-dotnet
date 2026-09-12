using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that sets the territory availability of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseavailabilitycreaterequest/data"/>
public sealed class InAppPurchaseAvailabilityCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseAvailabilities</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseAvailabilities";

    /// <summary>
    /// The attributes that describe the availability to set. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseAvailabilityCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseAvailabilityCreateRequestDataRelationships Relationships { get; set; }
}
