using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Links related to the response document, including paging links.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/pageddocumentlinks"/>
public sealed class PagedDocumentLinks
{
    /// <summary>
    /// The link that produced the current response document.
    /// </summary>
    [JsonPropertyName("self")]
    public string Self { get; set; } = null!;

    /// <summary>
    /// The link to the first page of documents.
    /// </summary>
    [JsonPropertyName("first")]
    public string? First { get; set; }

    /// <summary>
    /// The link to the next page of documents. Paging is forward-only; this link is absent on the last page.
    /// </summary>
    [JsonPropertyName("next")]
    public string? Next { get; set; }
}
