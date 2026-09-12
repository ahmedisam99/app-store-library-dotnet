using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The location of an error within the request. Apple sends either a pointer or a parameter, never both.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/pinpointing-the-location-of-errors"/>
public sealed class ErrorSource
{
    /// <summary>
    /// A JSON pointer, as defined by RFC 6901, that indicates the part of the request body that caused the error.
    /// </summary>
    [JsonPropertyName("pointer")]
    public string? Pointer { get; set; }

    /// <summary>
    /// The query parameter that caused the error.
    /// </summary>
    [JsonPropertyName("parameter")]
    public string? Parameter { get; set; }
}
