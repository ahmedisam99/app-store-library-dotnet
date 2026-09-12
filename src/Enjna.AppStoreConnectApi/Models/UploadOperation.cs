using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// One part of an asset upload. App Store Connect returns these when you reserve an asset, and
/// decides how it wants the file split.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/uploadoperation"/>
public sealed class UploadOperation
{
    /// <summary>
    /// The HTTP method to use for this part, usually <c>PUT</c>.
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    /// <summary>
    /// The pre-signed, unauthenticated and time-limited URL to send this part to. Don't share it.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The number of bytes of the original file this part carries.
    /// </summary>
    [JsonPropertyName("length")]
    public int? Length { get; set; }

    /// <summary>
    /// The byte offset into the original file at which this part starts.
    /// </summary>
    [JsonPropertyName("offset")]
    public int? Offset { get; set; }

    /// <summary>
    /// The headers to send with this part, verbatim.
    /// </summary>
    [JsonPropertyName("requestHeaders")]
    public UploadOperationHeader[]? RequestHeaders { get; set; }
}
