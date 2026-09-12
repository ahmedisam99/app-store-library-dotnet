using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Builds resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/build/attributes"/>
public sealed class BuildAttributes
{
    /// <summary>
    /// The build number of the uploaded build, which is unique within its pre-release version.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// The date and time you uploaded the build to App Store Connect.
    /// </summary>
    [JsonPropertyName("uploadedDate")]
    public DateTimeOffset? UploadedDate { get; set; }

    /// <summary>
    /// The date and time the build stops being available to testers. TestFlight builds expire
    /// 90 days after you upload them.
    /// </summary>
    [JsonPropertyName("expirationDate")]
    public DateTimeOffset? ExpirationDate { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the build expired, and testers can no longer install it.
    /// </summary>
    [JsonPropertyName("expired")]
    public bool? Expired { get; set; }

    /// <summary>
    /// The minimum operating system version a device needs to install the build.
    /// </summary>
    [JsonPropertyName("minOsVersion")]
    public string? MinOsVersion { get; set; }

    /// <summary>
    /// The minimum macOS version the build declares in its <c>LSMinimumSystemVersion</c> key.
    /// </summary>
    [JsonPropertyName("lsMinimumSystemVersion")]
    public string? LsMinimumSystemVersion { get; set; }

    /// <summary>
    /// The minimum macOS version App Store Connect computed for the build from its contents.
    /// </summary>
    [JsonPropertyName("computedMinMacOsVersion")]
    public string? ComputedMinMacOsVersion { get; set; }

    /// <summary>
    /// The minimum visionOS version App Store Connect computed for the build from its contents.
    /// </summary>
    [JsonPropertyName("computedMinVisionOsVersion")]
    public string? ComputedMinVisionOsVersion { get; set; }

    /// <summary>
    /// The app icon of the build, as a processed image asset.
    /// </summary>
    [JsonPropertyName("iconAssetToken")]
    public ImageAsset? IconAssetToken { get; set; }

    /// <summary>
    /// The state of App Store Connect's processing of the uploaded build.
    /// </summary>
    [JsonPropertyName("processingState")]
    public BuildProcessingState? ProcessingState { get; set; }

    /// <summary>
    /// The audience the build is available to.
    /// </summary>
    [JsonPropertyName("buildAudienceType")]
    public BuildAudienceType? BuildAudienceType { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the build uses encryption that export compliance
    /// doesn't exempt.
    /// </summary>
    [JsonPropertyName("usesNonExemptEncryption")]
    public bool? UsesNonExemptEncryption { get; set; }
}
