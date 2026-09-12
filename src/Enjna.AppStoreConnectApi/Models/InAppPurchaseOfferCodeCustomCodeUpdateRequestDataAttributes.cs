using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an In-App Purchase Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodecustomcodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeCustomCodeUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that indicates whether customers can redeem the code. Set it to <c>false</c>
    /// to stop redemptions; deactivating a custom code is permanent.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get => Get<bool?>(); set => Set(value); }
}
