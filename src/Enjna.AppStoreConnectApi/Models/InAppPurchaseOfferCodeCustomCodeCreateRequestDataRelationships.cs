using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchase Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeCreateRequestDataRelationships
{
    /// <summary>
    /// The In-App Purchase Offer Codes resource the custom code belongs to. This relationship is
    /// required.
    /// </summary>
    [JsonPropertyName("offerCode")]
    public required RelationshipDeclaration OfferCode { get; set; }
}
