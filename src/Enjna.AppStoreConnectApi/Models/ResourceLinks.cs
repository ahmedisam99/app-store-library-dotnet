using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Self-links to requested resources.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/resourcelinks"/>
public sealed class ResourceLinks
{
    /// <summary>
    /// The link that produced the current response document.
    /// </summary>
    [JsonPropertyName("self")]
    public string? Self { get; set; }
}
