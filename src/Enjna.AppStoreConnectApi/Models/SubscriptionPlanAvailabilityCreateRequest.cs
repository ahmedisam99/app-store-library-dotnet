using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to set the territories one billing plan of a subscription is for sale in.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionplanavailabilitycreaterequest"/>
public sealed class SubscriptionPlanAvailabilityCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionPlanAvailabilityCreateRequestData Data { get; set; }
}
