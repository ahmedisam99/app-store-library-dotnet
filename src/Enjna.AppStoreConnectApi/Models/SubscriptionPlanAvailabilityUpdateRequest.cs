using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to change the territories one billing plan of a subscription is for
/// sale in.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilityupdaterequest"/>
public sealed class SubscriptionPlanAvailabilityUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionPlanAvailabilityUpdateRequestData Data { get; set; }
}
