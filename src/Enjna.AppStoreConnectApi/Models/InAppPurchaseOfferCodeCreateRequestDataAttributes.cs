using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that creates an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecreaterequest"/>
public sealed class InAppPurchaseOfferCodeCreateRequestDataAttributes
{
    /// <summary>
    /// The name that identifies the offer code in App Store Connect. It is required, and customers
    /// never see it.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The spending behavior that makes a customer eligible to redeem the offer code. It is
    /// required, and takes at least one value.
    /// </summary>
    [JsonPropertyName("customerEligibilities")]
    public required InAppPurchaseOfferCodeCustomerEligibility[] CustomerEligibilities { get; set; }
}
