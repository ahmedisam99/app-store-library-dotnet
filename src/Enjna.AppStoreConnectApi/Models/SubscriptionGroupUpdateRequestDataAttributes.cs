using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you change with a request that updates a subscription group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupupdaterequest/data/attributes"/>
public sealed class SubscriptionGroupUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new name you use to identify the subscription group in App Store Connect.
    /// </summary>
    [JsonPropertyName("referenceName")]
    public string? ReferenceName { get => Get<string?>(); set => Set(value); }
}
