using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a price you create inline with an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecreaterequest"/>
public sealed class InAppPurchaseOfferPriceInlineCreateRelationships
{
    /// <summary>
    /// The Territories resource the price applies in.
    /// </summary>
    [JsonPropertyName("territory")]
    public RelationshipDeclaration? Territory { get; set; }

    /// <summary>
    /// The In-App Purchase Price Points resource that sets the discounted price in that territory.
    /// </summary>
    [JsonPropertyName("pricePoint")]
    public RelationshipDeclaration? PricePoint { get; set; }
}
