using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The kind of finance report to download.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-financereports"/>
[JsonConverter(typeof(JsonEnumMemberConverter<FinanceReportType>))]
public enum FinanceReportType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The financial report for a fiscal period, which matches the payment you receive.
    /// </summary>
    [EnumMember(Value = "FINANCIAL")]
    Financial,

    /// <summary>
    /// The transaction-level detail behind the financial report.
    /// </summary>
    [EnumMember(Value = "FINANCE_DETAIL")]
    FinanceDetail
}
