using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that updates an Apps resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appupdaterequest/data/attributes"/>
public sealed class AppUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A URL to a page of your own that describes the accessibility features the app supports.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("accessibilityUrl")]
    public string? AccessibilityUrl { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The bundle ID of the app. You can only set it while the app has no build and no version
    /// that reached the App Store.
    /// </summary>
    [JsonPropertyName("bundleId")]
    public string? BundleId { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The primary locale of the app, such as <c>en-US</c>.
    /// </summary>
    [JsonPropertyName("primaryLocale")]
    public string? PrimaryLocale { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The production URL that receives App Store Server Notifications about the app's subscribers.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("subscriptionStatusUrl")]
    public string? SubscriptionStatusUrl { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The version of App Store Server Notifications that Apple sends to the production URL.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("subscriptionStatusUrlVersion")]
    public SubscriptionStatusUrlVersion? SubscriptionStatusUrlVersion { get => Get<SubscriptionStatusUrlVersion?>(); set => Set(value); }

    /// <summary>
    /// The sandbox URL that receives App Store Server Notifications about the app's test subscribers.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("subscriptionStatusUrlForSandbox")]
    public string? SubscriptionStatusUrlForSandbox { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// The version of App Store Server Notifications that Apple sends to the sandbox URL.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("subscriptionStatusUrlVersionForSandbox")]
    public SubscriptionStatusUrlVersion? SubscriptionStatusUrlVersionForSandbox { get => Get<SubscriptionStatusUrlVersion?>(); set => Set(value); }

    /// <summary>
    /// Your declaration of whether the app displays or contains third-party content.
    /// </summary>
    [JsonPropertyName("contentRightsDeclaration")]
    public AppContentRightsDeclaration? ContentRightsDeclaration { get => Get<AppContentRightsDeclaration?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that indicates whether the app offers streamlined purchasing.
    /// </summary>
    [JsonPropertyName("streamlinedPurchasingEnabled")]
    public bool? StreamlinedPurchasingEnabled { get => Get<bool?>(); set => Set(value); }
}
