using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a price you create inline in a price schedule request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceinlinecreate/relationships"/>
public sealed class InAppPurchasePriceInlineCreateRelationships
{
    /// <summary>
    /// The price point the price charges, which fixes both the amount and the territory. Use the
    /// resource type <c>inAppPurchasePricePoints</c>.
    /// </summary>
    [JsonPropertyName("inAppPurchasePricePoint")]
    public RelationshipDeclaration? InAppPurchasePricePoint { get; set; }

    /// <summary>
    /// The in-app purchase the price belongs to. Use the resource type <c>inAppPurchases</c>.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public RelationshipDeclaration? InAppPurchaseV2 { get; set; }
}
