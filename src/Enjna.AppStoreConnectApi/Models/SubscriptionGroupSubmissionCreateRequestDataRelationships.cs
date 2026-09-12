using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships you set on a request that submits a subscription group for review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmissioncreaterequest/data/relationships"/>
public sealed class SubscriptionGroupSubmissionCreateRequestDataRelationships
{
    /// <summary>
    /// The subscription group to submit for review. This relationship is required.
    /// </summary>
    [JsonPropertyName("subscriptionGroup")]
    public required RelationshipDeclaration SubscriptionGroup { get; set; }
}
