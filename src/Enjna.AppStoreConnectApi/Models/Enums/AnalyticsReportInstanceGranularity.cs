using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The length of the reporting period one instance of an analytics report covers.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportinstance/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<AnalyticsReportInstanceGranularity>))]
public enum AnalyticsReportInstanceGranularity
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The instance covers a single day.
    /// </summary>
    [EnumMember(Value = "DAILY")]
    Daily,

    /// <summary>
    /// The instance covers a single week.
    /// </summary>
    [EnumMember(Value = "WEEKLY")]
    Weekly,

    /// <summary>
    /// The instance covers a single month.
    /// </summary>
    [EnumMember(Value = "MONTHLY")]
    Monthly
}
