using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a Subscription Localizations resource of the v2
/// API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionlocalizationv2updaterequest/data/attributes"/>
public sealed class SubscriptionLocalizationV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new display name customers see for the subscription in this language.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The new description customers see for the subscription in this language.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("description")]
    public string? Description { get => Get<string?>(); set => Set(value); }
}
