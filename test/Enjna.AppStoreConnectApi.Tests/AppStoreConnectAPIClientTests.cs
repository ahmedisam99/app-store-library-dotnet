using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class AppStoreConnectAPIClientTests
{
    private const string AppsResource = "client.appsResponse.json";
    private const string ErrorResource = "client.errorResponse.json";
    /// <summary>
    /// The expected <c>User-Agent</c>, built from the library assembly's own version. Asserting a
    /// literal version here would go stale the moment the package version is bumped, and pass
    /// anyway, which is how the sibling library shipped a wrong <c>User-Agent</c>.
    /// </summary>
    private static readonly string UserAgent =
        "enjna-app-store-connect-api/dotnet/" + GetLibraryPackageVersion();
    private const string RateLimitHeader = "user-hour-lim:3600;user-hour-rem:3572;";

    [Fact]
    // ReSharper disable once InconsistentNaming
    public async Task SignsBearerTokenWithES256HeaderNamingTheKeyId()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(AppsResource);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var authorization = handler.CapturedRequest!.Headers.Authorization;
        Assert.NotNull(authorization);
        Assert.Equal("Bearer", authorization.Scheme);

        var header = ReadTokenHeader(handler);
        Assert.Equal("ES256", header.GetProperty("alg").GetString());
        Assert.Equal(TestUtilities.KeyId, header.GetProperty("kid").GetString());
        Assert.Equal("JWT", header.GetProperty("typ").GetString());
    }

    [Fact]
    public async Task SignsBearerTokenWithIssuerAndAppStoreConnectAudience()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(AppsResource);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var payload = ReadTokenPayload(handler);
        Assert.Equal(TestUtilities.IssuerId, payload.GetProperty("iss").GetString());
        Assert.Equal("appstoreconnect-v1", payload.GetProperty("aud").GetString());
        Assert.False(payload.TryGetProperty("sub", out _));
        Assert.False(payload.TryGetProperty("scope", out _));

        var issuedAt = payload.GetProperty("iat").GetInt64();
        var expiresAt = payload.GetProperty("exp").GetInt64();
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Assert.InRange(issuedAt, now - 60, now + 5);
        Assert.Equal(900, expiresAt - issuedAt);
    }

    [Fact]
    public async Task SignsBearerTokenWithTheConfiguredTokenLifetime()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(AppsResource);

        client.TokenLifetime = TimeSpan.FromMinutes(3);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var payload = ReadTokenPayload(handler);
        Assert.Equal(180, payload.GetProperty("exp").GetInt64() - payload.GetProperty("iat").GetInt64());
    }

    [Fact]
    public void RejectsTokenLifetimesOutsideApplesLimits()
    {
        using var client = TestUtilities.CreateClient(new TestHttpMessageHandler());

        Assert.Throws<ArgumentOutOfRangeException>(() => client.TokenLifetime = TimeSpan.Zero);
        Assert.Throws<ArgumentOutOfRangeException>(() => client.TokenLifetime = TimeSpan.FromMinutes(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => client.TokenLifetime = TimeSpan.FromMinutes(20.5));

        client.TokenLifetime = TimeSpan.FromMinutes(20);
        Assert.Equal(TimeSpan.FromMinutes(20), client.TokenLifetime);

        Assert.Throws<ArgumentOutOfRangeException>(() => client.TokenLifetime = TimeSpan.FromHours(1));
        Assert.Equal(TimeSpan.FromMinutes(20), client.TokenLifetime);
    }

    [Fact]
    public async Task SignsIndividualKeyTokenWithSubjectAndNoIssuer()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString(AppsResource));

        using var client = AppStoreConnectAPIClient.ForIndividualKey(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            new HttpClient(handler));

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var payload = ReadTokenPayload(handler);
        Assert.Equal("user", payload.GetProperty("sub").GetString());
        Assert.False(payload.TryGetProperty("iss", out _));
        Assert.Equal("appstoreconnect-v1", payload.GetProperty("aud").GetString());
    }

    [Fact]
    public async Task AddsTheScopeClaimWhenATokenScopeIsSet()
    {
        var body = TestUtilities.ReadResourceAsString(AppsResource);
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(body).EnqueueJson(body).EnqueueJson(body);

        using var client = TestUtilities.CreateClient(handler);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        var unscopedToken = ReadToken(handler, 0);

        client.TokenScope = ["GET /v1/apps?filter[platform]=IOS", "GET /v1/apps/*"];

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        var scopedToken = ReadToken(handler, 1);

        Assert.NotEqual(unscopedToken, scopedToken);

        var scope = ReadTokenPayload(handler, 1)
            .GetProperty("scope")
            .EnumerateArray()
            .Select(entry => entry.GetString()!)
            .ToArray();

        Assert.Equal(new[] { "GET /v1/apps?filter[platform]=IOS", "GET /v1/apps/*" }, scope);

        client.TokenScope = null;

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotEqual(scopedToken, ReadToken(handler, 2));
        Assert.False(ReadTokenPayload(handler, 2).TryGetProperty("scope", out _));
    }

    private static string GetLibraryPackageVersion()
    {
        var informationalVersion = typeof(AppStoreConnectAPIClient).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        Assert.False(string.IsNullOrEmpty(informationalVersion));

        var metadataIndex = informationalVersion.IndexOf('+', StringComparison.Ordinal);

        return metadataIndex < 0 ? informationalVersion : informationalVersion[..metadataIndex];
    }

    [Fact]
    public async Task SendsUserAgentAndJsonAcceptHeaders()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(AppsResource);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Contains(UserAgent, request.Headers.GetValues("User-Agent"));
        Assert.Equal("application/json", request.Headers.Accept.ToString());
    }

    [Fact]
    public async Task SendsRequestsToTheAppStoreConnectApiByDefault()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(AppsResource);

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/apps",
            handler.CapturedRequest.RequestUri!.ToString());
        Assert.Equal("6446901002", response.Data[0].Id);
    }

    [Fact]
    public async Task SendsRequestsToACustomBaseUrl()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString(AppsResource));

        using var client = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId,
            new HttpClient(handler),
            "https://connect.example.com/proxy/");

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://connect.example.com/proxy/v1/apps",
            handler.CapturedRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ThrowsApiExceptionWithErrorDetails()
    {
        var (client, _) = TestUtilities.GetClientWithJson(ErrorResource, HttpStatusCode.NotFound);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(404, exception.HttpStatusCode);
        Assert.NotNull(exception.Errors);
        Assert.Single(exception.Errors);

        var error = exception.Errors[0];
        Assert.Equal("NOT_FOUND", error.Code);
        Assert.Equal("404", error.Status);
        Assert.Equal("The specified resource does not exist", error.Title);
        Assert.Equal("There is no resource of type 'apps' with id '6446901002'", error.Detail);
        Assert.Equal("filter[bundleId]", error.Source!.Parameter);
        Assert.Null(error.Source.Pointer);

        Assert.Equal("There is no resource of type 'apps' with id '6446901002'", exception.Message);
    }

    [Fact]
    public async Task ThrowsApiExceptionForAnUnparseableErrorBody()
    {
        const string body = "<html><head><title>502 Bad Gateway</title></head></html>";

        var (client, _) = TestUtilities.GetClientWithBody(body, HttpStatusCode.InternalServerError);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(500, exception.HttpStatusCode);
        Assert.Null(exception.Errors);
        Assert.Equal(body, exception.ResponseBody);
        Assert.Equal("App Store Connect API error: HTTP 500", exception.Message);

        Assert.Null(exception.InnerException);
    }

    [Fact]
    public async Task ReadsTheRateLimitHeaderOfASuccessfulResponse()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString(AppsResource)).WithRateLimit(RateLimitHeader);

        using var client = TestUtilities.CreateClient(handler);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var rateLimit = client.LastRateLimit;
        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3572, rateLimit.UserHourRemaining);
        Assert.Equal(RateLimitHeader, rateLimit.Raw);
    }

    [Fact]
    public async Task ReadsTheRateLimitHeaderOfAnErrorResponse()
    {
        var handler = new TestHttpMessageHandler();
        handler
            .EnqueueJson(TestUtilities.ReadResourceAsString(ErrorResource), HttpStatusCode.NotFound)
            .WithRateLimit(RateLimitHeader);

        using var client = TestUtilities.CreateClient(handler);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.NotNull(exception.RateLimit);
        Assert.Equal(3600, exception.RateLimit.UserHourLimit);
        Assert.Equal(3572, exception.RateLimit.UserHourRemaining);

        Assert.NotNull(client.LastRateLimit);
        Assert.Equal(3572, client.LastRateLimit.UserHourRemaining);
    }

    [Fact]
    public async Task ClearsTheRateLimitWhenTheHeaderIsAbsent()
    {
        var body = TestUtilities.ReadResourceAsString(AppsResource);
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(body).WithRateLimit(RateLimitHeader).EnqueueJson(body);

        using var client = TestUtilities.CreateClient(handler);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(client.LastRateLimit);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        Assert.Null(client.LastRateLimit);
    }

    [Fact]
    public void DisposesTheHttpClientItOwns()
    {
        var client = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId);

        var ownedHttpClient = GetHttpClient(client);

        client.Dispose();

        Assert.Throws<ObjectDisposedException>(() => ownedHttpClient.Timeout = TimeSpan.FromSeconds(30));
    }

    [Fact]
    public async Task LeavesACallerSuppliedHttpClientUsable()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString(AppsResource));

        using var httpClient = new HttpClient(handler);

        var client = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId,
            httpClient);

        client.Dispose();

        httpClient.Timeout = TimeSpan.FromSeconds(30);

        using var second = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId,
            httpClient);

        var response = await second.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("6446901002", response.Data[0].Id);
    }

    [Fact]
    public void DisposesIdempotently()
    {
        var client = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId);

        client.Dispose();
        client.Dispose();
    }

    private static string ReadToken(TestHttpMessageHandler handler, int requestIndex)
    {
        var authorization = handler.CapturedRequests[requestIndex].Headers.Authorization;
        Assert.NotNull(authorization);

        return authorization.Parameter!;
    }

    private static JsonElement ReadTokenHeader(TestHttpMessageHandler handler, int requestIndex = 0)
    {
        return ReadTokenSegment(handler, requestIndex, 0);
    }

    private static JsonElement ReadTokenPayload(TestHttpMessageHandler handler, int requestIndex = 0)
    {
        return ReadTokenSegment(handler, requestIndex, 1);
    }

    private static JsonElement ReadTokenSegment(TestHttpMessageHandler handler, int requestIndex, int segmentIndex)
    {
        var parts = ReadToken(handler, requestIndex).Split('.');
        Assert.Equal(3, parts.Length);

        using var document = JsonDocument.Parse(Base64UrlDecode(parts[segmentIndex]));

        return document.RootElement.Clone();
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var base64 = input.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }

    private static HttpClient GetHttpClient(AppStoreConnectAPIClient client)
    {
        var field = typeof(AppStoreConnectAPIClient).GetField(
            "_httpClient",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(field);

        return Assert.IsType<HttpClient>(field.GetValue(client));
    }

    [Fact]
    public async Task ThrowsAJsonExceptionRatherThanAnApiExceptionForAMalformedSuccessBody()
    {
        var (client, _) = TestUtilities.GetClientWithBody("<html>not json</html>");

        await Assert.ThrowsAsync<JsonException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.Created)]
    [InlineData(HttpStatusCode.NoContent)]
    public async Task NeverReportsASuccessStatusAsAnApiException(HttpStatusCode statusCode)
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(string.Empty, statusCode);

        using var client = TestUtilities.CreateClient(handler);

        var exception = await Record.ExceptionAsync(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.NotNull(exception);
        Assert.IsNotType<APIException>(exception);
    }

    [Fact]
    public async Task ReadsTheRetryAfterDelayFromASecondsHeader()
    {
        var response = new HttpResponseMessage((HttpStatusCode)429)
        {
            Content = new StringContent(TestUtilities.ReadResourceAsString(ErrorResource))
        };

        response.Headers.TryAddWithoutValidation("Retry-After", "37");

        var handler = new TestHttpMessageHandler();
        handler.Enqueue(response);

        using var client = TestUtilities.CreateClient(handler);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(429, exception.HttpStatusCode);
        Assert.Equal(TimeSpan.FromSeconds(37), exception.RetryAfter);
    }

    [Fact]
    public async Task ReadsTheRetryAfterDelayFromADateHeader()
    {
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent(TestUtilities.ReadResourceAsString(ErrorResource))
        };

        response.Headers.TryAddWithoutValidation(
            "Retry-After",
            DateTimeOffset.UtcNow.AddMinutes(2).ToString("R"));

        var handler = new TestHttpMessageHandler();
        handler.Enqueue(response);

        using var client = TestUtilities.CreateClient(handler);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.NotNull(exception.RetryAfter);
        Assert.InRange(exception.RetryAfter.Value, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(120));
    }

    [Fact]
    public async Task ReportsNoRetryAfterWhenTheResponseCarriesNoSuchHeader()
    {
        var (client, _) = TestUtilities.GetClientWithJson(ErrorResource, HttpStatusCode.NotFound);

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken));

        Assert.Null(exception.RetryAfter);
    }

    [Fact]
    public void RefusesToSerializeTheUnmappedEnumSentinel()
    {
        var decoded = JsonSerializer.Deserialize<AppAttributes>(
            "{\"contentRightsDeclaration\":\"A_VALUE_APPLE_ADDED_LATER\"}");

        Assert.Equal(AppContentRightsDeclaration._Unmapped, decoded!.ContentRightsDeclaration);

        Assert.Throws<JsonException>(() => JsonSerializer.Serialize(
            new AppUpdateRequestDataAttributes { ContentRightsDeclaration = decoded.ContentRightsDeclaration }));
    }

    [Fact]
    public void SendsAnExplicitNullToClearAToOneRelationship()
    {
        var json = JsonSerializer.Serialize(
            new BuildUpdateRequestDataRelationships
            {
                AppEncryptionDeclaration = new RelationshipDeclaration { Data = null }
            },
            new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

        Assert.Equal("{\"appEncryptionDeclaration\":{\"data\":null}}", json);
    }

    [Fact]
    public void ReportsANonStringEnumTokenAsAJsonException()
    {
        var exception = Record.Exception(
            () => JsonSerializer.Deserialize<AppAttributes>("{\"contentRightsDeclaration\":5}"));

        Assert.IsType<JsonException>(exception);
    }

    [Fact]
    public void ThrowsWhenARelationshipFactoryIsGivenNull()
    {
        Assert.Throws<ArgumentNullException>(() => RelationshipDeclaration.To("apps", null!));
        Assert.Throws<ArgumentNullException>(() => RelationshipDeclarationList.To("builds", null!));
    }
}
