using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2createrequest/data/relationships"/>
public sealed class InAppPurchaseV2CreateRequestDataRelationships
{
    /// <summary>
    /// The app the in-app purchase belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }
}
