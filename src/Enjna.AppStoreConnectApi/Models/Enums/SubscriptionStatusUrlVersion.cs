using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The version of the App Store Server Notifications that App Store Connect sends to your subscription status URL.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionstatusurlversion"/>
[JsonConverter(typeof(JsonEnumMemberConverter<SubscriptionStatusUrlVersion>))]
public enum SubscriptionStatusUrlVersion
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// Version 1 notifications.
    /// </summary>
    [EnumMember(Value = "V1")]
    V1,

    /// <summary>
    /// Version 2 notifications.
    /// </summary>
    [EnumMember(Value = "V2")]
    V2
}
