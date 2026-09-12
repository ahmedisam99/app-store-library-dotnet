using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that creates a developer response to a customer review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1createrequest/data"/>
public sealed class CustomerReviewResponseV1CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>customerReviewResponses</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "customerReviewResponses";

    /// <summary>
    /// The attributes that describe the request that creates a Customer Review Responses resource.
    /// This property is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required CustomerReviewResponseV1CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This property is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required CustomerReviewResponseV1CreateRequestDataRelationships Relationships { get; set; }
}
