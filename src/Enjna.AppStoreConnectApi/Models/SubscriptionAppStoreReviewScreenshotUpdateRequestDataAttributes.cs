using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that commits a subscription App Review screenshot.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionappstorereviewscreenshotupdaterequest/data/attributes"/>
public sealed class SubscriptionAppStoreReviewScreenshotUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The MD5 checksum of the whole screenshot file, which
    /// <see cref="AppStoreConnectAPIClient.ComputeSourceFileChecksum(byte[])"/> computes for you.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// Set this to <c>true</c> to tell App Store Connect that every upload operation finished.
    /// </summary>
    [JsonPropertyName("uploaded")]
    public bool? Uploaded { get => Get<bool?>(); set => Set(value); }
}
