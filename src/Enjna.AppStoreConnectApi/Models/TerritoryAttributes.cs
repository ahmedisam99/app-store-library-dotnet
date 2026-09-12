using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Territories resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/territory/attributes"/>
public sealed class TerritoryAttributes
{
    /// <summary>
    /// The ISO 4217 code of the currency the App Store charges customers in this territory, such as <c>USD</c>.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}
