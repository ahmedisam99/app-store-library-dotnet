using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an In-App Purchases resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasev2updaterequest/data/attributes"/>
public sealed class InAppPurchaseV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The new reference name of the in-app purchase.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The new note for App Review.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("reviewNote")]
    public string? ReviewNote { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether a customer can share the in-app purchase with their
    /// family group through Family Sharing.
    /// </summary>
    [JsonPropertyName("familySharable")]
    public bool? FamilySharable { get => Get<bool?>(); set => Set(value); }
}
