using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that commits a subscription promotional image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimageupdaterequest/data/attributes"/>
[Obsolete("Apple deprecated this request in App Store Connect API 4.4.1. Use SubscriptionImageV2UpdateRequestDataAttributes instead.")]
public sealed class SubscriptionImageUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The MD5 checksum of the whole image file, which
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
