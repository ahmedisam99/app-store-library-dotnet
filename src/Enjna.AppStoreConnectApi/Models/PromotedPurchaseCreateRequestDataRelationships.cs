using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Promoted Purchases resource. Set exactly
/// one of the in-app purchase and the subscription, to say what the app promotes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/promotedpurchasecreaterequest/data/relationships"/>
public sealed class PromotedPurchaseCreateRequestDataRelationships
{
    /// <summary>
    /// The app whose product page carries the promotion. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }

    /// <summary>
    /// The in-app purchase to promote.
    /// </summary>
    [JsonPropertyName("inAppPurchaseV2")]
    public RelationshipDeclaration? InAppPurchaseV2 { get; set; }

    /// <summary>
    /// The subscription to promote.
    /// </summary>
    [JsonPropertyName("subscription")]
    public RelationshipDeclaration? Subscription { get; set; }
}
