using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to set the territories a subscription is for sale in. Apple deprecated
/// this request along with the Subscription Availabilities resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionavailabilitycreaterequest"/>
public sealed class SubscriptionAvailabilityCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionAvailabilityCreateRequestData Data { get; set; }
}
