using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Subscription Offer Code Custom Codes resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionoffercodecustomcode"/>
public sealed class SubscriptionOfferCodeCustomCodeAttributes
{
    /// <summary>
    /// The alphanumeric code that customers enter to redeem the offer.
    /// </summary>
    [JsonPropertyName("customCode")]
    public string? CustomCode { get; set; }

    /// <summary>
    /// The maximum number of times customers can redeem the code.
    /// </summary>
    [JsonPropertyName("numberOfCodes")]
    public int? NumberOfCodes { get; set; }

    /// <summary>
    /// The date you created the custom code.
    /// </summary>
    [JsonPropertyName("createdDate")]
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// The last date customers can redeem the code.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public DateOnly? ExpirationDate { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether customers can still redeem the code.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; set; }
}
