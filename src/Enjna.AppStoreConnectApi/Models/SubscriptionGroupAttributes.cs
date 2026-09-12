using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Groups resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroup/attributes"/>
public sealed class SubscriptionGroupAttributes
{
    /// <summary>
    /// The name you use to identify the subscription group in App Store Connect. Customers never
    /// see it; the names they see are the group's localizations.
    /// </summary>
    [JsonPropertyName("referenceName")]
    public string? ReferenceName { get; set; }
}
