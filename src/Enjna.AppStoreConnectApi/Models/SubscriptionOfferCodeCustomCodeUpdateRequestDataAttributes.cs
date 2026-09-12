using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates a Subscription Offer Code Custom Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcodeupdaterequest"/>
public sealed class SubscriptionOfferCodeCustomCodeUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that indicates whether customers can redeem the code. Set it to <c>false</c>
    /// to stop redemptions; deactivating a custom code is permanent.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get => Get<bool?>(); set => Set(value); }
}
