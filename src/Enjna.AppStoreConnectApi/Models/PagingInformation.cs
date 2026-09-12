using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Paging details such as the total number of resources and the per-page limit.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/paginginformation/paging-data.dictionary"/>
public sealed class PagingInformation
{
    /// <summary>
    /// The total number of resources matching your request.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    /// <summary>
    /// The maximum number of resources the response returns per page.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// The position the next page starts at. Pass it back as the <c>cursor</c> query parameter to
    /// read that page. The value is absent on the last page.
    /// </summary>
    [JsonPropertyName("nextCursor")]
    public string? NextCursor { get; set; }
}
