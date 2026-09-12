using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to respond to a customer review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1createrequest"/>
public sealed class CustomerReviewResponseV1CreateRequest
{
    /// <summary>
    /// The resource data. This property is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required CustomerReviewResponseV1CreateRequestData Data { get; set; }
}
