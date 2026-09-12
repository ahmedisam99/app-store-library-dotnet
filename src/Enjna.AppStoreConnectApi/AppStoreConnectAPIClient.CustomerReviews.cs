using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Reads the customer reviews people wrote for an app on the App Store, newest first by default.
    /// Filter by territory or star rating, or narrow the list to the reviews you have or haven't
    /// answered yet with <c>exists[publishedResponse]</c>.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app to read the reviews of.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Customer Reviews resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-customerreviews"/>
    public async Task<ResourceListResponse<CustomerReview>> ListCustomerReviewsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<CustomerReview>>(
                path: $"/v1/apps/{appId}/customerReviews",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single customer review.
    /// </summary>
    /// <param name="customerReviewId">The opaque resource ID of the customer review to read.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Customer Reviews resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-customerreviews-_id_"/>
    public async Task<ResourceResponse<CustomerReview>> GetCustomerReviewAsync(
        string customerReviewId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<CustomerReview>>(
                path: $"/v1/customerReviews/{customerReviewId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the developer response to a customer review, by way of the review it answers.
    /// </summary>
    /// <param name="customerReviewId">The opaque resource ID of the customer review to read the response of.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Customer Review Responses resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-customerreviews-_id_-response"/>
    public async Task<ResourceResponse<CustomerReviewResponse>> GetResponseForCustomerReviewAsync(
        string customerReviewId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<CustomerReviewResponse>>(
                path: $"/v1/customerReviews/{customerReviewId}/response",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Responds to a customer review, or replaces the response the review already has. The response
    /// goes live on the App Store once Apple publishes it.
    /// </summary>
    /// <param name="request">The response text, and the customer review it answers.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Customer Review Responses resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-customerreviewresponses"/>
    public async Task<ResourceResponse<CustomerReviewResponse>> CreateCustomerReviewResponseAsync(
        CustomerReviewResponseV1CreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<CustomerReviewResponse>>(
                path: "/v1/customerReviewResponses",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single developer response to a customer review.
    /// </summary>
    /// <param name="customerReviewResponseId">The opaque resource ID of the response to read.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Customer Review Responses resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-customerreviewresponses-_id_"/>
    public async Task<ResourceResponse<CustomerReviewResponse>> GetCustomerReviewResponseAsync(
        string customerReviewResponseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<CustomerReviewResponse>>(
                path: $"/v1/customerReviewResponses/{customerReviewResponseId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a developer response to a customer review, which removes it from the App Store.
    /// </summary>
    /// <param name="customerReviewResponseId">The opaque resource ID of the response to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the response.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-customerreviewresponses-_id_"/>
    public async Task DeleteCustomerReviewResponseAsync(
        string customerReviewResponseId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/customerReviewResponses/{customerReviewResponseId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
