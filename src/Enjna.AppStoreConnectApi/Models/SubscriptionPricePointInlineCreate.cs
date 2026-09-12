using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A Subscription Price Points resource sent in the <c>included</c> array of a request, so that the
/// relationships in the same request body can point at it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpricepointinlinecreate"/>
public sealed class SubscriptionPricePointInlineCreate
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPricePoints</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPricePoints";

    /// <summary>
    /// The opaque resource ID of the price point.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
