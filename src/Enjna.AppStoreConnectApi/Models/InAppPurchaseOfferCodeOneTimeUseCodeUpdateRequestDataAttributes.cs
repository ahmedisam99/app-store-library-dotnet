using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an In-App Purchase Offer Code One-Time Use Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseoffercodeonetimeusecodeupdaterequest"/>
public sealed class InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that indicates whether customers can redeem the codes in the batch. Set it
    /// to <c>false</c> to stop redemptions; deactivating a batch of codes is permanent.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get => Get<bool?>(); set => Set(value); }
}
