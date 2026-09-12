using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that updates an auto-renewable subscription. The product
/// identifier isn't among them, because you can't change it once you create the subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionupdaterequest/data/attributes"/>
public sealed class SubscriptionUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The reference name of the subscription, which appears in App Store Connect and in your sales
    /// and trends reports.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the subscription with the
    /// members of their Family Sharing group.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// The length of a single billing period of the subscription.
    /// </summary>
    [JsonPropertyName("subscriptionPeriod")]
    public SubscriptionPeriod? SubscriptionPeriod { get => Get<SubscriptionPeriod?>(); set => Set(value); }

    /// <summary>
    /// A note for App Review that explains how to reach and verify the subscription in your app.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("reviewNote")]
    public string? ReviewNote { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The rank of the subscription within its subscription group, where level 1 is the highest
    /// level of service.
    /// </summary>
    [JsonPropertyName("groupLevel")]
    public int? GroupLevel { get => Get<int?>(); set => Set(value); }
}
