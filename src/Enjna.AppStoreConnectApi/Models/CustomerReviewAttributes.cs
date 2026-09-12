using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Customer Reviews resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreview/attributes"/>
public sealed class CustomerReviewAttributes
{
    /// <summary>
    /// The star rating the customer gave the app, from 1 through 5.
    /// </summary>
    [JsonPropertyName("rating")]
    public int? Rating { get; set; }

    /// <summary>
    /// The title the customer gave the review.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The text of the review the customer wrote.
    /// </summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }

    /// <summary>
    /// The nickname the customer publishes the review under.
    /// </summary>
    [JsonPropertyName("reviewerNickname")]
    public string? ReviewerNickname { get; set; }

    /// <summary>
    /// The date and time the customer wrote the review.
    /// </summary>
    [JsonPropertyName("createdDate")]
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// The App Store territory of the storefront the customer wrote the review in.
    /// </summary>
    [JsonPropertyName("territory")]
    public TerritoryCode? Territory { get; set; }
}
