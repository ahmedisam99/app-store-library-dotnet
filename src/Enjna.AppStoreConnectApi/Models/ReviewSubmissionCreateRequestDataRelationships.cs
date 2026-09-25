using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates a Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissioncreaterequest/data/relationships"/>
public sealed class ReviewSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The related Apps resource the submission belongs to. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }
}
