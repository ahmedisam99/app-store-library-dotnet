using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The details of a single error the App Store Connect API returns.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/errorresponse/errors-data.dictionary"/>
public sealed class ErrorDetail
{
    /// <summary>
    /// The unique ID of a specific instance of an error, request, and response. Provide it when you
    /// report an issue to Apple.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The HTTP status code of the error, as a string. It usually matches the response's status code,
    /// but the two can differ when one request produces several errors.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    /// <summary>
    /// A machine-readable code that indicates the type of error. The code is hierarchical and
    /// dot-separated, so compare it by prefix rather than by exact match.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// A summary of the error. Don't use this value for programmatic error handling.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// A detailed explanation of the error. Don't use this value for programmatic error handling.
    /// </summary>
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = null!;

    /// <summary>
    /// The location of the error within the request, when the API can determine it.
    /// </summary>
    [JsonPropertyName("source")]
    public ErrorSource? Source { get; set; }

    /// <summary>
    /// Links that provide more information about the error.
    /// </summary>
    [JsonPropertyName("links")]
    public ErrorLinks? Links { get; set; }

    /// <summary>
    /// Additional, free-form information about the error.
    /// </summary>
    [JsonPropertyName("meta")]
    public JsonElement? Meta { get; set; }
}
