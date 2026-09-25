using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a v2 In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2updaterequest/data/attributes"/>
public sealed class InAppPurchaseLocalizationV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new display name customers see for the in-app purchase in this language.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The new description customers see for the in-app purchase in this language.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("description")]
    public string? Description { get => Get<string?>(); set => Set(value); }
}
