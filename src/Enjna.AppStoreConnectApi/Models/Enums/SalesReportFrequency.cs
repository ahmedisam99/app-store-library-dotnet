using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The reporting period a sales and trends report covers. The frequency also decides how you write
/// the report date: a day, the last day of a week, a month, or a year.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-salesreports"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SalesReportFrequency>))]
public enum SalesReportFrequency
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// One day, with a report date of <c>YYYY-MM-DD</c>.
    /// </summary>
    [EnumMember(Value = "DAILY")]
    Daily,

    /// <summary>
    /// One week, with a report date of <c>YYYY-MM-DD</c> that names the last day of the week.
    /// </summary>
    [EnumMember(Value = "WEEKLY")]
    Weekly,

    /// <summary>
    /// One month, with a report date of <c>YYYY-MM</c>.
    /// </summary>
    [EnumMember(Value = "MONTHLY")]
    Monthly,

    /// <summary>
    /// One year, with a report date of <c>YYYY</c>.
    /// </summary>
    [EnumMember(Value = "YEARLY")]
    Yearly
}
