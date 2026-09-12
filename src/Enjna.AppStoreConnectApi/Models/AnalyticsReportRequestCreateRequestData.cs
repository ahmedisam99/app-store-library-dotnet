using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates an Analytics Report Requests resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequestcreaterequest/data"/>
public sealed class AnalyticsReportRequestCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>analyticsReportRequests</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "analyticsReportRequests";

    /// <summary>
    /// The attributes that describe the request that creates an Analytics Report Requests resource.
    /// This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required AnalyticsReportRequestCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required AnalyticsReportRequestCreateRequestDataRelationships Relationships { get; set; }
}
