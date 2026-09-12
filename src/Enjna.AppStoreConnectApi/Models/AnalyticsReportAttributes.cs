using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an Analytics Reports resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreport/attributes"/>
public sealed class AnalyticsReportAttributes
{
    /// <summary>
    /// The name of the report, such as <c>App Store Installation and Deletion Standard</c>.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The subject area the report belongs to.
    /// </summary>
    [JsonPropertyName("category")]
    public AnalyticsReportCategory? Category { get; set; }
}
