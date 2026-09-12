using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an Analytics Report Instances resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportinstance/attributes"/>
public sealed class AnalyticsReportInstanceAttributes
{
    /// <summary>
    /// The length of the reporting period this instance covers.
    /// </summary>
    [JsonPropertyName("granularity")]
    public AnalyticsReportInstanceGranularity? Granularity { get; set; }

    /// <summary>
    /// The date of the data this instance holds.
    /// </summary>
    [JsonPropertyName("processingDate")]
    public DateOnly? ProcessingDate { get; set; }
}
