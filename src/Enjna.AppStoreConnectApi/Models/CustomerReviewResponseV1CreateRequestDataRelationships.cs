using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a request that creates a developer response to a customer review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1createrequest/data/relationships"/>
public sealed class CustomerReviewResponseV1CreateRequestDataRelationships
{
    /// <summary>
    /// The related Customer Reviews resource that this response answers. This relationship is required.
    /// </summary>
    [JsonPropertyName("review")]
    public required RelationshipDeclaration Review { get; set; }
}
