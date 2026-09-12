using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// An HTTP header that you send with an asset upload operation.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/httpheader"/>
public sealed class UploadOperationHeader
{
    /// <summary>
    /// The header name, such as <c>Content-Type</c>.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The header value.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
