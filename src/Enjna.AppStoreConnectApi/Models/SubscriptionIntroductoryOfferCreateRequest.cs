using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create an introductory offer for a subscription in one territory.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryoffercreaterequest"/>
public sealed class SubscriptionIntroductoryOfferCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionIntroductoryOfferCreateRequestData Data { get; set; }

    /// <summary>
    /// The price points the relationships of this request point at.
    /// </summary>
    [JsonPropertyName("included")]
    public SubscriptionPricePointInlineCreate[]? Included { get; set; }
}
