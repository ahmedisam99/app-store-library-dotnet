using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Availabilities resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseavailability/attributes"/>
public sealed class InAppPurchaseAvailabilityAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether the in-app purchase goes on sale automatically in
    /// territories the App Store opens later.
    /// </summary>
    [JsonPropertyName("availableInNewTerritories")]
    public bool? AvailableInNewTerritories { get; set; }
}
