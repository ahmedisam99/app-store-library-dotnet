using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Enjna.AppStoreConnectApi;

/// <summary>
/// A client for the App Store Connect API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public partial class AppStoreConnectAPIClient : IDisposable
{
    private const string BaseUrl = "https://api.appstoreconnect.apple.com";
    private static readonly string UserAgent = $"enjna-app-store-connect-api/dotnet/{PackageVersion()}";
    private const string Audience = "appstoreconnect-v1";
    private const string RateLimitHeader = "X-Rate-Limit";
    private const string IndividualKeySubject = "user";
    private static readonly TimeSpan MaximumTokenLifetime = TimeSpan.FromMinutes(20);

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { AttributeChangeSet.ConfigureChangeTracking }
        }
    };

    private readonly string _signingKey;
    private readonly string _keyId;
    private readonly string? _issuerId;
    private readonly bool _isIndividualKey;
    private readonly string _urlBase;
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private TimeSpan _tokenLifetime = TimeSpan.FromMinutes(15);
    private bool _disposed;

    /// <summary>
    /// Creates a new App Store Connect API client that authenticates with a team API key.
    /// </summary>
    /// <param name="signingKey">Your private key downloaded from App Store Connect, in PEM format.</param>
    /// <param name="keyId">The key ID of the private key, from Users and Access, Integrations in App Store Connect.</param>
    /// <param name="issuerId">Your team's issuer ID, from Users and Access, Integrations in App Store Connect.</param>
    /// <param name="httpClient">An optional <see cref="HttpClient"/> instance to use for requests.</param>
    /// <param name="baseUrl">An optional base URL to send requests to, which defaults to the App Store Connect API.</param>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/creating-api-keys-for-app-store-connect-api"/>
    public AppStoreConnectAPIClient(
        string signingKey,
        string keyId,
        string issuerId,
        HttpClient? httpClient = null,
        string? baseUrl = null)
        : this(signingKey, keyId, issuerId, false, httpClient, baseUrl)
    {
    }

    private AppStoreConnectAPIClient(
        string signingKey,
        string keyId,
        string? issuerId,
        bool isIndividualKey,
        HttpClient? httpClient,
        string? baseUrl)
    {
        _signingKey = signingKey;
        _keyId = keyId;
        _issuerId = issuerId;
        _isIndividualKey = isIndividualKey;
        _urlBase = (baseUrl ?? BaseUrl).TrimEnd('/');
        _ownsHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
    }

    /// <summary>
    /// Creates a new App Store Connect API client that authenticates with an individual, personal
    /// API key. An individual key has no issuer ID, and reaches only the endpoints the associated
    /// user's role allows.
    /// </summary>
    /// <param name="signingKey">Your private key downloaded from App Store Connect, in PEM format.</param>
    /// <param name="keyId">The key ID of the private key, from your App Store Connect user profile.</param>
    /// <param name="httpClient">An optional <see cref="HttpClient"/> instance to use for requests.</param>
    /// <param name="baseUrl">An optional base URL to send requests to, which defaults to the App Store Connect API.</param>
    /// <returns>A client that signs its tokens with an individual API key.</returns>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/creating-api-keys-for-app-store-connect-api"/>
    public static AppStoreConnectAPIClient ForIndividualKey(
        string signingKey,
        string keyId,
        HttpClient? httpClient = null,
        string? baseUrl = null)
    {
        return new AppStoreConnectAPIClient(signingKey, keyId, null, true, httpClient, baseUrl);
    }

    /// <summary>
    /// The lifetime of the bearer tokens this client signs. It defaults to 15 minutes, and Apple
    /// rejects any token whose lifetime is longer than 20 minutes.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the lifetime isn't positive, or is longer than 20 minutes.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/generating-tokens-for-api-requests"/>
    public TimeSpan TokenLifetime
    {
        get => _tokenLifetime;
        set
        {
            if (value <= TimeSpan.Zero || value > MaximumTokenLifetime)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    "The token lifetime must be positive and no longer than 20 minutes.");
            }

            _tokenLifetime = value;
        }
    }

    /// <summary>
    /// The optional <c>scope</c> claim of the bearer tokens this client signs, such as
    /// <c>GET /v1/apps?filter[platform]=IOS</c>. A token that carries a scope is rejected for any
    /// request that no scope entry matches. <c>null</c>, the default, leaves the token unrestricted.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/generating-tokens-for-api-requests"/>
    public string[]? TokenScope { get; set; }

    /// <summary>
    /// The rate-limit information from the most recent response, whether the request succeeded or
    /// not. Apple documents an <c>X-Rate-Limit</c> header on every response, but a response that
    /// arrives without one sets this to <c>null</c> rather than leaving an older reading in place,
    /// so it always describes the latest response. Reads and writes can report different budgets,
    /// as <see cref="RateLimit"/> describes, so a reading taken after a write may not carry the
    /// hourly figures a read does. It is client-wide: every response
    /// this client receives overwrites it, on any thread, so a client shared across concurrent
    /// requests reports whichever response landed last rather than the one belonging to any
    /// particular call. To read the rate limit of one specific failed call, use
    /// <see cref="APIException.RateLimit"/> instead.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/identifying-rate-limits"/>
    public RateLimit? LastRateLimit { get; private set; }

    /// <summary>
    /// Reads the next page of a paged response.
    /// </summary>
    /// <typeparam name="TResource">The type of the resources the response contains.</typeparam>
    /// <param name="page">The page to read the next page of.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The next page, or <c>null</c> when the given page is the last one.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the page is <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/pageddocumentlinks"/>
    public async Task<ResourceListResponse<TResource>?> GetNextPageAsync<TResource>(
        ResourceListResponse<TResource> page,
        CancellationToken cancellationToken = default) where TResource : class
    {
        if (page is null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        var next = page.Links.Next;

        if (string.IsNullOrEmpty(next))
        {
            return null;
        }

        return await MakeLinkRequestAsync<ResourceListResponse<TResource>>(
                link: next,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Enumerates a paged response and every page that follows it, one page at a time.
    /// </summary>
    /// <typeparam name="TResource">The type of the resources the response contains.</typeparam>
    /// <param name="firstPage">The page to start from.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous sequence of pages, starting with the given one.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/pageddocumentlinks"/>
    public async IAsyncEnumerable<ResourceListResponse<TResource>> EnumeratePagesAsync<TResource>(
        ResourceListResponse<TResource> firstPage,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) where TResource : class
    {
        var page = firstPage;

        while (page is not null)
        {
            yield return page;

            page = await GetNextPageAsync(page, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Enumerates every resource of a paged response and of every page that follows it.
    /// </summary>
    /// <typeparam name="TResource">The type of the resources the response contains.</typeparam>
    /// <param name="firstPage">The page to start from.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous sequence of resources, starting with those of the given page.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/pageddocumentlinks"/>
    public async IAsyncEnumerable<TResource> EnumerateResourcesAsync<TResource>(
        ResourceListResponse<TResource> firstPage,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) where TResource : class
    {
        var pages = EnumeratePagesAsync(firstPage, cancellationToken).ConfigureAwait(false);

        await foreach (var page in pages)
        {
            foreach (var resource in page.Data)
            {
                yield return resource;
            }
        }
    }

    private async Task<T> MakeRequestAsync<T>(
        string path,
        HttpMethod method,
        Dictionary<string, string[]>? queryParameters,
        object? body,
        bool parseResponse,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendRequestAsync(
                url: BuildRequestUrl(path, queryParameters),
                method: method,
                body: body,
                accept: "application/json",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!parseResponse)
        {
            return default!;
        }

        return await ParseResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    private async Task<byte[]> MakeRawRequestAsync(
        string path,
        HttpMethod method,
        Dictionary<string, string[]>? queryParameters,
        string accept,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendRequestAsync(
                url: BuildRequestUrl(path, queryParameters),
                method: method,
                body: null,
                accept: accept,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> MakeRawTextRequestAsync(
        string path,
        HttpMethod method,
        Dictionary<string, string[]>? queryParameters,
        string accept,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendRequestAsync(
                url: BuildRequestUrl(path, queryParameters),
                method: method,
                body: null,
                accept: accept,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> MakeLinkRequestAsync<T>(
        string link,
        CancellationToken cancellationToken = default)
    {
        var isAbsolute = Uri.TryCreate(link, UriKind.Absolute, out var absolute)
                         && (absolute.Scheme == Uri.UriSchemeHttp || absolute.Scheme == Uri.UriSchemeHttps);

        var pathAndQuery = isAbsolute
            ? absolute!.PathAndQuery
            : link.StartsWith('/')
                ? link
                : "/" + link;

        using var response = await SendRequestAsync(
                url: _urlBase + pathAndQuery,
                method: HttpMethod.Get,
                body: null,
                accept: "application/json",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return await ParseResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendRequestAsync(
        string url,
        HttpMethod method,
        object? body,
        string accept,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, url);
        request.Headers.TryAddWithoutValidation("User-Agent", UserAgent);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateBearerToken());
        request.Headers.TryAddWithoutValidation("Accept", accept);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var rateLimit = CaptureRateLimit(response);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        using (response)
        {
            throw await CreateApiExceptionAsync(response, rateLimit, cancellationToken).ConfigureAwait(false);
        }
    }

    private string BuildRequestUrl(string path, Dictionary<string, string[]>? queryParameters)
    {
        var url = new StringBuilder(_urlBase).Append(path);

        if (queryParameters is not { Count: > 0 })
        {
            return url.ToString();
        }

        var separator = '?';

        foreach (var (key, values) in queryParameters)
        {
            if (values is not { Length: > 0 })
            {
                continue;
            }

            url.Append(separator)
                .Append(key)
                .Append('=')
                .Append(string.Join(',', values.Select(Uri.EscapeDataString)));

            separator = '&';
        }

        return url.ToString();
    }

    private RateLimit? CaptureRateLimit(HttpResponseMessage response)
    {
        var rateLimit = response.Headers.TryGetValues(RateLimitHeader, out var values)
            ? RateLimit.Parse(string.Join(";", values))
            : null;

        LastRateLimit = rateLimit;

        return rateLimit;
    }

    private static async Task<T> ParseResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var responseBodyStr = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var responseBody = JsonSerializer.Deserialize<T>(responseBodyStr, JsonOptions);

        if (responseBody is null)
        {
            throw new Exception("Unexpected response body format");
        }

        return responseBody;
    }

    private static async Task<APIException> CreateApiExceptionAsync(
        HttpResponseMessage response,
        RateLimit? rateLimit,
        CancellationToken cancellationToken)
    {
        var retryAfter = ParseRetryAfter(response);
        string? rawBody = null;

        try
        {
            rawBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var error = JsonSerializer.Deserialize<ErrorResponse>(rawBody, JsonOptions);

            if (error?.Errors is { Length: > 0 } errors)
            {
                return new APIException((int)response.StatusCode, errors, rateLimit, retryAfter, rawBody);
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // ignored
        }

        return new APIException((int)response.StatusCode, null, rateLimit, retryAfter, rawBody);
    }

    private static TimeSpan? ParseRetryAfter(HttpResponseMessage response)
    {
        var retryAfter = response.Headers.RetryAfter;

        var delay = retryAfter?.Delta ?? (retryAfter?.Date is { } date ? date - DateTimeOffset.UtcNow : null);

        if (delay is null)
        {
            return null;
        }

        return delay < TimeSpan.Zero ? TimeSpan.Zero : delay;
    }

    private string CreateBearerToken()
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(_signingKey);

        var securityKey = new ECDsaSecurityKey(ecdsa)
        {
            KeyId = _keyId,
            CryptoProviderFactory = new CryptoProviderFactory
            {
                CacheSignatureProviders = false
            }
        };

        var claims = new Dictionary<string, object>();

        if (_isIndividualKey)
        {
            claims["sub"] = IndividualKeySubject;
        }

        if (TokenScope is { Length: > 0 } scope)
        {
            claims["scope"] = scope;
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _isIndividualKey ? null : _issuerId,
            Audience = Audience,
            Expires = DateTime.UtcNow.Add(_tokenLifetime),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256),
            Claims = claims
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }

    private static string PackageVersion()
    {
        var informationalVersion = typeof(AppStoreConnectAPIClient).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (string.IsNullOrEmpty(informationalVersion))
        {
            return "0.0.0";
        }

        // A build that records the commit it came from appends "+<sha>" to the informational version.
        var metadataIndex = informationalVersion.IndexOf('+', StringComparison.Ordinal);

        return metadataIndex < 0 ? informationalVersion : informationalVersion[..metadataIndex];
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _uploadHttpClient?.Dispose();
        _uploadHttpClient = null;

        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
