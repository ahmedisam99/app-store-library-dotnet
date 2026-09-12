using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A territory you name in the <c>included</c> array of a request, so a relationship elsewhere in
/// the same request can point at it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/territoryinlinecreate"/>
public sealed class TerritoryInlineCreate : IInAppPurchasePriceScheduleCreateRequestIncludedResource
{
    /// <summary>
    /// The resource type. The value is always <c>territories</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "territories";

    /// <summary>
    /// The territory's resource ID, which is its three-letter code, such as <c>USA</c>.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
