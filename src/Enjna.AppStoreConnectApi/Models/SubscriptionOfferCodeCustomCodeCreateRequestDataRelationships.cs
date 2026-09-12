using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Subscription Offer Code Custom Codes
/// resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcodecreaterequest"/>
public sealed class SubscriptionOfferCodeCustomCodeCreateRequestDataRelationships
{
    /// <summary>
    /// The Subscription Offer Codes resource the custom code belongs to. This relationship is
    /// required.
    /// </summary>
    [JsonPropertyName("offerCode")]
    public required RelationshipDeclaration OfferCode { get; set; }
}
