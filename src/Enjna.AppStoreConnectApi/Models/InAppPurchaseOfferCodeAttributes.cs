using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercode"/>
public sealed class InAppPurchaseOfferCodeAttributes
{
    /// <summary>
    /// The name of the offer code, which you use to identify it in App Store Connect. Customers
    /// never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The spending behavior that makes a customer eligible to redeem the offer code.
    /// </summary>
    [JsonPropertyName("customerEligibilities")]
    public InAppPurchaseOfferCodeCustomerEligibility[]? CustomerEligibilities { get; set; }

    /// <summary>
    /// The number of codes the offer has generated for the production environment.
    /// </summary>
    [JsonPropertyName("productionCodeCount")]
    public int? ProductionCodeCount { get; set; }

    /// <summary>
    /// The number of codes the offer has generated for the sandbox environment.
    /// </summary>
    [JsonPropertyName("sandboxCodeCount")]
    public int? SandboxCodeCount { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether customers can still redeem the offer's codes.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; set; }
}
