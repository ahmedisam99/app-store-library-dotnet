using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Lists the recent and current review submissions of an app. Narrow the list by platform or
    /// state with <c>filter[platform]</c> and <c>filter[state]</c>.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app to list the review submissions of. It goes out as <c>filter[app]</c>, which the endpoint requires.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits. Leave <c>filter[app]</c> out, because <paramref name="appId"/> supplies it.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Review Submissions resources.</returns>
    /// <exception cref="ArgumentException">Thrown when the query already sets <c>filter[app]</c> to something other than <paramref name="appId"/>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-reviewsubmissions"/>
    public async Task<ResourceListResponse<ReviewSubmission>> ListReviewSubmissionsAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var queryParameters = query?.ToQueryParameters() ?? new Dictionary<string, string[]>(StringComparer.Ordinal);
        SetRequiredFilter(queryParameters, "filter[app]", appId, nameof(query));

        return await MakeRequestAsync<ResourceListResponse<ReviewSubmission>>(
                path: "/v1/reviewSubmissions",
                method: HttpMethod.Get,
                queryParameters: queryParameters,
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the review submissions of an app, by way of the app. Narrow the list by platform or
    /// state with <c>filter[platform]</c> and <c>filter[state]</c>.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app to list the review submissions of.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Review Submissions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-reviewsubmissions"/>
    public async Task<ResourceListResponse<ReviewSubmission>> ListReviewSubmissionsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<ReviewSubmission>>(
                path: $"/v1/apps/{appId}/reviewSubmissions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a review submission for an app, the first of the three steps that send versions to
    /// App Review. Add each version to it with
    /// <see cref="CreateReviewSubmissionItemAsync(ReviewSubmissionItemCreateRequest, CancellationToken)"/>,
    /// then submit it with
    /// <see cref="UpdateReviewSubmissionAsync(string, ReviewSubmissionUpdateRequest, CancellationToken)"/>.
    /// </summary>
    /// <remarks>
    /// The platform is no longer required when you create a submission, and you can add it when
    /// you update the submission instead. Submit your first in-app purchase, and your first
    /// subscription, together with an app binary submission through App Store Connect, because
    /// the review submission calls don't apply to that first submission.
    /// </remarks>
    /// <param name="request">The app to create the submission for, and optionally its platform.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-reviewsubmissions"/>
    public async Task<ResourceResponse<ReviewSubmission>> CreateReviewSubmissionAsync(
        ReviewSubmissionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<ReviewSubmission>>(
                path: "/v1/reviewSubmissions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single review submission, including its state and when it went to App Review.
    /// </summary>
    /// <param name="reviewSubmissionId">The opaque resource ID of the review submission to read.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and the limit on included items.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-reviewsubmissions-_id_"/>
    public async Task<ResourceResponse<ReviewSubmission>> GetReviewSubmissionAsync(
        string reviewSubmissionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<ReviewSubmission>>(
                path: $"/v1/reviewSubmissions/{reviewSubmissionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes a review submission. Set
    /// <see cref="ReviewSubmissionUpdateRequestDataAttributes.Submitted"/> to <c>true</c> to send it
    /// to App Review once its items are in place, which is the last of the three steps, or set
    /// <see cref="ReviewSubmissionUpdateRequestDataAttributes.Canceled"/> to <c>true</c> to cancel it.
    /// You can also add the platform here if you left it out when you created the submission.
    /// </summary>
    /// <remarks>
    /// An in-app purchase, subscription, or subscription group version in the submission moves from
    /// <c>READY_FOR_REVIEW</c> to <c>WAITING_FOR_REVIEW</c> after you mark the submission submitted,
    /// then to <c>IN_REVIEW</c>, and finally to <c>APPROVED</c> or <c>REJECTED</c>.
    /// </remarks>
    /// <param name="reviewSubmissionId">The opaque resource ID of the review submission to change.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-reviewsubmissions-_id_"/>
    public async Task<ResourceResponse<ReviewSubmission>> UpdateReviewSubmissionAsync(
        string reviewSubmissionId,
        ReviewSubmissionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<ReviewSubmission>>(
                path: $"/v1/reviewSubmissions/{reviewSubmissionId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the items in a review submission. Each item names one thing to review, such as an
    /// App Store version or an in-app purchase version.
    /// </summary>
    /// <param name="reviewSubmissionId">The opaque resource ID of the review submission to list the items of.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Review Submission Items resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-reviewsubmissions-_id_-items"/>
    public async Task<ResourceListResponse<ReviewSubmissionItem>> ListItemsForReviewSubmissionAsync(
        string reviewSubmissionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<ReviewSubmissionItem>>(
                path: $"/v1/reviewSubmissions/{reviewSubmissionId}/items",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an item to a review submission, the second of the three steps that send versions to
    /// App Review. An item stands for one thing to review, such as an in-app purchase version,
    /// so add one item for each version you submit.
    /// </summary>
    /// <remarks>
    /// An in-app purchase, subscription, or subscription group version moves from
    /// <c>PREPARE_FOR_SUBMISSION</c> to <c>READY_FOR_REVIEW</c> when you add it to a review
    /// submission.
    /// </remarks>
    /// <param name="request">The review submission to add the item to, and the one thing it names for review.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Review Submission Items resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-reviewsubmissionitems"/>
    public async Task<ResourceResponse<ReviewSubmissionItem>> CreateReviewSubmissionItemAsync(
        ReviewSubmissionItemCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<ReviewSubmissionItem>>(
                path: "/v1/reviewSubmissionItems",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes an item in a review submission, marking it resolved or removed.
    /// </summary>
    /// <param name="reviewSubmissionItemId">The opaque resource ID of the review submission item to change.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Review Submission Items resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-reviewsubmissionitems-_id_"/>
    public async Task<ResourceResponse<ReviewSubmissionItem>> UpdateReviewSubmissionItemAsync(
        string reviewSubmissionItemId,
        ReviewSubmissionItemUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<ReviewSubmissionItem>>(
                path: $"/v1/reviewSubmissionItems/{reviewSubmissionItemId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes an item from a review submission.
    /// </summary>
    /// <param name="reviewSubmissionItemId">The opaque resource ID of the review submission item to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect removed the item.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-reviewsubmissionitems-_id_"/>
    public async Task DeleteReviewSubmissionItemAsync(
        string reviewSubmissionItemId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/reviewSubmissionItems/{reviewSubmissionItemId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
