using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that reserves a Subscription Images resource of the v2
/// API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2createrequest/data/relationships"/>
public sealed class SubscriptionImageV2CreateRequestDataRelationships
{
    /// <summary>
    /// The subscription version the image belongs to, of type <c>subscriptionVersions</c>. The
    /// version must be in <c>PREPARE_FOR_SUBMISSION</c>. This relationship is required.
    /// </summary>
    [JsonPropertyName("version")]
    public required RelationshipDeclaration Version { get; set; }
}
