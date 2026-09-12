namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an Analytics Report Requests resource. Creating one asks
/// Apple to generate analytics reports for an app; the reports themselves appear on its
/// <c>reports</c> relationship once they're ready.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequest"/>
public sealed class AnalyticsReportRequest : Resource<AnalyticsReportRequestAttributes>
{
}
