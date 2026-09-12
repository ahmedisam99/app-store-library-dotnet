using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that sets the territory availability of an in-app purchase.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseavailabilitycreaterequest/data/attributes"/>
public sealed class InAppPurchaseAvailabilityCreateRequestDataAttributes
{
    /// <summary>
    /// Whether the in-app purchase goes on sale automatically in territories the App Store opens
    /// later. This attribute is required.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public required bool AvailableInNewTerritories { get; set; }
}
