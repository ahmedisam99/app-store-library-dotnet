using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you set on a request that creates a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupcreaterequest/data/attributes"/>
public sealed class SubscriptionGroupCreateRequestDataAttributes
{
    /// <summary>
    /// The name you use to identify the subscription group in App Store Connect. It is required,
    /// and customers never see it.
    /// </summary>
    [JsonPropertyName("referenceName")]
    public required string ReferenceName { get; set; }
}
