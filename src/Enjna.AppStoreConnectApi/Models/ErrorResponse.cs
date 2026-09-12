using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The error details that an unsuccessful App Store Connect API response returns.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/errorresponse"/>
public sealed class ErrorResponse
{
    /// <summary>
    /// One or more errors that describe why the request failed.
    /// </summary>
    [JsonPropertyName("errors")]
    public ErrorDetail[]? Errors { get; set; }
}
