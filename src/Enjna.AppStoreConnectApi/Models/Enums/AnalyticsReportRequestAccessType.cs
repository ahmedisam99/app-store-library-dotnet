using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// How long an analytics report request keeps producing reports.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequest/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<AnalyticsReportRequestAccessType>))]
public enum AnalyticsReportRequestAccessType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// A one-off request for the data that already exists, covering up to the past year.
    /// </summary>
    [EnumMember(Value = "ONE_TIME_SNAPSHOT")]
    OneTimeSnapshot,

    /// <summary>
    /// A standing request that keeps producing new report instances as Apple generates the data.
    /// </summary>
    [EnumMember(Value = "ONGOING")]
    Ongoing
}
