using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Group Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupversion/attributes"/>
public sealed class SubscriptionGroupVersionAttributes
{
    /// <summary>
    /// The number App Store Connect gives this version of the subscription group's metadata.
    /// </summary>
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    /// <summary>
    /// Where the version sits in the App Review workflow. You can add, change, or remove the
    /// version's localizations only while it is in
    /// <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionGroupVersionState? State { get; set; }
}
