using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Offer Code One-Time Use
/// Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeCreateRequestDataRelationships
{
    /// <summary>
    /// The In-App Purchase Offer Codes resource the batch of codes belongs to. This relationship is
    /// required.
    /// </summary>
    [JsonPropertyName("offerCode")]
    public required RelationshipDeclaration OfferCode { get; set; }
}
