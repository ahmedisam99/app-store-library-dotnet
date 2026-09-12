using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCreateRequestDataRelationships
{
    /// <summary>
    /// The In-App Purchases resource the offer code discounts. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchase")]
    public required RelationshipDeclaration InAppPurchase { get; set; }

    /// <summary>
    /// The prices of the offer code, one per territory. This relationship is required, and its
    /// identifiers are the temporary ones you give the entries of the request's <c>included</c>
    /// array.
    /// </summary>
    [JsonPropertyName("prices")]
    public required RelationshipDeclarationList Prices { get; set; }
}
