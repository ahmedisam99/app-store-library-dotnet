using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes you change with a request that updates a subscription group localization. The
/// locale isn't among them, because you set it once when you create the localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationupdaterequest/data/attributes"/>
public sealed class SubscriptionGroupLocalizationUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new display name of the subscription group in this language.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The new app name to show in this language instead of the name on the App Store.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get => Get<string?>(); set => Set(value); }
}
