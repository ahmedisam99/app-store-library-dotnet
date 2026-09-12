using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The subject area an analytics report belongs to.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreport/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<AnalyticsReportCategory>))]
public enum AnalyticsReportCategory
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// How people use your app, such as sessions, installs, deletions, and crashes.
    /// </summary>
    [EnumMember(Value = "APP_USAGE")]
    AppUsage,

    /// <summary>
    /// How people discover and engage with your app on the App Store.
    /// </summary>
    [EnumMember(Value = "APP_STORE_ENGAGEMENT")]
    AppStoreEngagement,

    /// <summary>
    /// Purchases, proceeds, and subscription activity.
    /// </summary>
    [EnumMember(Value = "COMMERCE")]
    Commerce,

    /// <summary>
    /// How your app uses system frameworks and the capabilities they expose.
    /// </summary>
    [EnumMember(Value = "FRAMEWORK_USAGE")]
    FrameworkUsage,

    /// <summary>
    /// How your app performs on customer devices, such as launch times and hangs.
    /// </summary>
    [EnumMember(Value = "PERFORMANCE")]
    Performance
}
