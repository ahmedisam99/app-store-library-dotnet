using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that creates a v2 In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationv2createrequest/data"/>
public sealed class InAppPurchaseLocalizationV2CreateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseLocalizations";

    /// <summary>
    /// The attributes that describe the localization to create. This member is required.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required InAppPurchaseLocalizationV2CreateRequestDataAttributes Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources that you set with this request. This member is required.
    /// </summary>
    [JsonPropertyName("relationships")]
    public required InAppPurchaseLocalizationV2CreateRequestDataRelationships Relationships { get; set; }
}
