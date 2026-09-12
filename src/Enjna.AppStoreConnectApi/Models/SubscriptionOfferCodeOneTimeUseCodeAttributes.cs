using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Offer Code One-Time Use Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodeonetimeusecode"/>
public sealed class SubscriptionOfferCodeOneTimeUseCodeAttributes
{
    /// <summary>
    /// The number of unique codes in the batch.
    /// </summary>
    [JsonPropertyName("numberOfCodes")]
    public int? NumberOfCodes { get; set; }

    /// <summary>
    /// The date you created the batch of codes.
    /// </summary>
    [JsonPropertyName("createdDate")]
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// The last date customers can redeem a code from the batch.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether customers can still redeem the codes in the batch.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    /// <summary>
    /// The environment the codes in the batch are redeemable in.
    /// </summary>
    [JsonPropertyName("environment")]
    public OfferCodeEnvironment? Environment { get; set; }
}
