using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The level of detail of a sales and trends report. Which sub-types a report supports depends on
/// its <see cref="SalesReportType"/>.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-salesreports"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SalesReportSubType>))]
public enum SalesReportSubType
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// Aggregated totals.
    /// </summary>
    [EnumMember(Value = "SUMMARY")]
    Summary,

    /// <summary>
    /// Row-level detail rather than aggregated totals.
    /// </summary>
    [EnumMember(Value = "DETAILED")]
    Detailed,

    /// <summary>
    /// Totals broken out by install type.
    /// </summary>
    [EnumMember(Value = "SUMMARY_INSTALL_TYPE")]
    SummaryInstallType,

    /// <summary>
    /// Totals broken out by territory.
    /// </summary>
    [EnumMember(Value = "SUMMARY_TERRITORY")]
    SummaryTerritory,

    /// <summary>
    /// Totals broken out by distribution channel.
    /// </summary>
    [EnumMember(Value = "SUMMARY_CHANNEL")]
    SummaryChannel
}
