using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionversion/attributes"/>
public sealed class SubscriptionVersionAttributes
{
    /// <summary>
    /// The number of this version.
    /// </summary>
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    /// <summary>
    /// Where the version sits in the App Review workflow.
    /// </summary>
    [JsonPropertyName("state")]
    public SubscriptionVersionState? State { get; set; }
}
