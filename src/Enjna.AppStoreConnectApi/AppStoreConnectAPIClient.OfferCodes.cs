using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Creates an offer code for a consumable, non-consumable, or non-renewing in-app purchase,
    /// along with the prices customers pay in each territory.
    /// </summary>
    /// <param name="request">The offer code to create, and the prices to create with it.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the In-App Purchase Offer Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseoffercodes"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCode>> CreateInAppPurchaseOfferCodeAsync(
        InAppPurchaseOfferCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCode>>(
                path: "/v1/inAppPurchaseOfferCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single in-app purchase offer code, including how many codes it has
    /// generated in each environment.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeId">The opaque resource ID of the In-App Purchase Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and per-relationship limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an In-App Purchase Offer Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCode>> GetInAppPurchaseOfferCodeAsync(
        string inAppPurchaseOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCode>>(
                path: $"/v1/inAppPurchaseOfferCodes/{inAppPurchaseOfferCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates an in-app purchase offer code, which stops customers from redeeming any of its
    /// codes. Deactivation is permanent.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeId">The opaque resource ID of the In-App Purchase Offer Codes resource.</param>
    /// <param name="request">The change to apply to the offer code.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase Offer Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaseoffercodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCode>> UpdateInAppPurchaseOfferCodeAsync(
        string inAppPurchaseOfferCodeId,
        InAppPurchaseOfferCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCode>>(
                path: $"/v1/inAppPurchaseOfferCodes/{inAppPurchaseOfferCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the custom codes of an in-app purchase offer code.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeId">The opaque resource ID of the In-App Purchase Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Offer Code Custom Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodes-_id_-customcodes"/>
    public async Task<ResourceListResponse<InAppPurchaseOfferCodeCustomCode>> ListCustomCodesForInAppPurchaseOfferCodeAsync(
        string inAppPurchaseOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseOfferCodeCustomCode>>(
                path: $"/v1/inAppPurchaseOfferCodes/{inAppPurchaseOfferCodeId}/customCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the batches of one-time use codes of an in-app purchase offer code.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeId">The opaque resource ID of the In-App Purchase Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Offer Code One-Time Use Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodes-_id_-onetimeusecodes"/>
    public async Task<ResourceListResponse<InAppPurchaseOfferCodeOneTimeUseCode>> ListOneTimeUseCodesForInAppPurchaseOfferCodeAsync(
        string inAppPurchaseOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseOfferCodeOneTimeUseCode>>(
                path: $"/v1/inAppPurchaseOfferCodes/{inAppPurchaseOfferCodeId}/oneTimeUseCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prices of an in-app purchase offer code, one per territory the offer is available
    /// in. Filter by territory with <c>filter[territory]</c>.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeId">The opaque resource ID of the In-App Purchase Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Offer Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodes-_id_-prices"/>
    public async Task<ResourceListResponse<InAppPurchaseOfferPrice>> ListPricesForInAppPurchaseOfferCodeAsync(
        string inAppPurchaseOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseOfferPrice>>(
                path: $"/v1/inAppPurchaseOfferCodes/{inAppPurchaseOfferCodeId}/prices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a custom code for an in-app purchase offer code: one code you choose, which many
    /// customers can redeem.
    /// </summary>
    /// <param name="request">The custom code to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the In-App Purchase Offer Code Custom Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseoffercodecustomcodes"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeCustomCode>> CreateInAppPurchaseOfferCodeCustomCodeAsync(
        InAppPurchaseOfferCodeCustomCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeCustomCode>>(
                path: "/v1/inAppPurchaseOfferCodeCustomCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single custom code of an in-app purchase offer code.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeCustomCodeId">The opaque resource ID of the In-App Purchase Offer Code Custom Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an In-App Purchase Offer Code Custom Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodecustomcodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeCustomCode>> GetInAppPurchaseOfferCodeCustomCodeAsync(
        string inAppPurchaseOfferCodeCustomCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeCustomCode>>(
                path: $"/v1/inAppPurchaseOfferCodeCustomCodes/{inAppPurchaseOfferCodeCustomCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates a custom code of an in-app purchase offer code, which stops customers from
    /// redeeming it. Deactivation is permanent.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeCustomCodeId">The opaque resource ID of the In-App Purchase Offer Code Custom Codes resource.</param>
    /// <param name="request">The change to apply to the custom code.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase Offer Code Custom Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaseoffercodecustomcodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeCustomCode>> UpdateInAppPurchaseOfferCodeCustomCodeAsync(
        string inAppPurchaseOfferCodeCustomCodeId,
        InAppPurchaseOfferCodeCustomCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeCustomCode>>(
                path: $"/v1/inAppPurchaseOfferCodeCustomCodes/{inAppPurchaseOfferCodeCustomCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a batch of one-time use codes for an in-app purchase offer code. Each generated
    /// code is unique, and a customer can redeem it once.
    /// </summary>
    /// <param name="request">The batch of codes to generate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the In-App Purchase Offer Code One-Time Use Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseoffercodeonetimeusecodes"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>> CreateInAppPurchaseOfferCodeOneTimeUseCodeAsync(
        InAppPurchaseOfferCodeOneTimeUseCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>>(
                path: "/v1/inAppPurchaseOfferCodeOneTimeUseCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single batch of one-time use codes of an in-app purchase offer code.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeOneTimeUseCodeId">The opaque resource ID of the In-App Purchase Offer Code One-Time Use Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an In-App Purchase Offer Code One-Time Use Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodeonetimeusecodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>> GetInAppPurchaseOfferCodeOneTimeUseCodeAsync(
        string inAppPurchaseOfferCodeOneTimeUseCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>>(
                path: $"/v1/inAppPurchaseOfferCodeOneTimeUseCodes/{inAppPurchaseOfferCodeOneTimeUseCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates a batch of one-time use codes of an in-app purchase offer code, which stops
    /// customers from redeeming any code in it. Deactivation is permanent.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeOneTimeUseCodeId">The opaque resource ID of the In-App Purchase Offer Code One-Time Use Codes resource.</param>
    /// <param name="request">The change to apply to the batch of codes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase Offer Code One-Time Use Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaseoffercodeonetimeusecodes-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>> UpdateInAppPurchaseOfferCodeOneTimeUseCodeAsync(
        string inAppPurchaseOfferCodeOneTimeUseCodeId,
        InAppPurchaseOfferCodeOneTimeUseCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseOfferCodeOneTimeUseCode>>(
                path: $"/v1/inAppPurchaseOfferCodeOneTimeUseCodes/{inAppPurchaseOfferCodeOneTimeUseCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads the codes of a batch of one-time use codes of an in-app purchase offer code, as a
    /// comma-separated value document you can hand out to customers.
    /// </summary>
    /// <param name="inAppPurchaseOfferCodeOneTimeUseCodeId">The opaque resource ID of the In-App Purchase Offer Code One-Time Use Codes resource.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The generated codes, as the text of a comma-separated value document.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseoffercodeonetimeusecodes-_id_-values"/>
    public async Task<string> GetValuesForInAppPurchaseOfferCodeOneTimeUseCodeAsync(
        string inAppPurchaseOfferCodeOneTimeUseCodeId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRawTextRequestAsync(
                path: $"/v1/inAppPurchaseOfferCodeOneTimeUseCodes/{inAppPurchaseOfferCodeOneTimeUseCodeId}/values",
                method: HttpMethod.Get,
                queryParameters: null,
                accept: "text/csv",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the offer codes of an in-app purchase. Filter by territory with
    /// <c>filter[territory]</c>.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the In-App Purchases resource.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Offer Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-offercodes"/>
    public async Task<ResourceListResponse<InAppPurchaseOfferCode>> ListOfferCodesForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseOfferCode>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/offerCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an offer code for an auto-renewable subscription, along with the prices customers
    /// pay in each territory.
    /// </summary>
    /// <param name="request">The offer code to create, and the prices to create with it.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Offer Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionoffercodes"/>
    public async Task<ResourceResponse<SubscriptionOfferCode>> CreateSubscriptionOfferCodeAsync(
        SubscriptionOfferCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCode>>(
                path: "/v1/subscriptionOfferCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single subscription offer code, including how many codes it has
    /// generated in each environment.
    /// </summary>
    /// <param name="subscriptionOfferCodeId">The opaque resource ID of the Subscription Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and per-relationship limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a Subscription Offer Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCode>> GetSubscriptionOfferCodeAsync(
        string subscriptionOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCode>>(
                path: $"/v1/subscriptionOfferCodes/{subscriptionOfferCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates a subscription offer code, which stops customers from redeeming any of its
    /// codes. Deactivation is permanent.
    /// </summary>
    /// <param name="subscriptionOfferCodeId">The opaque resource ID of the Subscription Offer Codes resource.</param>
    /// <param name="request">The change to apply to the offer code.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Offer Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionoffercodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCode>> UpdateSubscriptionOfferCodeAsync(
        string subscriptionOfferCodeId,
        SubscriptionOfferCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCode>>(
                path: $"/v1/subscriptionOfferCodes/{subscriptionOfferCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the custom codes of a subscription offer code.
    /// </summary>
    /// <param name="subscriptionOfferCodeId">The opaque resource ID of the Subscription Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Offer Code Custom Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodes-_id_-customcodes"/>
    public async Task<ResourceListResponse<SubscriptionOfferCodeCustomCode>> ListCustomCodesForSubscriptionOfferCodeAsync(
        string subscriptionOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionOfferCodeCustomCode>>(
                path: $"/v1/subscriptionOfferCodes/{subscriptionOfferCodeId}/customCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the batches of one-time use codes of a subscription offer code.
    /// </summary>
    /// <param name="subscriptionOfferCodeId">The opaque resource ID of the Subscription Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Offer Code One-Time Use Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodes-_id_-onetimeusecodes"/>
    public async Task<ResourceListResponse<SubscriptionOfferCodeOneTimeUseCode>> ListOneTimeUseCodesForSubscriptionOfferCodeAsync(
        string subscriptionOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionOfferCodeOneTimeUseCode>>(
                path: $"/v1/subscriptionOfferCodes/{subscriptionOfferCodeId}/oneTimeUseCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prices of a subscription offer code, one per territory the offer is available in.
    /// Filter by territory with <c>filter[territory]</c>.
    /// </summary>
    /// <param name="subscriptionOfferCodeId">The opaque resource ID of the Subscription Offer Codes resource.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Offer Code Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodes-_id_-prices"/>
    public async Task<ResourceListResponse<SubscriptionOfferCodePrice>> ListPricesForSubscriptionOfferCodeAsync(
        string subscriptionOfferCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionOfferCodePrice>>(
                path: $"/v1/subscriptionOfferCodes/{subscriptionOfferCodeId}/prices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a custom code for a subscription offer code: one code you choose, which many
    /// customers can redeem.
    /// </summary>
    /// <param name="request">The custom code to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Offer Code Custom Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionoffercodecustomcodes"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeCustomCode>> CreateSubscriptionOfferCodeCustomCodeAsync(
        SubscriptionOfferCodeCustomCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeCustomCode>>(
                path: "/v1/subscriptionOfferCodeCustomCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single custom code of a subscription offer code.
    /// </summary>
    /// <param name="subscriptionOfferCodeCustomCodeId">The opaque resource ID of the Subscription Offer Code Custom Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a Subscription Offer Code Custom Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodecustomcodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeCustomCode>> GetSubscriptionOfferCodeCustomCodeAsync(
        string subscriptionOfferCodeCustomCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeCustomCode>>(
                path: $"/v1/subscriptionOfferCodeCustomCodes/{subscriptionOfferCodeCustomCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates a custom code of a subscription offer code, which stops customers from redeeming
    /// it. Deactivation is permanent.
    /// </summary>
    /// <param name="subscriptionOfferCodeCustomCodeId">The opaque resource ID of the Subscription Offer Code Custom Codes resource.</param>
    /// <param name="request">The change to apply to the custom code.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Offer Code Custom Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionoffercodecustomcodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeCustomCode>> UpdateSubscriptionOfferCodeCustomCodeAsync(
        string subscriptionOfferCodeCustomCodeId,
        SubscriptionOfferCodeCustomCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeCustomCode>>(
                path: $"/v1/subscriptionOfferCodeCustomCodes/{subscriptionOfferCodeCustomCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a batch of one-time use codes for a subscription offer code. Each generated code
    /// is unique, and a customer can redeem it once.
    /// </summary>
    /// <param name="request">The batch of codes to generate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Offer Code One-Time Use Codes resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionoffercodeonetimeusecodes"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>> CreateSubscriptionOfferCodeOneTimeUseCodeAsync(
        SubscriptionOfferCodeOneTimeUseCodeCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>>(
                path: "/v1/subscriptionOfferCodeOneTimeUseCodes",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the details of a single batch of one-time use codes of a subscription offer code.
    /// </summary>
    /// <param name="subscriptionOfferCodeOneTimeUseCodeId">The opaque resource ID of the Subscription Offer Code One-Time Use Codes resource.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a Subscription Offer Code One-Time Use Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodeonetimeusecodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>> GetSubscriptionOfferCodeOneTimeUseCodeAsync(
        string subscriptionOfferCodeOneTimeUseCodeId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>>(
                path: $"/v1/subscriptionOfferCodeOneTimeUseCodes/{subscriptionOfferCodeOneTimeUseCodeId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deactivates a batch of one-time use codes of a subscription offer code, which stops
    /// customers from redeeming any code in it. Deactivation is permanent.
    /// </summary>
    /// <param name="subscriptionOfferCodeOneTimeUseCodeId">The opaque resource ID of the Subscription Offer Code One-Time Use Codes resource.</param>
    /// <param name="request">The change to apply to the batch of codes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Offer Code One-Time Use Codes resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionoffercodeonetimeusecodes-_id_"/>
    public async Task<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>> UpdateSubscriptionOfferCodeOneTimeUseCodeAsync(
        string subscriptionOfferCodeOneTimeUseCodeId,
        SubscriptionOfferCodeOneTimeUseCodeUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionOfferCodeOneTimeUseCode>>(
                path: $"/v1/subscriptionOfferCodeOneTimeUseCodes/{subscriptionOfferCodeOneTimeUseCodeId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads the codes of a batch of one-time use codes of a subscription offer code, as a
    /// comma-separated value document you can hand out to customers.
    /// </summary>
    /// <param name="subscriptionOfferCodeOneTimeUseCodeId">The opaque resource ID of the Subscription Offer Code One-Time Use Codes resource.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The generated codes, as the text of a comma-separated value document.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionoffercodeonetimeusecodes-_id_-values"/>
    public async Task<string> GetValuesForSubscriptionOfferCodeOneTimeUseCodeAsync(
        string subscriptionOfferCodeOneTimeUseCodeId,
        CancellationToken cancellationToken = default)
    {
        return await MakeRawTextRequestAsync(
                path: $"/v1/subscriptionOfferCodeOneTimeUseCodes/{subscriptionOfferCodeOneTimeUseCodeId}/values",
                method: HttpMethod.Get,
                queryParameters: null,
                accept: "text/csv",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
