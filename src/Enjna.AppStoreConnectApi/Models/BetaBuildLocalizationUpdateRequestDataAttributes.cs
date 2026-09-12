using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a request that updates a Beta Build Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betabuildlocalizationupdaterequest/data/attributes"/>
public sealed class BetaBuildLocalizationUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A description of the changes and the features testers should try out in this build.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("whatsNew")]
    public string? WhatsNew { get => Get<string?>(); set => Set(value); }
}
