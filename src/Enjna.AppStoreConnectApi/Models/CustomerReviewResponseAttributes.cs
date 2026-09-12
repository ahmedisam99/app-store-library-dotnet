using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Customer Review Responses resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1/attributes"/>
public sealed class CustomerReviewResponseAttributes
{
    /// <summary>
    /// The text of your response to the customer review.
    /// </summary>
    [JsonPropertyName("responseBody")]
    public string? ResponseBody { get; set; }

    /// <summary>
    /// The date and time you last changed the response.
    /// </summary>
    [JsonPropertyName("lastModifiedDate")]
    public DateTimeOffset? LastModifiedDate { get; set; }

    /// <summary>
    /// Whether the response is live on the App Store, or still waiting to be published.
    /// </summary>
    [JsonPropertyName("state")]
    public CustomerReviewResponseState? State { get; set; }
}
