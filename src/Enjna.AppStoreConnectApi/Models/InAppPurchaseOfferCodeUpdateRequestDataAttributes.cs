using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an In-App Purchase Offer Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that indicates whether customers can redeem the offer's codes. Set it to
    /// <c>false</c> to stop redemptions; deactivating an offer code is permanent.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get => Get<bool?>(); set => Set(value); }
}
