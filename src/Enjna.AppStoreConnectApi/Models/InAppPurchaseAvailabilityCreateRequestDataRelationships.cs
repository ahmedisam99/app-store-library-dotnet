using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that sets the territory availability of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseavailabilitycreaterequest/data/relationships"/>
public sealed class InAppPurchaseAvailabilityCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase whose availability you are setting. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchase")]
    public required RelationshipDeclaration InAppPurchase { get; set; }

    /// <summary>
    /// The territories to put the in-app purchase on sale in. This relationship is required, and it
    /// replaces the whole set rather than adding to it. Build it with
    /// <see cref="RelationshipDeclarationList.To(string, string[])"/> and the resource type
    /// <c>territories</c>.
    /// </summary>
    [JsonPropertyName("availableTerritories")]
    public required RelationshipDeclarationList AvailableTerritories { get; set; }
}
