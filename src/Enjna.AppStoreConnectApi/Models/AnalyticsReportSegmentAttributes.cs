using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an Analytics Report Segments resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportsegment/attributes"/>
public sealed class AnalyticsReportSegmentAttributes
{
    /// <summary>
    /// The checksum of the segment's file, to verify the download with.
    /// </summary>
    [JsonPropertyName("checksum")]
    public string? Checksum { get; set; }

    /// <summary>
    /// The size of the segment's file, in bytes.
    /// </summary>
    [JsonPropertyName("sizeInBytes")]
    public long? SizeInBytes { get; set; }

    /// <summary>
    /// The URL to download the segment's file from. The URL is pre-signed, unauthenticated, and
    /// expires, so download it promptly rather than storing it.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
