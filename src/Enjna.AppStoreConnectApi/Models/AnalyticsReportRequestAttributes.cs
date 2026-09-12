using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an Analytics Report Requests resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequest/attributes"/>
public sealed class AnalyticsReportRequestAttributes
{
    /// <summary>
    /// How long the request keeps producing reports.
    /// </summary>
    [JsonPropertyName("accessType")]
    public AnalyticsReportRequestAccessType? AccessType { get; set; }

    /// <summary>
    /// Whether Apple stopped generating new reports for an ongoing request because nobody
    /// downloaded them for an extended period. Create the request again to resume it.
    /// </summary>
    [JsonPropertyName("stoppedDueToInactivity")]
    public bool? StoppedDueToInactivity { get; set; }
}
