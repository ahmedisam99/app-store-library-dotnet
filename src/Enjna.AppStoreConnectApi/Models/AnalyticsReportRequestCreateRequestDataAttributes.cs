using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an Analytics Report Requests resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequestcreaterequest/data/attributes"/>
public sealed class AnalyticsReportRequestCreateRequestDataAttributes
{
    /// <summary>
    /// How long the request keeps producing reports: a single snapshot of the data that already
    /// exists, or an ongoing request that keeps producing new instances. This attribute is required.
    /// </summary>
    [JsonPropertyName("accessType")]
    public required AnalyticsReportRequestAccessType AccessType { get; set; }
}
