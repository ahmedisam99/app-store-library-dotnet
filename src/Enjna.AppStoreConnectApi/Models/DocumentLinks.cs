using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Self-links to documents that can contain information for one or more resources.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/documentlinks"/>
public sealed class DocumentLinks
{
    /// <summary>
    /// The link that produced the current response document.
    /// </summary>
    [JsonPropertyName("self")]
    public string Self { get; set; } = null!;
}
