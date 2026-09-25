using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The resource data of the request that reserves a subscription promotional image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagecreaterequest/data"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionImageV2CreateRequestData instead.")]
public sealed class SubscriptionImageCreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>subscriptionImages</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "subscriptionImages";

    /// <summary>
    /// The attributes that describe the image to reserve. This value is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required SubscriptionImageCreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This value is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required SubscriptionImageCreateRequestDataRelationships Relationships { get; set; }
}
