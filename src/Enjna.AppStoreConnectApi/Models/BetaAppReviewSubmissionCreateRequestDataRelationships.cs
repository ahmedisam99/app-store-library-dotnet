using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a request that creates a Beta App Review Submissions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betaappreviewsubmissioncreaterequest/data/relationships"/>
public sealed class BetaAppReviewSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The related Builds resource to submit for beta review. This relationship is required.
    /// </summary>
    [JsonPropertyName("build")]
    public required RelationshipDeclaration Build { get; set; }
}
