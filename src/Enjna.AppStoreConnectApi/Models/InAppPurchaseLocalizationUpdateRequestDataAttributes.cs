using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationupdaterequest/data/attributes"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use InAppPurchaseLocalizationV2UpdateRequestDataAttributes instead.")]
public sealed class InAppPurchaseLocalizationUpdateRequestDataAttributes : AttributeChangeSet
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
