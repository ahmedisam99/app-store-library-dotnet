using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a request that creates a developer response to a customer review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1createrequest/data/attributes"/>
public sealed class CustomerReviewResponseV1CreateRequestDataAttributes
{
    /// <summary>
    /// The text of your response to the customer review. This attribute is required.
    /// </summary>
    [JsonPropertyName("responseBody")]
    public required string ResponseBody { get; set; }
}
