using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of a request that submits a subscription group for review. The request has no
/// attributes; the group you point the relationship at is the whole of it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptiongroupsubmissioncreaterequest/data"/>
public sealed class SubscriptionGroupSubmissionCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionGroupSubmissions</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionGroupSubmissions";

    /// <summary>
    /// The relationships to other resources that you set with this request. They are required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionGroupSubmissionCreateRequestDataRelationships Relationships { get; set; }
}
