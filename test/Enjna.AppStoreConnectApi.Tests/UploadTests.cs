using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class UploadTests
{
    private const string ReservationResource = "uploads.inAppPurchaseImageReservation.json";

    /// <summary>
    /// The 24 bytes the reservation fixture splits into a 10 byte part and a 14 byte part.
    /// </summary>
    private const string AssetFileText = "APPSTORECONNECTASSET-024";

    private static readonly FieldInfo UploadHttpClientField =
        typeof(AppStoreConnectAPIClient).GetField("_uploadHttpClient", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException(
            "AppStoreConnectAPIClient no longer has an _uploadHttpClient field to redirect uploads with.");

    [Fact]
    public void ComputesLowercaseHexMd5OfTheFile()
    {
        var checksum = AppStoreConnectAPIClient.ComputeSourceFileChecksum(Encoding.ASCII.GetBytes("abc"));

        Assert.Equal("900150983cd24fb0d6963f7d28e17f72", checksum);
    }

    [Fact]
    public void ComputesTheChecksumOfAnEmptyFile()
    {
        var checksum = AppStoreConnectAPIClient.ComputeSourceFileChecksum(Array.Empty<byte>());

        Assert.Equal("d41d8cd98f00b204e9800998ecf8427e", checksum);
    }

    [Fact]
    public void ComputesTheChecksumOfTheWholeUnsplitFile()
    {
        var checksum = AppStoreConnectAPIClient.ComputeSourceFileChecksum(GetAssetFileData());

        Assert.Equal("d4ecad05119394e7a5acdc44dd99bf13", checksum);
    }

    [Fact]
    public void ThrowsWhenComputingTheChecksumOfNullFileData()
    {
        Assert.Throws<ArgumentNullException>(() => AppStoreConnectAPIClient.ComputeSourceFileChecksum(null!));
    }

    [Fact]
    public async Task SendsEachUploadOperationWithItsOwnMethodAndUrl()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(2, queuesReservation: true);
        var operations = await ReserveUploadOperationsAsync(client);

        await client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken);

        uploadHandler.AssertAllResponsesConsumed();
        Assert.Equal(2, uploadHandler.CapturedRequests.Count);

        Assert.Equal(HttpMethod.Put, uploadHandler.CapturedRequests[0].Method);
        Assert.Equal(
            "https://upload.example.apple.com/assets/part-one",
            uploadHandler.CapturedRequests[0].RequestUri!.AbsoluteUri);

        Assert.Equal(HttpMethod.Put, uploadHandler.CapturedRequests[1].Method);
        Assert.Equal(
            "https://upload.example.apple.com/assets/part-two",
            uploadHandler.CapturedRequests[1].RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task SendsTheByteSliceEachUploadOperationAsksFor()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(2, queuesReservation: true);
        var operations = await ReserveUploadOperationsAsync(client);

        await client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken);

        uploadHandler.AssertAllResponsesConsumed();

        Assert.Equal("APPSTORECO", uploadHandler.CapturedRequestBodies[0]);
        Assert.Equal("NNECTASSET-024", uploadHandler.CapturedRequestBodies[1]);
        Assert.Equal(10, uploadHandler.CapturedRequests[0].Content!.Headers.ContentLength);
        Assert.Equal(14, uploadHandler.CapturedRequests[1].Content!.Headers.ContentLength);
    }

    [Fact]
    public async Task SendsTheRequestHeadersOfEachUploadOperation()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(2, queuesReservation: true);
        var operations = await ReserveUploadOperationsAsync(client);

        await client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken);

        uploadHandler.AssertAllResponsesConsumed();

        var firstPart = uploadHandler.CapturedRequests[0];
        var secondPart = uploadHandler.CapturedRequests[1];

        Assert.Equal("image/png", firstPart.Content!.Headers.ContentType!.MediaType);
        Assert.Equal("1", Assert.Single(firstPart.Headers.GetValues("x-apple-upload-part")));
        Assert.Equal("2", Assert.Single(secondPart.Headers.GetValues("x-apple-upload-part")));
    }

    [Fact]
    public async Task SendsNoAuthorizationHeaderToTheUploadUrl()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(2, queuesReservation: true);
        var operations = await ReserveUploadOperationsAsync(client);

        await client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken);

        uploadHandler.AssertAllResponsesConsumed();
        Assert.Equal(2, uploadHandler.CapturedRequests.Count);

        foreach (var request in uploadHandler.CapturedRequests)
        {
            Assert.Null(request.Headers.Authorization);
            Assert.False(request.Headers.Contains("Authorization"));
        }
    }

    [Fact]
    public async Task SendsTheWholeFileWithPutWhenAnOperationOmitsItsMethodAndRange()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(1, queuesReservation: false);

        var operations = new[]
        {
            new UploadOperation
            {
                Url = "https://upload.example.apple.com/assets/whole"
            }
        };

        await client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken);

        uploadHandler.AssertAllResponsesConsumed();

        var request = Assert.Single(uploadHandler.CapturedRequests);

        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal(AssetFileText, uploadHandler.CapturedRequestBodies[0]);
    }

    [Fact]
    public async Task ThrowsApiExceptionWhenAPartFailsToUpload()
    {
        var uploadHandler = new TestHttpMessageHandler()
            .EnqueueJson("Upload session expired.", HttpStatusCode.Forbidden);

        var client = CreateClientWithUploadHandler(uploadHandler, queuesReservation: false);

        var operations = new[]
        {
            new UploadOperation
            {
                Method = "PUT",
                Url = "https://upload.example.apple.com/assets/expired",
                Offset = 0,
                Length = 24
            }
        };

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken));

        uploadHandler.AssertAllResponsesConsumed();
        Assert.Equal(403, exception.HttpStatusCode);
        Assert.Equal("Upload session expired.", exception.ResponseBody);
    }

    [Fact]
    public async Task ThrowsWhenAnUploadOperationHasNoUrl()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(0, queuesReservation: false);

        var operations = new[]
        {
            new UploadOperation
            {
                Method = "PUT",
                Offset = 0,
                Length = 24
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken));

        Assert.Empty(uploadHandler.CapturedRequests);
    }

    [Fact]
    public async Task ThrowsWhenAnUploadOperationReachesPastTheEndOfTheFile()
    {
        var (client, uploadHandler) = CreateClientWithUploadHandler(0, queuesReservation: false);

        var operations = new[]
        {
            new UploadOperation
            {
                Method = "PUT",
                Url = "https://upload.example.apple.com/assets/too-long",
                Offset = 16,
                Length = 16
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken));

        Assert.Empty(uploadHandler.CapturedRequests);
    }

    [Fact]
    public async Task ThrowsWhenTheFileDataIsNull()
    {
        var (client, _) = CreateClientWithUploadHandler(0, queuesReservation: false);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => client.UploadAssetAsync(Array.Empty<UploadOperation>(), null!, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ReadsTheUploadOperationsOfAReservation()
    {
        var (client, _) = TestUtilities.GetClientWithJson(ReservationResource);

        var reservation = await client.GetInAppPurchaseImageV2Async(
            "d1f3a7c4-1b2e-4f5a-9c6d-7e8f9a0b1c2d",
            cancellationToken: TestContext.Current.CancellationToken);

        var attributes = reservation.Data.Attributes!;
        var operations = attributes.UploadOperations!;

        Assert.Equal("promotional-image.png", attributes.FileName);
        Assert.Equal(24L, attributes.FileSize);
        Assert.Equal(2, operations.Length);
        Assert.Equal("PUT", operations[0].Method);
        Assert.Equal(0, operations[0].Offset);
        Assert.Equal(10, operations[0].Length);
        Assert.Equal(10, operations[1].Offset);
        Assert.Equal(14, operations[1].Length);
        Assert.Equal("Content-Type", operations[1].RequestHeaders![0].Name);
        Assert.Equal("image/png", operations[1].RequestHeaders![0].Value);
    }

    private static byte[] GetAssetFileData()
    {
        return Encoding.ASCII.GetBytes(AssetFileText);
    }

    private static async Task<UploadOperation[]> ReserveUploadOperationsAsync(AppStoreConnectAPIClient client)
    {
        var reservation = await client.GetInAppPurchaseImageV2Async(
            "d1f3a7c4-1b2e-4f5a-9c6d-7e8f9a0b1c2d",
            cancellationToken: TestContext.Current.CancellationToken);

        return reservation.Data.Attributes!.UploadOperations!;
    }

    /// <summary>
    /// Builds a client whose uploads go to a separate handler that replays
    /// <paramref name="successfulParts"/> empty 200 responses. Pass <paramref name="queuesReservation"/>
    /// as <see langword="true"/> for a test that reserves through the API first, so the reservation
    /// fixture is waiting for that call.
    /// </summary>
    private static (AppStoreConnectAPIClient Client, TestHttpMessageHandler UploadHandler) CreateClientWithUploadHandler(
        int successfulParts,
        bool queuesReservation)
    {
        var uploadHandler = new TestHttpMessageHandler();

        for (var part = 0; part < successfulParts; part++)
        {
            uploadHandler.EnqueueEmpty(HttpStatusCode.OK);
        }

        return (CreateClientWithUploadHandler(uploadHandler, queuesReservation), uploadHandler);
    }

    /// <summary>
    /// Builds a client that uploads through <paramref name="uploadHandler"/>. A test that builds its
    /// own upload operations passes <paramref name="queuesReservation"/> as <see langword="false"/>,
    /// so an API call it was not supposed to make fails instead of silently draining a fixture.
    /// </summary>
    private static AppStoreConnectAPIClient CreateClientWithUploadHandler(
        TestHttpMessageHandler uploadHandler,
        bool queuesReservation)
    {
        var apiHandler = new TestHttpMessageHandler();

        if (queuesReservation)
        {
            apiHandler.EnqueueJson(TestUtilities.ReadResourceAsString(ReservationResource));
        }

        var client = TestUtilities.CreateClient(apiHandler);

        // Redirect the library's own upload client, not the one API calls go through.
        UploadHttpClientField.SetValue(client, new HttpClient(uploadHandler));

        return client;
    }

    [Fact]
    public async Task SendsNoPartWhenALaterOperationIsInvalid()
    {
        var uploadHandler = new TestHttpMessageHandler();
        var client = CreateClientWithUploadHandler(uploadHandler, queuesReservation: true);
        var operations = await ReserveUploadOperationsAsync(client);

        var invalid = new[]
        {
            operations[0],
            new UploadOperation { Url = "https://upload.example.com/part2", Offset = 0, Length = int.MaxValue }
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadAssetAsync(invalid, GetAssetFileData(), TestContext.Current.CancellationToken));

        Assert.Empty(uploadHandler.CapturedRequests);
    }

    [Theory]
    [InlineData("X-Evil", "a\r\nX-Injected: yes\r\nAuthorization: Bearer STOLEN")]
    [InlineData("X-Evil", "a\nX-Injected: yes")]
    [InlineData("X-Ev\ril", "value")]
    public async Task RejectsAnUploadHeaderCarryingAControlCharacter(string name, string value)
    {
        var uploadHandler = new TestHttpMessageHandler();
        var client = CreateClientWithUploadHandler(uploadHandler, queuesReservation: false);

        var operations = new[]
        {
            new UploadOperation
            {
                Url = "https://upload.example.apple.com/assets/part-one",
                Offset = 0,
                Length = 24,
                RequestHeaders = new[] { new UploadOperationHeader { Name = name, Value = value } }
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken));

        Assert.Empty(uploadHandler.CapturedRequests);
    }

    [Theory]
    [InlineData("PU T", "https://upload.example.apple.com/assets/part-one")]
    [InlineData("PUT", "assets/part-one")]
    [InlineData("PUT", "ftp://upload.example.apple.com/assets/part-one")]
    public async Task RejectsAnUnusableUploadMethodOrUrlBeforeSendingAnything(string method, string url)
    {
        var uploadHandler = new TestHttpMessageHandler();
        var client = CreateClientWithUploadHandler(uploadHandler, queuesReservation: true);
        var validOperations = await ReserveUploadOperationsAsync(client);

        var operations = new[]
        {
            validOperations[0],
            new UploadOperation { Method = method, Url = url, Offset = 10, Length = 14 }
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadAssetAsync(operations, GetAssetFileData(), TestContext.Current.CancellationToken));

        Assert.Empty(uploadHandler.CapturedRequests);
    }
}
