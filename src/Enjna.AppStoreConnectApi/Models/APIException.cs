using System;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// An exception that indicates an App Store Connect API error response.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/parsing-the-error-response-code"/>
public sealed class APIException : Exception
{
    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    public int HttpStatusCode { get; }

    /// <summary>
    /// The errors from the API response, if the response body was a parsable error document.
    /// Compare <see cref="ErrorDetail.Code"/> by prefix rather than by exact match.
    /// </summary>
    public ErrorDetail[]? Errors { get; }

    /// <summary>
    /// The rate-limit information from the response, if the API sent it.
    /// </summary>
    public RateLimit? RateLimit { get; }

    /// <summary>
    /// How long the API asked you to wait before retrying, from the response's <c>Retry-After</c>
    /// header. A response that gives the header as a date is converted to the delay remaining when
    /// the response was read. It is <c>null</c> when the response didn't carry the header, which is
    /// the usual case outside <c>429 Too Many Requests</c> and <c>503 Service Unavailable</c>.
    /// </summary>
    public TimeSpan? RetryAfter { get; }

    /// <summary>
    /// The raw response body, if it was readable. A <c>406 Not Acceptable</c> response, for example,
    /// carries plain text rather than JSON.
    /// </summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Creates a new App Store Connect API exception.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code of the response.</param>
    /// <param name="errors">The errors from the API response, if available.</param>
    /// <param name="rateLimit">The rate-limit information from the response, if available.</param>
    /// <param name="retryAfter">How long the API asked you to wait before retrying, if available.</param>
    /// <param name="responseBody">The raw response body, if available.</param>
    /// <param name="innerException">The exception that caused this one, if any.</param>
    public APIException(
        int httpStatusCode,
        ErrorDetail[]? errors = null,
        RateLimit? rateLimit = null,
        TimeSpan? retryAfter = null,
        string? responseBody = null,
        Exception? innerException = null)
        : base(BuildMessage(httpStatusCode, errors, responseBody), innerException)
    {
        HttpStatusCode = httpStatusCode;
        Errors = errors;
        RateLimit = rateLimit;
        RetryAfter = retryAfter;
        ResponseBody = responseBody;
    }

    private static string BuildMessage(int httpStatusCode, ErrorDetail[]? errors, string? responseBody)
    {
        var first = errors is { Length: > 0 } ? errors[0] : null;

        if (first?.Detail is not null || first?.Title is not null)
        {
            return first.Detail ?? first.Title;
        }

        if (httpStatusCode == 0)
        {
            return responseBody is { Length: > 0 } description
                ? description
                : "App Store Connect API error";
        }

        return $"App Store Connect API error: HTTP {httpStatusCode}";
    }
}
