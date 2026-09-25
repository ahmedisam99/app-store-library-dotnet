using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that updates localized subscription metadata.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationupdaterequest/data/attributes"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionLocalizationV2UpdateRequestDataAttributes instead.")]
public sealed class SubscriptionLocalizationUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The display name of the subscription that customers see on the App Store in this locale.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The description of the subscription that customers see on the App Store in this locale.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("description")]
    public string? Description { get => Get<string?>(); set => Set(value); }
}
