using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a subscription group localization of the
/// version-based workflow. The locale isn't among them, because you set it once when you create
/// the localization.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongrouplocalizationv2updaterequest/data/attributes"/>
public sealed class SubscriptionGroupLocalizationV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new display name of the subscription group in this language.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The new custom app name that customers see for the subscription group in this language.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("customAppName")]
    public string? CustomAppName { get => Get<string?>(); set => Set(value); }
}
