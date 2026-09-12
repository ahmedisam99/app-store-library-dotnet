using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates a Promoted Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchaseupdaterequest/data"/>
public sealed class PromotedPurchaseUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>promotedPurchases</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "promotedPurchases";

    /// <summary>
    /// The opaque resource ID of the promotion to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public PromotedPurchaseUpdateRequestDataAttributes? Attributes { get; set; }
}
