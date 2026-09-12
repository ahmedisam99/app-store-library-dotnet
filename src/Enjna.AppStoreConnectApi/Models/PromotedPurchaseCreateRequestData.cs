using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a Promoted Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchasecreaterequest/data"/>
public sealed class PromotedPurchaseCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>promotedPurchases</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "promotedPurchases";

    /// <summary>
    /// The attributes that describe the promotion to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required PromotedPurchaseCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required PromotedPurchaseCreateRequestDataRelationships Relationships { get; set; }
}
