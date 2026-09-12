using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an In-App Purchase Localizations resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaselocalizationupdaterequest/data"/>
public sealed class InAppPurchaseLocalizationUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>inAppPurchaseLocalizations</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "inAppPurchaseLocalizations";

    /// <summary>
    /// The opaque resource ID of the localization to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change. Leave an attribute unset to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public InAppPurchaseLocalizationUpdateRequestDataAttributes? Attributes { get; set; }
}
