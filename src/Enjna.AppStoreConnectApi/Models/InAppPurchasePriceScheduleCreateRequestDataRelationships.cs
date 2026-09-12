using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that sets the price schedule of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceschedulecreaterequest/data/relationships"/>
public sealed class InAppPurchasePriceScheduleCreateRequestDataRelationships
{
    /// <summary>
    /// The in-app purchase whose price schedule you are setting. This relationship is required.
    /// </summary>
    [JsonPropertyName("inAppPurchase")]
    public required RelationshipDeclaration InAppPurchase { get; set; }

    /// <summary>
    /// The territory whose price the App Store converts into every other territory's price. This
    /// relationship is required, and it can point at a <see cref="TerritoryInlineCreate"/> you send
    /// in the request's <c>included</c> array.
    /// </summary>
    [JsonPropertyName("baseTerritory")]
    public required RelationshipDeclaration BaseTerritory { get; set; }

    /// <summary>
    /// The prices you set by hand, each pointing at an <see cref="InAppPurchasePriceInlineCreate"/>
    /// in the request's <c>included</c> array. This relationship is required. Build it with
    /// <see cref="RelationshipDeclarationList.To(string, string[])"/> and the resource type
    /// <c>inAppPurchasePrices</c>.
    /// </summary>
    [JsonPropertyName("manualPrices")]
    public required RelationshipDeclarationList ManualPrices { get; set; }
}
