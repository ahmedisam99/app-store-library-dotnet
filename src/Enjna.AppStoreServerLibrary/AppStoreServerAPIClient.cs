using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Enjna.AppStoreServerLibrary.Models;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Enjna.AppStoreServerLibrary;

/// <summary>
/// A client for the App Store Server API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi"/>
public class AppStoreServerAPIClient : IDisposable
{
    private const string ProductionUrl = "https://api.storekit.apple.com";
    private const string SandboxUrl = "https://api.storekit-sandbox.apple.com";
    private const string LocalTestingUrl = "https://local-testing-base-url";
    private static readonly string UserAgent = $"enjna-app-store-server-library/dotnet/{PackageVersion()}";
    private static readonly JsonSerializerOptions JsonOptions = new();

    private readonly string _signingKey;
    private readonly string _keyId;
    private readonly string _issuerId;
    private readonly string _urlBase;
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private bool _disposed;

    /// <summary>
    /// Creates a new App Store Server API client.
    /// </summary>
    /// <param name="signingKey">Your private key downloaded from App Store Connect, in PEM format.</param>
    /// <param name="keyId">Your private key ID from App Store Connect.</param>
    /// <param name="issuerId">Your issuer ID from the Keys page in App Store Connect.</param>
    /// <param name="environment">The environment to target.</param>
    /// <param name="httpClient">An optional <see cref="HttpClient"/> instance to use for requests.</param>
    /// <exception cref="ArgumentException">Thrown when the environment is Xcode.</exception>
    public AppStoreServerAPIClient(
        string signingKey,
        string keyId,
        string issuerId,
        AppStoreEnvironment environment,
        HttpClient? httpClient = null)
    {
        _signingKey = signingKey;
        _keyId = keyId;
        _issuerId = issuerId;
        _ownsHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });

        _urlBase = environment switch
        {
            AppStoreEnvironment.Production => ProductionUrl,
            AppStoreEnvironment.Sandbox => SandboxUrl,
            AppStoreEnvironment.LocalTesting => LocalTestingUrl,
            AppStoreEnvironment.Xcode => throw new ArgumentException("Xcode is not a supported for an AppStoreServerAPIClient"),
            _ => throw new ArgumentException($"Unsupported environment: {environment}")
        };
    }

    /// <summary>
    /// Uses a subscription's product identifier to extend the renewal date for all of its eligible active subscribers.
    /// </summary>
    /// <param name="request">The request body for extending a subscription renewal date for all of its active subscribers.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that indicates the server successfully received the subscription-renewal-date extension request.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/extend_subscription_renewal_dates_for_all_active_subscribers"/>
    public async Task<MassExtendRenewalDateResponse> ExtendRenewalDateForAllActiveSubscribersAsync(
        MassExtendRenewalDateRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<MassExtendRenewalDateResponse>(
                path: "/inApps/v1/subscriptions/extend/mass",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Extends the renewal date of a customer's active subscription using the original transaction identifier.
    /// </summary>
    /// <param name="originalTransactionId">The original transaction identifier of the subscription receiving a renewal date extension.</param>
    /// <param name="request">The request body containing subscription-renewal-extension data.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that indicates whether an individual renewal-date extension succeeded, and related details.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/extend_a_subscription_renewal_date"/>
    public async Task<ExtendRenewalDateResponse> ExtendSubscriptionRenewalDateAsync(
        string originalTransactionId,
        ExtendRenewalDateRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ExtendRenewalDateResponse>(
                path: $"/inApps/v1/subscriptions/extend/{originalTransactionId}",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get the statuses for all of a customer's auto-renewable subscriptions in your app.
    /// </summary>
    /// <param name="anyTransactionId">Any transactionId, originalTransactionId, or appTransactionId that belongs to the customer for your app.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="status">An optional filter that indicates the status of subscriptions to include in the response. Your query may specify more than one status query parameter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains status information for all of a customer's auto-renewable subscriptions in your app.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_all_subscription_statuses"/>
    public async Task<StatusResponse> GetAllSubscriptionStatusesAsync(
        string anyTransactionId,
        string bundleId,
        Status[]? status = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string[]>();
        if (status is not null)
        {
            queryParams["status"] = status.Select(s => ((int)s).ToString()).ToArray();
        }

        return await MakeRequestAsync<StatusResponse>(
                path: $"/inApps/v1/subscriptions/{anyTransactionId}",
                method: HttpMethod.Get,
                queryParameters: queryParams,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a paginated list of all of a customer's refunded in-app purchases for your app.
    /// </summary>
    /// <param name="anyTransactionId">Any transactionId, originalTransactionId, or appTransactionId that belongs to the customer for your app.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="revision">A token you provide to get the next set of up to 20 transactions. All responses include a revision token. Use the revision token from the previous <see cref="RefundHistoryResponse"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains status information for all of a customer's auto-renewable subscriptions in your app.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_refund_history"/>
    public async Task<RefundHistoryResponse> GetRefundHistoryAsync(
        string anyTransactionId,
        string bundleId,
        string? revision = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string[]>();
        if (revision is not null)
        {
            queryParams["revision"] = [revision];
        }

        return await MakeRequestAsync<RefundHistoryResponse>(
                path: $"/inApps/v2/refund/lookup/{anyTransactionId}",
                method: HttpMethod.Get,
                queryParameters: queryParams,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Checks whether a renewal date extension request completed, and provides the final count of successful or failed extensions.
    /// </summary>
    /// <param name="requestIdentifier">The UUID that represents your request to the Extend Subscription Renewal Dates for All Active Subscribers endpoint.</param>
    /// <param name="productId">The product identifier of the auto-renewable subscription that you request a renewal-date extension for.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that indicates the current status of a request to extend the subscription renewal date to all eligible subscribers.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_status_of_subscription_renewal_date_extensions"/>
    public async Task<MassExtendRenewalDateStatusResponse> GetStatusOfSubscriptionRenewalDateExtensionsAsync(
        string requestIdentifier,
        string productId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<MassExtendRenewalDateStatusResponse>(
                path: $"/inApps/v1/subscriptions/extend/mass/{productId}/{requestIdentifier}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Check the status of the test App Store server notification sent to your server.
    /// </summary>
    /// <param name="testNotificationToken">The test notification token received from the Request a Test Notification endpoint.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the contents of the test notification sent by the App Store server and the result from your server.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_test_notification_status"/>
    public async Task<CheckTestNotificationResponse> GetTestNotificationStatusAsync(
        string testNotificationToken,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<CheckTestNotificationResponse>(
                path: $"/inApps/v1/notifications/test/{testNotificationToken}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a list of notifications that the App Store server attempted to send to your server.
    /// </summary>
    /// <param name="request">The request body that includes the start and end dates, and optional query constraints.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="paginationToken">An optional token you use to get the next set of up to 20 notification history records. All responses that have more records available include a paginationToken. Omit this parameter the first time you call this endpoint.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the App Store Server Notifications history for your app.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_notification_history"/>
    public async Task<NotificationHistoryResponse> GetNotificationHistoryAsync(
        NotificationHistoryRequest request,
        string bundleId,
        string? paginationToken = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string[]>();
        if (paginationToken is not null)
        {
            queryParams["paginationToken"] = [paginationToken];
        }

        return await MakeRequestAsync<NotificationHistoryResponse>(
                path: "/inApps/v1/notifications/history",
                method: HttpMethod.Post,
                queryParameters: queryParams,
                body: request,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a customer's in-app purchase transaction history for your app.
    /// </summary>
    /// <param name="anyTransactionId">Any transactionId, originalTransactionId, or appTransactionId that belongs to the customer for your app.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="request">An optional request that includes query constraints.</param>
    /// <param name="version">The version of the Get Transaction History endpoint to use. V2 is recommended.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the customer's transaction history for an app.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_transaction_history"/>
    public async Task<HistoryResponse> GetTransactionHistoryAsync(
        string anyTransactionId,
        string bundleId,
        TransactionHistoryRequest? request = null,
        GetTransactionHistoryVersion version = GetTransactionHistoryVersion.V2,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string[]>();

        if (request?.Revision is not null)
        {
            queryParams["revision"] = [request.Revision];
        }

        if (request?.StartDate is not null)
        {
            queryParams["startDate"] = [request.StartDate.Value.ToString()];
        }

        if (request?.EndDate is not null)
        {
            queryParams["endDate"] = [request.EndDate.Value.ToString()];
        }

        if (request?.ProductIds is not null)
        {
            queryParams["productId"] = request.ProductIds;
        }

        if (request?.ProductTypes is not null)
        {
            queryParams["productType"] = request.ProductTypes.Select(GetEnumMemberValue).ToArray();
        }

        if (request?.Sort is not null)
        {
            queryParams["sort"] = [GetEnumMemberValue(request.Sort.Value)];
        }

        if (request?.SubscriptionGroupIdentifiers is not null)
        {
            queryParams["subscriptionGroupIdentifier"] = request.SubscriptionGroupIdentifiers;
        }

        if (request?.InAppOwnershipType is not null)
        {
            queryParams["inAppOwnershipType"] = [GetEnumMemberValue(request.InAppOwnershipType.Value)];
        }

        if (request?.Revoked is not null)
        {
            queryParams["revoked"] = [request.Revoked.Value.ToString().ToLowerInvariant()];
        }

        var versionStr = GetEnumMemberValue(version);

        return await MakeRequestAsync<HistoryResponse>(
                path: $"/inApps/{versionStr}/history/{anyTransactionId}",
                method: HttpMethod.Get,
                queryParameters: queryParams,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get information about a single transaction for your app.
    /// </summary>
    /// <param name="transactionId">The identifier of a transaction that belongs to the customer, and which may be an original transaction identifier.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains signed transaction information for a single transaction.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get_transaction_info"/>
    public async Task<TransactionInfoResponse> GetTransactionInfoAsync(
        string transactionId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<TransactionInfoResponse>(
                path: $"/inApps/v1/transactions/{transactionId}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a customer's in-app purchases from a receipt using the order ID.
    /// </summary>
    /// <param name="orderId">The order ID for in-app purchases that belong to the customer.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that includes the order lookup status and an array of signed transactions for the in-app purchases in the order.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/look_up_order_id"/>
    public async Task<OrderLookupResponse> LookUpOrderIdAsync(
        string orderId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<OrderLookupResponse>(
                path: $"/inApps/v1/lookup/{orderId}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Ask App Store Server Notifications to send a test notification to your server.
    /// </summary>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the test notification token.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/request_a_test_notification"/>
    public async Task<SendTestNotificationResponse> RequestTestNotificationAsync(
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<SendTestNotificationResponse>(
                path: "/inApps/v1/notifications/test",
                method: HttpMethod.Post,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Send consumption information about a consumable in-app purchase to the App Store after your server receives a consumption request notification.
    /// </summary>
    /// <param name="transactionId">The transaction identifier for which you're providing consumption information. You receive this identifier in the CONSUMPTION_REQUEST notification the App Store sends to your server.</param>
    /// <param name="request">The request body containing consumption information.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/send-consumption-information-v1"/>
    [Obsolete("Use SendConsumptionInformationAsync instead.")]
    public async Task SendConsumptionDataAsync(
        string transactionId,
        ConsumptionRequestV1 request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/transactions/consumption/{transactionId}",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Send consumption information about an In-App Purchase to the App Store after your server receives a consumption request notification.
    /// </summary>
    /// <param name="transactionId">The transaction identifier for which you're providing consumption information. You receive this identifier in the CONSUMPTION_REQUEST notification the App Store sends to your server's App Store Server Notifications V2 endpoint.</param>
    /// <param name="request">The request body containing consumption information.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/send-consumption-information"/>
    public async Task SendConsumptionInformationAsync(
        string transactionId,
        ConsumptionRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v2/transactions/consumption/{transactionId}",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the app account token value for a purchase the customer makes outside your app, or updates its value in an existing transaction.
    /// </summary>
    /// <param name="originalTransactionId">The original transaction identifier of the transaction to receive the app account token update.</param>
    /// <param name="request">The request body that contains a valid app account token value.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/set-app-account-token"/>
    public async Task SetAppAccountTokenAsync(
        string originalTransactionId,
        UpdateAppAccountTokenRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/transactions/{originalTransactionId}/appAccountToken",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Upload an image to use for retention messaging.
    /// </summary>
    /// <param name="imageIdentifier">A UUID you provide to uniquely identify the image you upload. Must be lowercase.</param>
    /// <param name="image">The image file to upload.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="imageSize">The size of the image you upload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/upload-image"/>
    public async Task UploadImageAsync(
        string imageIdentifier,
        byte[] image,
        string bundleId,
        ImageSize? imageSize = null,
        CancellationToken cancellationToken = default)
    {
        Dictionary<string, string[]>? queryParams = null;
        if (imageSize.HasValue)
        {
            queryParams = new Dictionary<string, string[]>
            {
                ["imageSize"] = [GetEnumMemberValue(imageSize.Value)]
            };
        }

        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/image/{imageIdentifier}",
                method: HttpMethod.Put,
                queryParameters: queryParams,
                body: image,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a previously uploaded image.
    /// </summary>
    /// <param name="imageIdentifier">The identifier of the image to delete.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/delete-image"/>
    public async Task DeleteImageAsync(
        string imageIdentifier,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/image/{imageIdentifier}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get the image identifier and state for all uploaded images.
    /// </summary>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains status information for all images.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/get-image-list"/>
    public async Task<GetImageListResponse> GetImageListAsync(
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetImageListResponse>(
                path: "/inApps/v1/messaging/image/list",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Upload a message to use for retention messaging.
    /// </summary>
    /// <param name="messageIdentifier">A UUID you provide to uniquely identify the message you upload. Must be lowercase.</param>
    /// <param name="request">The message text to upload.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/upload-message"/>
    public async Task UploadMessageAsync(
        string messageIdentifier,
        UploadMessageRequestBody request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/message/{messageIdentifier}",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a previously uploaded message.
    /// </summary>
    /// <param name="messageIdentifier">The identifier of the message to delete.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/delete-message"/>
    public async Task DeleteMessageAsync(
        string messageIdentifier,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/message/{messageIdentifier}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get the message identifier and state of all uploaded messages.
    /// </summary>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains status information for all messages.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/get-message-list"/>
    public async Task<GetMessageListResponse> GetMessageListAsync(
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<GetMessageListResponse>(
                path: "/inApps/v1/messaging/message/list",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Configure a default message for a specific product in a specific locale.
    /// </summary>
    /// <param name="productId">The product identifier for the default configuration.</param>
    /// <param name="locale">The locale for the default configuration.</param>
    /// <param name="request">The request body that includes the message identifier to configure as the default message.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/configure-default-message"/>
    public async Task ConfigureDefaultMessageAsync(
        string productId,
        string locale,
        DefaultConfigurationRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/default/{productId}/{locale}",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a default message for a product in a locale.
    /// </summary>
    /// <param name="productId">The product ID of the default message configuration.</param>
    /// <param name="locale">The locale of the default message configuration.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/delete-default-message"/>
    public async Task DeleteDefaultMessageAsync(
        string productId,
        string locale,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/messaging/default/{productId}/{locale}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Get a customer's app transaction information for your app.
    /// </summary>
    /// <param name="anyTransactionId">Any transactionId, originalTransactionId, or appTransactionId that belongs to the customer for your app.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains signed app transaction information for a customer.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/get-app-transaction-info"/>
    public async Task<AppTransactionInfoResponse> GetAppTransactionInfoAsync(
        string anyTransactionId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<AppTransactionInfoResponse>(
                path: $"/inApps/v1/transactions/appTransactions/{anyTransactionId}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Notifies the App Store server that your system has finished processing the customer's transaction.
    /// </summary>
    /// <param name="transactionId">The transaction identifier of the transaction to mark as finished.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/finish-transaction"/>
    public async Task FinishTransactionAsync(
        string transactionId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: $"/inApps/v1/transactions/{transactionId}/finish",
                method: HttpMethod.Post,
                queryParameters: null,
                body: null,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the default message for a specific product in a specific locale, if it's configured.
    /// </summary>
    /// <param name="productId">The product identifier of the message.</param>
    /// <param name="locale">The locale of the message.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The response body that contains the default configuration information.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/get-default-message"/>
    public async Task<DefaultConfigurationResponse> GetDefaultMessageAsync(
        string productId,
        string locale,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<DefaultConfigurationResponse>(
                path: $"/inApps/v1/messaging/default/{productId}/{locale}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Configures the URL for your Get Retention Message endpoint in the sandbox and production environments.
    /// </summary>
    /// <param name="request">The request body that includes your endpoint's URL.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/configure-realtime-url"/>
    public async Task ConfigureRealtimeUrlAsync(
        RealtimeUrlRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: "/inApps/v1/messaging/realtime/url",
                method: HttpMethod.Put,
                queryParameters: null,
                body: request,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the URL for your Get Retention Message endpoint, in the sandbox or production environments.
    /// </summary>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/delete-realtime-url"/>
    public async Task DeleteRealtimeUrlAsync(
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object?>(
                path: "/inApps/v1/messaging/realtime/url",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the URL for real-time messages that points to your Get Retention Message endpoint, which you previously configured.
    /// </summary>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The response body that contains the URL for your Get Retention Message endpoint.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/get-realtime-url"/>
    public async Task<RealtimeUrlResponse> GetRealtimeUrlAsync(
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<RealtimeUrlResponse>(
                path: "/inApps/v1/messaging/realtime/url",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Initiates a performance test of your Get Retention Message endpoint in the sandbox environment.
    /// </summary>
    /// <param name="request">The request body which specifies a transaction identifier of an In-App Purchase to use for this test.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The performance test response object.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/initiate-performance-test"/>
    public async Task<PerformanceTestResponse> InitiatePerformanceTestAsync(
        PerformanceTestRequest request,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PerformanceTestResponse>(
                path: "/inApps/v1/messaging/performanceTest",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the results of the performance test for the specified identifier.
    /// </summary>
    /// <param name="requestId">The ID of the performance test to return, which you receive in the <see cref="PerformanceTestResponse"/> when you call <see cref="InitiatePerformanceTestAsync"/>.</param>
    /// <param name="bundleId">Your app's bundle ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An object the API returns that describes the performance test results.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/retentionmessaging/get-performance-test-results"/>
    public async Task<PerformanceTestResultResponse> GetPerformanceTestResultsAsync(
        string requestId,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<PerformanceTestResultResponse>(
                path: $"/inApps/v1/messaging/performanceTest/result/{requestId}",
                method: HttpMethod.Get,
                queryParameters: null,
                body: null,
                parseResponse: true,
                bundleId: bundleId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<T> MakeRequestAsync<T>(
        string path,
        HttpMethod method,
        Dictionary<string, string[]>? queryParameters,
        object? body,
        bool parseResponse,
        string bundleId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(_urlBase + path);

        if (queryParameters is { Count: > 0 })
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var (key, values) in queryParameters)
            {
                foreach (var value in values)
                {
                    query.Add(key, value);
                }
            }

            uriBuilder.Query = query.ToString();
        }

        using var request = new HttpRequestMessage(method, uriBuilder.Uri);
        request.Headers.TryAddWithoutValidation("User-Agent", UserAgent);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateBearerToken(bundleId));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (body is byte[] binaryBody)
        {
            request.Content = new ByteArrayContent(binaryBody);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        }
        else if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            if (!parseResponse)
            {
                return default!;
            }

            var responseBodyStr = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var responseBody = JsonSerializer.Deserialize<T>(responseBodyStr, JsonOptions);

            if (responseBody is null)
            {
                throw new Exception("Unexpected response body format");
            }

            return responseBody;
        }

        try
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var error = JsonSerializer.Deserialize<ErrorResponseBody>(errorBody, JsonOptions);

            if (error?.ErrorCode.HasValue == true)
            {
                var rawApiError = error.ErrorCode.Value;
                var apiError = Enum.IsDefined(typeof(APIError), rawApiError)
                    ? (APIError?)rawApiError
                    : null;

                throw new APIException((int)response.StatusCode, rawApiError, apiError, error.ErrorMessage);
            }

            throw new APIException((int)response.StatusCode);
        }
        catch (APIException)
        {
            throw;
        }
        catch
        {
            throw new APIException((int)response.StatusCode);
        }
    }

    private string CreateBearerToken(string bundleId)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(_signingKey);

        var securityKey = new ECDsaSecurityKey(ecdsa)
        {
            KeyId = _keyId
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuerId,
            Audience = "appstoreconnect-v1",
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256),
            Claims = new Dictionary<string, object>
            {
                ["bid"] = bundleId
            }
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }

    private static string GetEnumMemberValue<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var memberInfo = typeof(TEnum).GetMember(value.ToString());
        if (memberInfo.Length > 0)
        {
            var attr = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
            if (attr?.Value is not null)
            {
                return attr.Value;
            }
        }

        return value.ToString();
    }

    private static string PackageVersion()
    {
        var informationalVersion = typeof(AppStoreServerAPIClient).Assembly
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
}
