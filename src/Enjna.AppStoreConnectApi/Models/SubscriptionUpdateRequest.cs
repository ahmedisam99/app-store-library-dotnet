using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an auto-renewable subscription.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionupdaterequest"/>
public sealed class SubscriptionUpdateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionUpdateRequestData Data { get; set; }

    /// <summary>
    /// Related resources to create in the same request, which the relationships of
    /// <see cref="Data"/> then point at by their client-assigned IDs. Every entry is a
    /// <see cref="SubscriptionPriceInlineCreate"/>, a
    /// <see cref="SubscriptionIntroductoryOfferInlineCreate"/>, or a
    /// <see cref="SubscriptionPromotionalOfferInlineCreate"/>, the three types Apple allows here,
    /// so the array is heterogeneous and every entry serializes by its own runtime type.
    /// </summary>
    [JsonPropertyName("included")]
    public ISubscriptionUpdateRequestIncludedResource[]? Included { get; set; }
}
