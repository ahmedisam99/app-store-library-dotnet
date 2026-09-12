using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Paging information for data responses.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/paginginformation"/>
public sealed class PagingMeta
{
    /// <summary>
    /// The paging details, such as the total number of resources and the per-page limit.
    /// </summary>
    [JsonPropertyName("paging")]
    public PagingInformation Paging { get; set; } = null!;
}
