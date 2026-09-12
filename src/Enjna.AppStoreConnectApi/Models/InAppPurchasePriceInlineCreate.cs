using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A price you create inline, in the <c>included</c> array of a price schedule request, and refer
/// to from the request's <c>manualPrices</c> relationship by the placeholder ID you give it here.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceinlinecreate"/>
public sealed class InAppPurchasePriceInlineCreate : IInAppPurchasePriceScheduleCreateRequestIncludedResource
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchasePrices</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchasePrices";

    /// <summary>
    /// The placeholder ID that ties this price to the entry that refers to it. Any string works as
    /// long as it is unique within the request, such as <c>${price1}</c>.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The dates the price applies between.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchasePriceInlineCreateAttributes? Attributes { get; set; }

    /// <summary>
    /// The in-app purchase the price belongs to and the price point it charges.
    /// </summary>
    [JsonPropertyName("relationships")]
    public InAppPurchasePriceInlineCreateRelationships? Relationships { get; set; }
}
