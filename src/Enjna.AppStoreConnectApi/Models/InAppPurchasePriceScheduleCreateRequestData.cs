using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that sets the price schedule of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceschedulecreaterequest/data"/>
public sealed class InAppPurchasePriceScheduleCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchasePriceSchedules</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchasePriceSchedules";

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchasePriceScheduleCreateRequestDataRelationships Relationships { get; set; }
}
