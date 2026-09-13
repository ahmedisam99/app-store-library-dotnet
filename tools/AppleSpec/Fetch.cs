using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppleSpec;

public static class AppleDocs
{
    private const string UserAgent =
        "AppleSpec/1.0 (+https://github.com/ahmedisam99/app-store-library-dotnet; refreshes a vendored snapshot of Apple's documentation)";

    private const int MaxAttempts = 4;

    // Every request in a sync passes through one gate, so running the frameworks together cannot
    // multiply the ceiling by the number of them. Measured against Apple: 8 at a time reaches
    // 11.7 pages a second, 24 reaches 45.8 and 32 reaches 72.1, with no throttling at any of them.
    // 24 is the politeness ceiling rather than the fast one; SendAsync backs off if that is wrong.
    public const int MaxInFlight = 24;

    private static readonly SemaphoreSlim Gate = new(MaxInFlight);

    private static readonly HttpClient Client = CreateClient();

    public static string IndexUrl(string framework) => $"https://developer.apple.com/tutorials/data/index/{framework}";

    public static string PageUrl(string slug) => $"https://developer.apple.com/tutorials/data/documentation/{slug}.json";

    // The navigator tree repeats a page once per containment path and links across
    // frameworks, so it is neither unique nor confined to the framework asked for.
    public static async Task<IReadOnlyList<string>> ListSlugsAsync(string framework, CancellationToken cancellationToken)
    {
        using var index = await GetJsonAsync(IndexUrl(framework), cancellationToken).ConfigureAwait(false);

        if (!index.RootElement.TryGetProperty("interfaceLanguages", out var languages)
            || !languages.TryGetProperty("data", out var roots)
            || roots.ValueKind != JsonValueKind.Array)
        {
            throw new SpecFormatException($"The navigator index for {framework} has no interfaceLanguages.data array of nodes.");
        }

        // Apple reshuffles the navigator for editorial reasons, so tree order is not stable.
        var slugs = new SortedSet<string>(StringComparer.Ordinal);

        foreach (var node in roots.EnumerateArray())
        {
            Collect(node, framework, slugs, cancellationToken);
        }

        if (slugs.Count == 0)
        {
            throw new SpecFormatException($"The navigator index for {framework} listed no pages under /documentation/{framework}.");
        }

        return slugs.ToList();
    }

    public static Task<JsonDocument> GetPageAsync(string slug, CancellationToken cancellationToken) =>
        GetJsonAsync(PageUrl(slug), cancellationToken);

    public static async Task<HttpMeta> HeadAsync(string url, CancellationToken cancellationToken)
    {
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            using var response = await SendAsync(HttpMethod.Head, url, cancellationToken).ConfigureAwait(false);

            return new HttpMeta(
                Header(response, "Last-Modified"),
                Header(response, "ETag"),
                response.Content.Headers.ContentLength);
        }
        finally
        {
            Gate.Release();
        }
    }

    public static async Task<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken)
    {
        // The slot is held until the body is read, not just until the headers arrive: responses come
        // back with ResponseHeadersRead, so releasing earlier would leave the download uncounted.
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            using var response = await SendAsync(HttpMethod.Get, url, cancellationToken).ConfigureAwait(false);

            return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            Gate.Release();
        }
    }

    private static async Task<JsonDocument> GetJsonAsync(string url, CancellationToken cancellationToken)
    {
        var body = await GetBytesAsync(url, cancellationToken).ConfigureAwait(false);

        try
        {
            return JsonDocument.Parse(body);
        }
        catch (JsonException ex)
        {
            throw new SpecFormatException($"{url} did not return JSON: {ex.Message}");
        }
    }

    private static void Collect(JsonElement node, string framework, SortedSet<string> slugs, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Group markers are the only navigator nodes that carry no path.
        if (node.TryGetProperty("path", out var path) && path.ValueKind == JsonValueKind.String)
        {
            var value = path.GetString() ?? "";

            if (value == $"/documentation/{framework}" || value.StartsWith($"/documentation/{framework}/", StringComparison.Ordinal))
            {
                slugs.Add(value["/documentation/".Length..]);
            }
        }

        if (node.TryGetProperty("children", out var children) && children.ValueKind == JsonValueKind.Array)
        {
            foreach (var child in children.EnumerateArray())
            {
                Collect(child, framework, slugs, cancellationToken);
            }
        }
    }

    private static async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            HttpResponseMessage response;

            try
            {
                using var request = new HttpRequestMessage(method, url);
                response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (attempt < MaxAttempts)
            {
                await BackOffAsync(attempt, cancellationToken).ConfigureAwait(false);
                continue;
            }
            catch (TaskCanceledException) when (attempt < MaxAttempts && !cancellationToken.IsCancellationRequested)
            {
                // A timeout, not the user: HttpClient reports both this way.
                await BackOffAsync(attempt, cancellationToken).ConfigureAwait(false);
                continue;
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var status = response.StatusCode;
            response.Dispose();

            if (attempt < MaxAttempts && IsTransient(status))
            {
                await BackOffAsync(attempt, cancellationToken).ConfigureAwait(false);
                continue;
            }

            throw new HttpRequestException($"{method} {url} returned {(int)status} {status}.");
        }
    }

    private static bool IsTransient(HttpStatusCode status) =>
        status is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests || (int)status >= 500;

    private static Task BackOffAsync(int attempt, CancellationToken cancellationToken) =>
        Task.Delay(TimeSpan.FromMilliseconds(500 * Math.Pow(2, attempt - 1)), cancellationToken);

    private static string? Header(HttpResponseMessage response, string name)
    {
        if (response.Headers.TryGetValues(name, out var values) || response.Content.Headers.TryGetValues(name, out values))
        {
            return values.FirstOrDefault();
        }

        return null;
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient(new SocketsHttpHandler
        {
            // Apple serves this over HTTP/1.1, so a connection is a request: anything below the
            // gate would make the gate a fiction.
            MaxConnectionsPerServer = MaxInFlight,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            AutomaticDecompression = DecompressionMethods.All
        })
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

        return client;
    }
}

public readonly record struct HttpMeta(string? LastModified, string? ETag, long? ContentLength);
