using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Nodes;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

internal static class TestUtilities
{
    private const string ResourcePrefix = "Enjna.AppStoreConnectApi.Tests.Resources.";

    public const string KeyId = "testKeyId";

    public const string IssuerId = "99b16628-15e4-4668-972b-eeff55eeff55";

    public static string ReadResourceAsString(string path)
    {
        var resourceName = ResourcePrefix + path;
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream is null)
        {
            throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static byte[] ReadResourceAsBytes(string path)
    {
        var resourceName = ResourcePrefix + path;
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream is null)
        {
            throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        }

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public static string GetSigningKey()
    {
        return ReadResourceAsString("certs.testSigningKey.p8");
    }

    public static (AppStoreConnectAPIClient Client, TestHttpMessageHandler Handler) GetClientWithJson(
        string? resourcePath,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var handler = new TestHttpMessageHandler();
        if (resourcePath is not null)
        {
            handler.EnqueueJson(ReadResourceAsString(resourcePath), statusCode);
        }
        else
        {
            handler.EnqueueEmpty(statusCode);
        }

        return (CreateClient(handler), handler);
    }

    public static (AppStoreConnectAPIClient Client, TestHttpMessageHandler Handler) GetClientWithBody(
        string body,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(body, statusCode);
        return (CreateClient(handler), handler);
    }

    public static AppStoreConnectAPIClient CreateClient(TestHttpMessageHandler handler)
    {
        return new AppStoreConnectAPIClient(
            GetSigningKey(),
            KeyId,
            IssuerId,
            new HttpClient(handler));
    }

    /// <summary>
    /// Asserts that a captured request body is the same JSON as the expected one, ignoring member
    /// order and whitespace, so a test can compare against Apple's example payloads verbatim.
    /// </summary>
    public static void AssertJsonEquivalent(string expected, string? actual)
    {
        Assert.NotNull(actual);

        var expectedNode = JsonNode.Parse(expected);
        var actualNode = JsonNode.Parse(actual);

        Assert.True(
            JsonNode.DeepEquals(expectedNode, actualNode),
            $"Expected {expectedNode!.ToJsonString()} but the request sent {actualNode!.ToJsonString()}.");
    }
}
