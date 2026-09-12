using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreServerLibrary.Models;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Xunit;

namespace Enjna.AppStoreServerLibrary.Tests;

public class AppStoreServerAPIClientTests
{
    private const string KeyId = "testKeyId";
    private const string IssuerId = "testIssuerId";
    private const string BundleId = "com.example";

    private sealed class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string? _responseBody;

        public HttpRequestMessage? CapturedRequest { get; private set; }
        public string? CapturedRequestBody { get; private set; }

        public TestHttpMessageHandler(HttpStatusCode statusCode, string? responseBody)
        {
            _statusCode = statusCode;
            _responseBody = responseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CapturedRequest = request;
            if (request.Content is not null)
            {
                CapturedRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            }

            var response = new HttpResponseMessage(_statusCode);
            if (_responseBody is not null)
            {
                response.Content = new StringContent(_responseBody, Encoding.UTF8, "application/json");
            }

            return response;
        }
    }

    private static (AppStoreServerAPIClient Client, TestHttpMessageHandler Handler) GetClientWithBody(
        string? resourcePath, HttpStatusCode statusCode)
    {
        var body = resourcePath is not null ? TestUtilities.ReadResourceAsString(resourcePath) : null;
        var handler = new TestHttpMessageHandler(statusCode, body);
        var httpClient = new HttpClient(handler);
        var signingKey = TestUtilities.GetSigningKey();
        var client = new AppStoreServerAPIClient(signingKey, KeyId, IssuerId, AppStoreEnvironment.LocalTesting, httpClient);
        return (client, handler);
    }

    private static void AssertCommonHeaders(TestHttpMessageHandler handler)
    {
        var request = handler.CapturedRequest!;

        Assert.Contains("enjna-app-store-server-library/dotnet/2.2.0", request.Headers.GetValues("User-Agent"));
        Assert.Contains("application/json", request.Headers.Accept.ToString());

        var auth = request.Headers.Authorization;
        Assert.NotNull(auth);
        Assert.Equal("Bearer", auth.Scheme);
        Assert.NotNull(auth.Parameter);

        // Decode and verify JWT structure
        var parts = auth.Parameter.Split('.');
        Assert.Equal(3, parts.Length);

        var headerJson = Base64UrlDecode(parts[0]);
        using var header = JsonDocument.Parse(headerJson);
        Assert.Equal("ES256", header.RootElement.GetProperty("alg").GetString());
        Assert.Equal(KeyId, header.RootElement.GetProperty("kid").GetString());

        var payloadJson = Base64UrlDecode(parts[1]);
        using var payload = JsonDocument.Parse(payloadJson);
        Assert.Equal(BundleId, payload.RootElement.GetProperty("bid").GetString());
        Assert.Equal("appstoreconnect-v1", payload.RootElement.GetProperty("aud").GetString());
        Assert.Equal(IssuerId, payload.RootElement.GetProperty("iss").GetString());
    }

    private static string GetBearerBundleId(TestHttpMessageHandler handler)
    {
        var auth = handler.CapturedRequest!.Headers.Authorization!;
        var parts = auth.Parameter!.Split('.');
        var payloadJson = Base64UrlDecode(parts[1]);
        using var payload = JsonDocument.Parse(payloadJson);
        return payload.RootElement.GetProperty("bid").GetString()!;
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

    // 1. ExtendRenewalDateForAllActiveSubscribersAsync
    [Fact]
    public async Task ExtendRenewalDateForAllActiveSubscribers()
    {
        var (client, handler) = GetClientWithBody(
            "models.extendRenewalDateForAllActiveSubscribersResponse.json", HttpStatusCode.OK);

        var request = new MassExtendRenewalDateRequest
        {
            RequestIdentifier = "test-request-id",
            ExtendByDays = 45,
            ExtendReasonCode = ExtendReasonCode.CustomerSatisfaction,
            ProductId = "com.example.product",
            StorefrontCountryCodes = ["USA", "MEX"]
        };

        var response = await client.ExtendRenewalDateForAllActiveSubscribersAsync(request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/subscriptions/extend/mass", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("758883e8-151b-47b7-abd0-60c4d804c2f5", response.RequestIdentifier);
    }

    // 2. ExtendSubscriptionRenewalDateAsync
    [Fact]
    public async Task ExtendSubscriptionRenewalDate()
    {
        var (client, handler) = GetClientWithBody(
            "models.extendSubscriptionRenewalDateResponse.json", HttpStatusCode.OK);

        var request = new ExtendRenewalDateRequest
        {
            RequestIdentifier = "test-id",
            ExtendByDays = 45,
            ExtendReasonCode = ExtendReasonCode.CustomerSatisfaction
        };

        var response = await client.ExtendSubscriptionRenewalDateAsync("4124214", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/subscriptions/extend/4124214", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("2312412", response.OriginalTransactionId);
        Assert.Equal("9993", response.WebOrderLineItemId);
        Assert.True(response.Success);
        Assert.Equal(1698148900000L, response.EffectiveDate);
    }

    // 3. GetAllSubscriptionStatusesAsync
    [Fact]
    public async Task GetAllSubscriptionStatuses()
    {
        var (client, handler) = GetClientWithBody(
            "models.getAllSubscriptionStatusesResponse.json", HttpStatusCode.OK);

        var response = await client.GetAllSubscriptionStatusesAsync("4321", BundleId, [Status.Active, Status.Revoked],
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/subscriptions/4321", handler.CapturedRequest.RequestUri!.AbsolutePath);
        var query = handler.CapturedRequest.RequestUri.Query;
        Assert.Contains("status=1", query);
        Assert.Contains("status=5", query);
        Assert.Equal(AppStoreEnvironment.LocalTesting, response.Environment);
        Assert.Equal("com.example", response.BundleId);
        Assert.Equal(5454545L, response.AppAppleId);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Length);
    }

    // 4. GetRefundHistoryAsync
    [Fact]
    public async Task GetRefundHistory()
    {
        var (client, handler) = GetClientWithBody(
            "models.getRefundHistoryResponse.json", HttpStatusCode.OK);

        var response = await client.GetRefundHistoryAsync("555555", BundleId, "revision_input",
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v2/refund/lookup/555555", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Contains("revision=revision_input", handler.CapturedRequest.RequestUri.Query);
        Assert.True(response.HasMore);
        Assert.Equal("revision_output", response.Revision);
        Assert.NotNull(response.SignedTransactions);
        Assert.Equal(2, response.SignedTransactions.Length);
    }

    // 5. GetStatusOfSubscriptionRenewalDateExtensionsAsync
    [Fact]
    public async Task GetStatusOfSubscriptionRenewalDateExtensions()
    {
        var (client, handler) = GetClientWithBody(
            "models.getStatusOfSubscriptionRenewalDateExtensionsResponse.json", HttpStatusCode.OK);

        var response = await client.GetStatusOfSubscriptionRenewalDateExtensionsAsync("com.example.product", "test-id", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Contains("/inApps/v1/subscriptions/extend/mass/", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("20fba8a0-2b80-4a7d-a17f-85c1854727f8", response.RequestIdentifier);
        Assert.True(response.Complete);
        Assert.Equal(1698148900000L, response.CompleteDate);
        Assert.Equal(30L, response.SucceededCount);
        Assert.Equal(2L, response.FailedCount);
    }

    // 6. GetTestNotificationStatusAsync
    [Fact]
    public async Task GetTestNotificationStatus()
    {
        var (client, handler) = GetClientWithBody(
            "models.getTestNotificationStatusResponse.json", HttpStatusCode.OK);

        var response = await client.GetTestNotificationStatusAsync("test-token", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/notifications/test/test-token", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("signed_payload", response.SignedPayload);
        Assert.NotNull(response.SendAttempts);
        Assert.Equal(2, response.SendAttempts.Length);
    }

    // 7. GetNotificationHistoryAsync
    [Fact]
    public async Task GetNotificationHistory()
    {
        var (client, handler) = GetClientWithBody(
            "models.getNotificationHistoryResponse.json", HttpStatusCode.OK);

        var request = new NotificationHistoryRequest
        {
            StartDate = 1698148800000,
            EndDate = 1698148900000
        };

        var response = await client.GetNotificationHistoryAsync(request, BundleId, "test-pagination-token",
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/notifications/history", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Contains("paginationToken=test-pagination-token", handler.CapturedRequest.RequestUri.Query);
        Assert.Equal("57715481-805a-4283-8499-1c19b5d6b20a", response.PaginationToken);
        Assert.True(response.HasMore);
        Assert.NotNull(response.NotificationHistory);
        Assert.Equal(2, response.NotificationHistory.Length);
    }

    // 8. GetTransactionHistoryAsync
    [Fact]
    public async Task GetTransactionHistory()
    {
        var (client, handler) = GetClientWithBody(
            "models.transactionHistoryResponse.json", HttpStatusCode.OK);

        var request = new TransactionHistoryRequest
        {
            StartDate = 1698148800000,
            EndDate = 1698148900000,
            ProductIds = ["com.example.product"],
            ProductTypes = [ProductType.AutoRenewable],
            Sort = SortOrder.Ascending,
            SubscriptionGroupIdentifiers = ["sub_group_1"],
            InAppOwnershipType = InAppOwnershipType.Purchased,
            Revoked = false,
            Revision = "revision_input"
        };

        var response = await client.GetTransactionHistoryAsync("999999", BundleId, request,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v2/history/999999", handler.CapturedRequest.RequestUri!.AbsolutePath);
        var query = handler.CapturedRequest.RequestUri.Query;
        Assert.Contains("revision=revision_input", query);
        Assert.Contains("startDate=1698148800000", query);
        Assert.Contains("endDate=1698148900000", query);
        Assert.Equal("revision_output", response.Revision);
        Assert.True(response.HasMore);
        Assert.Equal("com.example", response.BundleId);
        Assert.Equal(323232L, response.AppAppleId);
        Assert.Equal(AppStoreEnvironment.LocalTesting, response.Environment);
        Assert.NotNull(response.SignedTransactions);
        Assert.Equal(2, response.SignedTransactions.Length);
    }

    // 9. GetTransactionInfoAsync
    [Fact]
    public async Task GetTransactionInfo()
    {
        var (client, handler) = GetClientWithBody(
            "models.transactionInfoResponse.json", HttpStatusCode.OK);

        var response =
            await client.GetTransactionInfoAsync("999999", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/transactions/999999", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("signed_transaction_info_value", response.SignedTransactionInfo);
    }

    // 10. LookUpOrderIdAsync
    [Fact]
    public async Task LookUpOrderId()
    {
        var (client, handler) = GetClientWithBody(
            "models.lookupOrderIdResponse.json", HttpStatusCode.OK);

        var response =
            await client.LookUpOrderIdAsync("W002182", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/lookup/W002182", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal(OrderLookupStatus.Invalid, response.Status);
        Assert.NotNull(response.SignedTransactions);
        Assert.Equal(2, response.SignedTransactions.Length);
    }

    // 11. RequestTestNotificationAsync
    [Fact]
    public async Task RequestTestNotification()
    {
        var (client, handler) = GetClientWithBody(
            "models.requestTestNotificationResponse.json", HttpStatusCode.OK);

        var response =
            await client.RequestTestNotificationAsync(BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/notifications/test", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("ce3af791-365e-4c60-841b-1674b43c1609", response.TestNotificationToken);
    }

    // 12. SendConsumptionInformationAsync
    [Fact]
    public async Task SendConsumptionInformation()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new ConsumptionRequest
        {
            CustomerConsented = true,
            ConsumptionPercentage = 50000,
            DeliveryStatus = DeliveryStatus.Delivered,
            RefundPreference = RefundPreference.Decline,
            SampleContentProvided = false
        };

        await client.SendConsumptionInformationAsync("49571273", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v2/transactions/consumption/49571273", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        var root = body.RootElement;
        Assert.True(root.GetProperty("customerConsented").GetBoolean());
        Assert.Equal(50000, root.GetProperty("consumptionPercentage").GetInt32());
        Assert.Equal("DELIVERED", root.GetProperty("deliveryStatus").GetString());
        Assert.Equal("DECLINE", root.GetProperty("refundPreference").GetString());
        Assert.False(root.GetProperty("sampleContentProvided").GetBoolean());
    }

    // 12b. SendConsumptionInformationAsync with only required fields
    [Fact]
    public async Task SendConsumptionInformation_OnlyRequiredFields()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new ConsumptionRequest
        {
            CustomerConsented = true,
            DeliveryStatus = DeliveryStatus.UndeliveredQualityIssue,
            SampleContentProvided = true
        };

        await client.SendConsumptionInformationAsync("49571273", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v2/transactions/consumption/49571273", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        var root = body.RootElement;
        Assert.True(root.GetProperty("customerConsented").GetBoolean());
        Assert.Equal("UNDELIVERED_QUALITY_ISSUE", root.GetProperty("deliveryStatus").GetString());
        Assert.True(root.GetProperty("sampleContentProvided").GetBoolean());
        Assert.Equal(JsonValueKind.Null, root.GetProperty("consumptionPercentage").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("refundPreference").ValueKind);
    }

    // 13. SetAppAccountTokenAsync
    [Fact]
    public async Task SetAppAccountToken()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new UpdateAppAccountTokenRequest
        {
            AppAccountToken = "7e3fb20b-4cdb-47cc-936d-99d65f608138"
        };

        await client.SetAppAccountTokenAsync("12345", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/transactions/12345/appAccountToken", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 14. GetImageListAsync
    [Fact]
    public async Task GetImageList()
    {
        var (client, handler) = GetClientWithBody(
            "models.getImageListResponse.json", HttpStatusCode.OK);

        var response = await client.GetImageListAsync(BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/image/list", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.ImageIdentifiers);
        Assert.Single(response.ImageIdentifiers);
        Assert.Equal("a1b2c3d4-e5f6-7890-a1b2-c3d4e5f67890", response.ImageIdentifiers[0].ImageIdentifier);
        Assert.Equal(ImageState.Approved, response.ImageIdentifiers[0].ImageState);
    }

    // 15. UploadImageAsync
    [Fact]
    public async Task UploadImage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.UploadImageAsync("test-image-id", [0x89, 0x50, 0x4E, 0x47], BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/image/test-image-id", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 16. DeleteImageAsync
    [Fact]
    public async Task DeleteImage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.DeleteImageAsync("test-image-id", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/image/test-image-id", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 17. GetMessageListAsync
    [Fact]
    public async Task GetMessageList()
    {
        var (client, handler) = GetClientWithBody(
            "models.getMessageListResponse.json", HttpStatusCode.OK);

        var response = await client.GetMessageListAsync(BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/message/list", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.MessageIdentifiers);
        Assert.Single(response.MessageIdentifiers);
        Assert.Equal("a1b2c3d4-e5f6-7890-a1b2-c3d4e5f67890", response.MessageIdentifiers[0].MessageIdentifier);
        Assert.Equal(MessageState.Approved, response.MessageIdentifiers[0].MessageState);
    }

    // 18. UploadMessageAsync
    [Fact]
    public async Task UploadMessage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new UploadMessageRequestBody
        {
            Header = "Test Header",
            Body = "Test Body"
        };

        await client.UploadMessageAsync("test-message-id", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/message/test-message-id", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 18b. UploadMessageAsync with image
    [Fact]
    public async Task UploadMessage_WithImage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new UploadMessageRequestBody
        {
            Header = "Header text",
            Body = "Body text",
            Image = new UploadMessageImage
            {
                ImageIdentifier = "b2c3d4e5-f6a7-8901-b2c3-d4e5f6a78901",
                AltText = "Alt text"
            }
        };

        await client.UploadMessageAsync("a1b2c3d4-e5f6-7890-a1b2-c3d4e5f67890", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/message/a1b2c3d4-e5f6-7890-a1b2-c3d4e5f67890",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        var root = body.RootElement;
        Assert.Equal("Header text", root.GetProperty("header").GetString());
        Assert.Equal("Body text", root.GetProperty("body").GetString());
        var image = root.GetProperty("image");
        Assert.Equal("b2c3d4e5-f6a7-8901-b2c3-d4e5f6a78901", image.GetProperty("imageIdentifier").GetString());
        Assert.Equal("Alt text", image.GetProperty("altText").GetString());
    }

    // 19. DeleteMessageAsync
    [Fact]
    public async Task DeleteMessage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.DeleteMessageAsync("test-message-id", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/message/test-message-id", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 20. ConfigureDefaultMessageAsync
    [Fact]
    public async Task ConfigureDefaultMessage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new DefaultConfigurationRequest
        {
            MessageIdentifier = "msg-id"
        };

        await client.ConfigureDefaultMessageAsync("com.example.product", "en-US", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/default/com.example.product/en-US",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 21. DeleteDefaultMessageAsync
    [Fact]
    public async Task DeleteDefaultMessage()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.DeleteDefaultMessageAsync("com.example.product", "en-US", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/default/com.example.product/en-US",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 22. GetAppTransactionInfoAsync
    [Fact]
    public async Task GetAppTransactionInfo()
    {
        var (client, handler) = GetClientWithBody(
            "models.appTransactionInfoResponse.json", HttpStatusCode.OK);

        var response =
            await client.GetAppTransactionInfoAsync("999999", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/transactions/appTransactions/999999",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("signed_app_transaction_info_value", response.SignedAppTransactionInfo);
    }

    // 23. Error: GeneralInternal (500)
    [Fact]
    public async Task ApiError_GeneralInternal()
    {
        var (client, _) = GetClientWithBody(
            "models.apiException.json", HttpStatusCode.InternalServerError);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetTestNotificationStatusAsync("test-token", BundleId,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(500, ex.HttpStatusCode);
        Assert.Equal(APIError.GeneralInternal, ex.ApiError);
        Assert.Equal("An unknown error occurred.", ex.ErrorMessage);
    }

    // 24. Error: RateLimitExceeded (429)
    [Fact]
    public async Task ApiError_RateLimitExceeded()
    {
        var (client, _) = GetClientWithBody(
            "models.apiTooManyRequestsException.json", (HttpStatusCode)429);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetTestNotificationStatusAsync("test-token", BundleId,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(429, ex.HttpStatusCode);
        Assert.Equal(APIError.RateLimitExceeded, ex.ApiError);
        Assert.Equal("Rate limit exceeded.", ex.ErrorMessage);
    }

    // 25. Error: Unknown error code (ApiError is null)
    [Fact]
    public async Task ApiError_UnknownErrorCode()
    {
        var (client, _) = GetClientWithBody(
            "models.apiUnknownError.json", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetTestNotificationStatusAsync("test-token", BundleId,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(400, ex.HttpStatusCode);
        Assert.Equal(9990000L, ex.RawApiError);
        Assert.Null(ex.ApiError);
        Assert.Equal("Testing error.", ex.ErrorMessage);
    }

    // 26. Malformed environment in response deserializes as _Unmapped
    [Fact]
    public async Task MalformedEnvironment_DeserializesAs_Unmapped()
    {
        var (client, _) = GetClientWithBody(
            "models.transactionHistoryResponseWithMalformedEnvironment.json", HttpStatusCode.OK);

        var request = new TransactionHistoryRequest();

        var response = await client.GetTransactionHistoryAsync("999999", BundleId, request,
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(AppStoreEnvironment._Unmapped, response.Environment);
    }

    // 27. Malformed appAppleId in response throws
    [Fact]
    public async Task MalformedAppAppleId_ThrowsException()
    {
        var (client, _) = GetClientWithBody(
            "models.transactionHistoryResponseWithMalformedAppAppleId.json", HttpStatusCode.OK);

        var request = new TransactionHistoryRequest();

        await Assert.ThrowsAnyAsync<Exception>(() =>
            client.GetTransactionHistoryAsync("999999", BundleId, request,
                cancellationToken: TestContext.Current.CancellationToken));
    }

    // 28. Xcode environment throws ArgumentException
    [Fact]
    public void XcodeEnvironment_ThrowsArgumentException()
    {
        var signingKey = TestUtilities.GetSigningKey();

        Assert.Throws<ArgumentException>(() =>
            new AppStoreServerAPIClient(signingKey, KeyId, IssuerId, AppStoreEnvironment.Xcode));
    }

    // 29. SetAppAccountToken: InvalidAppAccountTokenUuid (4000183)
    [Fact]
    public async Task SetAppAccountToken_InvalidUuid()
    {
        var (client, _) = GetClientWithBody(
            "models.invalidAppAccountTokenUUIDError.json", HttpStatusCode.BadRequest);

        var request = new UpdateAppAccountTokenRequest
        {
            AppAccountToken = "invalid"
        };

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.SetAppAccountTokenAsync("12345", request, BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(400, ex.HttpStatusCode);
        Assert.Equal(APIError.InvalidAppAccountTokenUuid, ex.ApiError);
    }

    // 30. SetAppAccountToken: FamilyTransactionNotSupported (4000185)
    [Fact]
    public async Task SetAppAccountToken_FamilyTransactionNotSupported()
    {
        var (client, _) = GetClientWithBody(
            "models.familyTransactionNotSupportedError.json", HttpStatusCode.BadRequest);

        var request = new UpdateAppAccountTokenRequest
        {
            AppAccountToken = "7e3fb20b-4cdb-47cc-936d-99d65f608138"
        };

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.SetAppAccountTokenAsync("12345", request, BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(400, ex.HttpStatusCode);
        Assert.Equal(APIError.FamilyTransactionNotSupported, ex.ApiError);
    }

    // 31. SetAppAccountToken: TransactionIdIsNotOriginalTransactionId (4000187)
    [Fact]
    public async Task SetAppAccountToken_NotOriginalTransactionId()
    {
        var (client, _) = GetClientWithBody(
            "models.transactionIdNotOriginalTransactionId.json", HttpStatusCode.BadRequest);

        var request = new UpdateAppAccountTokenRequest
        {
            AppAccountToken = "7e3fb20b-4cdb-47cc-936d-99d65f608138"
        };

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.SetAppAccountTokenAsync("12345", request, BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(400, ex.HttpStatusCode);
        Assert.Equal(APIError.TransactionIdIsNotOriginalTransactionId, ex.ApiError);
    }

    // 32. GetAppTransactionInfo: InvalidTransactionId (4000006)
    [Fact]
    public async Task GetAppTransactionInfo_InvalidTransactionId()
    {
        var (client, _) = GetClientWithBody(
            "models.invalidTransactionIdError.json", HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetAppTransactionInfoAsync("invalid", BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(400, ex.HttpStatusCode);
        Assert.Equal(APIError.InvalidTransactionId, ex.ApiError);
    }

    // 33. GetAppTransactionInfo: AppTransactionDoesNotExist (4040019)
    [Fact]
    public async Task GetAppTransactionInfo_DoesNotExist()
    {
        var (client, _) = GetClientWithBody("models.appTransactionDoesNotExistError.json", HttpStatusCode.NotFound);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetAppTransactionInfoAsync("999999", BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(404, ex.HttpStatusCode);
        Assert.Equal(APIError.AppTransactionDoesNotExist, ex.ApiError);
    }

    // 34. GetAppTransactionInfo: TransactionIdNotFound (4040010)
    [Fact]
    public async Task GetAppTransactionInfo_TransactionIdNotFound()
    {
        var (client, _) = GetClientWithBody("models.transactionIdNotFoundError.json", HttpStatusCode.NotFound);

        var ex = await Assert.ThrowsAsync<APIException>(() =>
            client.GetAppTransactionInfoAsync("999999", BundleId, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(404, ex.HttpStatusCode);
        Assert.Equal(APIError.TransactionIdNotFound, ex.ApiError);
    }

    // 35. SendConsumptionData (deprecated V1)
    [Fact]
    public async Task SendConsumptionData_V1()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

#pragma warning disable CS0618 // Type or member is obsolete
        var request = new ConsumptionRequestV1
        {
            CustomerConsented = true,
            ConsumptionStatus = ConsumptionStatus.NotConsumed,
            Platform = Platform.NonApple,
            SampleContentProvided = false,
            DeliveryStatus = DeliveryStatusV1.DidNotDeliverDueToServerOutage,
            AppAccountToken = "7389a31a-fb6d-4569-a2a6-db7d85d84813",
            AccountTenure = AccountTenure.ThirtyDaysToNinetyDays,
            PlayTime = PlayTime.OneDayToFourDays,
            LifetimeDollarsRefunded = LifetimeDollarsRefunded
                .OneThousandDollarsToOneThousandNineHundredNinetyNineDollarsAndNinetyNineCents,
            LifetimeDollarsPurchased = LifetimeDollarsPurchased.TwoThousandDollarsOrGreater,
            UserStatus = UserStatus.LimitedAccess,
            RefundPreference = RefundPreferenceV1.NoPreference
        };

        await client.SendConsumptionDataAsync("49571273", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);
#pragma warning restore CS0618

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/transactions/consumption/49571273", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        var root = body.RootElement;
        Assert.True(root.GetProperty("customerConsented").GetBoolean());
        Assert.Equal(1, root.GetProperty("consumptionStatus").GetInt32());
        Assert.Equal(2, root.GetProperty("platform").GetInt32());
        Assert.False(root.GetProperty("sampleContentProvided").GetBoolean());
        Assert.Equal(3, root.GetProperty("deliveryStatus").GetInt32());
        Assert.Equal("7389a31a-fb6d-4569-a2a6-db7d85d84813", root.GetProperty("appAccountToken").GetString());
        Assert.Equal(4, root.GetProperty("accountTenure").GetInt32());
        Assert.Equal(5, root.GetProperty("playTime").GetInt32());
        Assert.Equal(6, root.GetProperty("lifetimeDollarsRefunded").GetInt32());
        Assert.Equal(7, root.GetProperty("lifetimeDollarsPurchased").GetInt32());
        Assert.Equal(4, root.GetProperty("userStatus").GetInt32());
        Assert.Equal(3, root.GetProperty("refundPreference").GetInt32());
    }

    // 36. GetTransactionHistory with V1 version
    [Fact]
    public async Task GetTransactionHistory_V1()
    {
        var (client, handler) = GetClientWithBody("models.transactionHistoryResponse.json", HttpStatusCode.OK);

        var request = new TransactionHistoryRequest();

#pragma warning disable CS0612 // Type or member is obsolete
        await client.GetTransactionHistoryAsync("999999", BundleId, request, version: GetTransactionHistoryVersion.V1,
            cancellationToken: TestContext.Current.CancellationToken);
#pragma warning restore CS0612 // Type or member is obsolete

        Assert.Equal("/inApps/v1/history/999999", handler.CapturedRequest!.RequestUri!.AbsolutePath);
    }

    // 37. GetAllSubscriptionStatuses without status filter
    [Fact]
    public async Task GetAllSubscriptionStatuses_NoFilter()
    {
        var (client, handler) = GetClientWithBody("models.getAllSubscriptionStatusesResponse.json", HttpStatusCode.OK);

        await client.GetAllSubscriptionStatusesAsync("4321", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/inApps/v1/subscriptions/4321", handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Equal("", handler.CapturedRequest.RequestUri.Query);
    }

    // 38. GetRefundHistory without revision
    [Fact]
    public async Task GetRefundHistory_NoRevision()
    {
        var (client, handler) = GetClientWithBody("models.getRefundHistoryResponse.json", HttpStatusCode.OK);

        await client.GetRefundHistoryAsync("555555", BundleId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/inApps/v2/refund/lookup/555555", handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Equal("", handler.CapturedRequest.RequestUri.Query);
    }

    [Fact]
    public async Task RequestTestNotification_SameClient_UsesPerCallBundleId()
    {
        var (client, handler) = GetClientWithBody("models.requestTestNotificationResponse.json", HttpStatusCode.OK);

        await client.RequestTestNotificationAsync("com.example.one",
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal("com.example.one", GetBearerBundleId(handler));

        await client.RequestTestNotificationAsync("com.example.two",
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal("com.example.two", GetBearerBundleId(handler));
    }

    // 39. FinishTransactionAsync
    [Fact]
    public async Task FinishTransaction()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.FinishTransactionAsync("999999", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/transactions/999999/finish", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 40. GetDefaultMessageAsync
    [Fact]
    public async Task GetDefaultMessage()
    {
        var (client, handler) = GetClientWithBody(
            "models.getDefaultMessageResponse.json", HttpStatusCode.OK);

        var response = await client.GetDefaultMessageAsync("com.example.product", "en-US", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/default/com.example.product/en-US",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.MessageIdentifier);
    }

    // 41. ConfigureRealtimeUrlAsync
    [Fact]
    public async Task ConfigureRealtimeUrl()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new RealtimeUrlRequest
        {
            RealtimeUrl = "https://example.com/retention"
        };

        await client.ConfigureRealtimeUrlAsync(request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/realtime/url", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        Assert.Equal("https://example.com/retention", body.RootElement.GetProperty("realtimeURL").GetString());
    }

    // 42. DeleteRealtimeUrlAsync
    [Fact]
    public async Task DeleteRealtimeUrl()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.DeleteRealtimeUrlAsync(BundleId, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/realtime/url", handler.CapturedRequest.RequestUri!.AbsolutePath);
    }

    // 43. GetRealtimeUrlAsync
    [Fact]
    public async Task GetRealtimeUrl()
    {
        var (client, handler) = GetClientWithBody(
            "models.getRealtimeUrlResponse.json", HttpStatusCode.OK);

        var response = await client.GetRealtimeUrlAsync(BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/realtime/url", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.RealtimeUrl);
    }

    // 44. InitiatePerformanceTestAsync
    [Fact]
    public async Task InitiatePerformanceTest()
    {
        var (client, handler) = GetClientWithBody(
            "models.performanceTestResponse.json", HttpStatusCode.OK);

        var request = new PerformanceTestRequest
        {
            OriginalTransactionId = "999999"
        };

        var response = await client.InitiatePerformanceTestAsync(request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/performanceTest", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.RequestId);
        Assert.NotNull(response.Config);
    }

    // 45. GetPerformanceTestResultsAsync
    [Fact]
    public async Task GetPerformanceTestResults()
    {
        var (client, handler) = GetClientWithBody(
            "models.performanceTestResultResponse.json", HttpStatusCode.OK);

        var response = await client.GetPerformanceTestResultsAsync("test-request-id", BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/performanceTest/result/test-request-id",
            handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.NotNull(response.Config);
        Assert.NotNull(response.ResponseTimes);
    }

    // 46. UploadImageAsync with imageSize
    [Fact]
    public async Task UploadImage_WithImageSize()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        await client.UploadImageAsync("test-image-id", [0x89, 0x50, 0x4E, 0x47], BundleId,
            imageSize: ImageSize.BulletPoint, cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
        Assert.Equal("/inApps/v1/messaging/image/test-image-id", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Contains("imageSize=BULLET_POINT", handler.CapturedRequest.RequestUri.Query);
    }

    // 47. UploadMessageAsync with bullet points and headerPosition
    [Fact]
    public async Task UploadMessage_WithBulletPoints()
    {
        var (client, handler) = GetClientWithBody(null, HttpStatusCode.OK);

        var request = new UploadMessageRequestBody
        {
            Header = "Header text",
            Body = "Body text",
            HeaderPosition = HeaderPosition.AboveImage,
            BulletPoints =
            [
                new BulletPoint
                {
                    Text = "Point 1",
                    ImageIdentifier = "img-1",
                    AltText = "Bullet image 1"
                }
            ]
        };

        await client.UploadMessageAsync("msg-id", request, BundleId,
            cancellationToken: TestContext.Current.CancellationToken);

        AssertCommonHeaders(handler);
        Assert.NotNull(handler.CapturedRequestBody);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody);
        var root = body.RootElement;
        Assert.Equal("ABOVE_IMAGE", root.GetProperty("headerPosition").GetString());
        var bullets = root.GetProperty("bulletPoints");
        Assert.Equal(JsonValueKind.Array, bullets.ValueKind);
        Assert.Equal(1, bullets.GetArrayLength());
        Assert.Equal("Point 1", bullets[0].GetProperty("text").GetString());
        Assert.Equal("img-1", bullets[0].GetProperty("imageIdentifier").GetString());
        Assert.Equal("Bullet image 1", bullets[0].GetProperty("altText").GetString());
    }
}
