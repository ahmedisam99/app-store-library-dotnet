using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A price you create inline, in the <c>included</c> array of the request body that updates a
/// subscription, and refer to from the request's <c>prices</c> relationship by the placeholder ID
/// you give it here.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionpriceinlinecreate"/>
public sealed class SubscriptionPriceInlineCreate : ISubscriptionUpdateRequestIncludedResource
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionPrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionPrices";

    /// <summary>
    /// A temporary identifier you invent for this price, such as <c>${price1}</c>, and refer to
    /// from the <c>prices</c> relationship of the subscription you're updating.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The day the price takes effect and the billing plan it applies to.
    /// </summary>
    [JsonPropertyName("attributes")]
    public SubscriptionPriceInlineCreateAttributes? Attributes { get; set; }

    /// <summary>
    /// The subscription the price belongs to, the territory it applies in, and the price point
    /// that sets the amount.
    /// </summary>
    [JsonPropertyName("relationships")]
    public SubscriptionPriceInlineCreateRelationships? Relationships { get; set; }
}
