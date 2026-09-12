using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an Apps resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/app/attributes"/>
public sealed class AppAttributes
{
    /// <summary>
    /// A URL to a page of your own that describes the accessibility features the app supports.
    /// </summary>
    [JsonPropertyName("accessibilityUrl")]
    public string? AccessibilityUrl { get; set; }

    /// <summary>
    /// The name of the app as it appears on the App Store.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The bundle ID of the app, which matches the bundle ID you register in your developer account.
    /// </summary>
    [JsonPropertyName("bundleId")]
    public string? BundleId { get; set; }

    /// <summary>
    /// The SKU you assign to the app. It is unique within your account, and customers never see it.
    /// </summary>
    [JsonPropertyName("sku")]
    public string? Sku { get; set; }

    /// <summary>
    /// The primary locale of the app, such as <c>en-US</c>. The App Store falls back to this
    /// locale's metadata in territories the app has no localization for.
    /// </summary>
    [JsonPropertyName("primaryLocale")]
    public string? PrimaryLocale { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the app is, or has ever been, in the Kids category.
    /// </summary>
    [JsonPropertyName("isOrEverWasMadeForKids")]
    public bool? IsOrEverWasMadeForKids { get; set; }

    /// <summary>
    /// The production URL that receives App Store Server Notifications about the app's subscribers.
    /// </summary>
    [JsonPropertyName("subscriptionStatusUrl")]
    public string? SubscriptionStatusUrl { get; set; }

    /// <summary>
    /// The version of App Store Server Notifications that Apple sends to the production URL.
    /// </summary>
    [JsonPropertyName("subscriptionStatusUrlVersion")]
    public SubscriptionStatusUrlVersion? SubscriptionStatusUrlVersion { get; set; }

    /// <summary>
    /// The sandbox URL that receives App Store Server Notifications about the app's test subscribers.
    /// </summary>
    [JsonPropertyName("subscriptionStatusUrlForSandbox")]
    public string? SubscriptionStatusUrlForSandbox { get; set; }

    /// <summary>
    /// The version of App Store Server Notifications that Apple sends to the sandbox URL.
    /// </summary>
    [JsonPropertyName("subscriptionStatusUrlVersionForSandbox")]
    public SubscriptionStatusUrlVersion? SubscriptionStatusUrlVersionForSandbox { get; set; }

    /// <summary>
    /// Your declaration of whether the app displays or contains third-party content.
    /// </summary>
    [JsonPropertyName("contentRightsDeclaration")]
    public AppContentRightsDeclaration? ContentRightsDeclaration { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the app offers streamlined purchasing, which lets a
    /// customer complete a purchase without leaving the app's own purchase flow.
    /// </summary>
    [JsonPropertyName("streamlinedPurchasingEnabled")]
    public bool? StreamlinedPurchasingEnabled { get; set; }
}
