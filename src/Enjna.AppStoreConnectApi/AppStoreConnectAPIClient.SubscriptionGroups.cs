using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Creates a subscription group for an app. Every auto-renewable subscription belongs to a
    /// group, so create the group before you create the subscriptions it holds. The group's localized
    /// names go on a version of the group. Create the version with
    /// <see cref="CreateSubscriptionGroupVersionAsync"/>, then add each localization to it with
    /// <see cref="CreateSubscriptionGroupLocalizationV2Async"/>.
    /// </summary>
    /// <param name="request">The subscription group to create, including the app it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongroups"/>
    public async Task<ResourceResponse<SubscriptionGroup>> CreateSubscriptionGroupAsync(
        SubscriptionGroupCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroup>>(
                path: "/v1/subscriptionGroups",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single subscription group, optionally along with its subscriptions, its
    /// localizations, and its versions.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to read.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and per-relationship limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroups-_id_"/>
    public async Task<ResourceResponse<SubscriptionGroup>> GetSubscriptionGroupAsync(
        string subscriptionGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroup>>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Renames a subscription group. The reference name is internal to App Store Connect, so
    /// changing it never reaches customers; change what they see with the group's localizations.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to update.</param>
    /// <param name="request">The attributes to change on the subscription group.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptiongroups-_id_"/>
    public async Task<ResourceResponse<SubscriptionGroup>> UpdateSubscriptionGroupAsync(
        string subscriptionGroupId,
        SubscriptionGroupUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroup>>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a subscription group. A group that holds a subscription customers have already
    /// bought can't be deleted.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the subscription group.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptiongroups-_id_"/>
    public async Task DeleteSubscriptionGroupAsync(
        string subscriptionGroupId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the subscriptions in a subscription group. Filter by product ID, name, or state
    /// through the query.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to read the subscriptions of.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscriptions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroups-_id_-subscriptions"/>
    public async Task<ResourceListResponse<Subscription>> ListSubscriptionsForSubscriptionGroupAsync(
        string subscriptionGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Subscription>>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}/subscriptions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the versions of a subscription group. Each version carries its state, so you can find
    /// the current draft, the most recently approved version, and any version in review. Filter by
    /// state, or include each version's localizations, through the query.
    /// </summary>
    /// <remarks>
    /// Look for a draft here before you create a version with
    /// <see cref="CreateSubscriptionGroupVersionAsync"/>: filter <c>state</c> to
    /// <c>PREPARE_FOR_SUBMISSION</c>, and reuse the version you find.
    /// </remarks>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to read the versions of.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Group Versions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroups-_id_-versions"/>
    public async Task<ResourceListResponse<SubscriptionGroupVersion>> ListVersionsForSubscriptionGroupAsync(
        string subscriptionGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionGroupVersion>>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}/versions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a draft version of a subscription group, the container that the group's localized
    /// names attach to for one App Review cycle. The new version starts in
    /// <c>PREPARE_FOR_SUBMISSION</c>, the only state in which you
    /// can add, change, or remove its localizations. To change them once the version has left that
    /// state, create a new version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A subscription group may already have a draft version. So before you create one, list the
    /// group's versions with <see cref="ListVersionsForSubscriptionGroupAsync"/>, filtered by
    /// <c>state</c>, and reuse a draft you find. Apple describes a new version as capturing the
    /// group's current localized metadata.
    /// </para>
    /// <para>
    /// Group-level localizations go to App Review with the subscriptions in the group. Submit a
    /// subscription group version on its own only when you change the group's localizations without
    /// changing any of its subscriptions.
    /// </para>
    /// </remarks>
    /// <param name="request">The version to create, pointing at the subscription group it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongroupversions"/>
    public async Task<ResourceResponse<SubscriptionGroupVersion>> CreateSubscriptionGroupVersionAsync(
        SubscriptionGroupVersionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupVersion>>(
                path: "/v1/subscriptionGroupVersions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single subscription group version, optionally along with its subscription group and
    /// its localizations. After you submit the version, read it again to follow its state through
    /// App Review.
    /// </summary>
    /// <param name="subscriptionGroupVersionId">The opaque resource ID of the subscription group version to read.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and per-relationship limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Group Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroupversions-_id_"/>
    public async Task<ResourceResponse<SubscriptionGroupVersion>> GetSubscriptionGroupVersionAsync(
        string subscriptionGroupVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupVersion>>(
                path: $"/v1/subscriptionGroupVersions/{subscriptionGroupVersionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the localizations of a subscription group version, one per language you translated the
    /// group's display name into for that version.
    /// </summary>
    /// <param name="subscriptionGroupVersionId">The opaque resource ID of the subscription group version to read the localizations of.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Group Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroupversions-_id_-localizations"/>
    public async Task<ResourceListResponse<SubscriptionGroupLocalizationV2>> ListLocalizationsForSubscriptionGroupVersionAsync(
        string subscriptionGroupVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionGroupLocalizationV2>>(
                path: $"/v1/subscriptionGroupVersions/{subscriptionGroupVersionId}/localizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Translates a subscription group into one more language by creating a localization on a
    /// subscription group version. The localization relates to the version, not to the subscription
    /// group, and you can add one only while the version is in
    /// <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    /// <param name="request">The localization to create, including the subscription group version it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v2-subscriptiongrouplocalizations"/>
    public async Task<ResourceResponse<SubscriptionGroupLocalizationV2>> CreateSubscriptionGroupLocalizationV2Async(
        SubscriptionGroupLocalizationV2CreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalizationV2>>(
                path: "/v2/subscriptionGroupLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single subscription group localization, optionally along with the version it belongs to.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to read.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-subscriptiongrouplocalizations-_id_"/>
    public async Task<ResourceResponse<SubscriptionGroupLocalizationV2>> GetSubscriptionGroupLocalizationV2Async(
        string subscriptionGroupLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalizationV2>>(
                path: $"/v2/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the display name, or the custom app name, of a subscription group localization. The
    /// locale is fixed once you create the localization, and you can change the localization only
    /// while its version is in <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to update.</param>
    /// <param name="request">The attributes to change on the localization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v2-subscriptiongrouplocalizations-_id_"/>
    public async Task<ResourceResponse<SubscriptionGroupLocalizationV2>> UpdateSubscriptionGroupLocalizationV2Async(
        string subscriptionGroupLocalizationId,
        SubscriptionGroupLocalizationV2UpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalizationV2>>(
                path: $"/v2/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a subscription group localization, removing the group's translation for that language
    /// from its version. You can remove a localization only while its version is in
    /// <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the localization.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v2-subscriptiongrouplocalizations-_id_"/>
    public async Task DeleteSubscriptionGroupLocalizationV2Async(
        string subscriptionGroupLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v2/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the localizations of a subscription group, one per language you translated the group's
    /// display name into. Apple replaced this endpoint with
    /// <see cref="ListLocalizationsForSubscriptionGroupVersionAsync"/>, which lists the localizations
    /// of one version of the group.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to read the localizations of.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Group Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroups-_id_-subscriptiongrouplocalizations"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use ListLocalizationsForSubscriptionGroupVersionAsync instead.")]
    public async Task<ResourceListResponse<SubscriptionGroupLocalization>> ListLocalizationsForSubscriptionGroupAsync(
        string subscriptionGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionGroupLocalization>>(
                path: $"/v1/subscriptionGroups/{subscriptionGroupId}/subscriptionGroupLocalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Translates a subscription group into one more language by creating a localization for it.
    /// Apple replaced this endpoint with <see cref="CreateSubscriptionGroupLocalizationV2Async"/>,
    /// which creates the localization on a subscription group version.
    /// </summary>
    /// <param name="request">The localization to create, including the subscription group it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongrouplocalizations"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use CreateSubscriptionGroupLocalizationV2Async instead.")]
    public async Task<ResourceResponse<SubscriptionGroupLocalization>> CreateSubscriptionGroupLocalizationAsync(
        SubscriptionGroupLocalizationCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalization>>(
                path: "/v1/subscriptionGroupLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single subscription group localization. Apple replaced this endpoint with
    /// <see cref="GetSubscriptionGroupLocalizationV2Async"/>, which reads a localization that belongs
    /// to a subscription group version.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to read.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongrouplocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use GetSubscriptionGroupLocalizationV2Async instead.")]
    public async Task<ResourceResponse<SubscriptionGroupLocalization>> GetSubscriptionGroupLocalizationAsync(
        string subscriptionGroupLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalization>>(
                path: $"/v1/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the display name, or the custom app name, of a subscription group localization. The
    /// locale is fixed once you create the localization. Apple replaced this endpoint with
    /// <see cref="UpdateSubscriptionGroupLocalizationV2Async"/>, which updates a localization that
    /// belongs to a subscription group version.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to update.</param>
    /// <param name="request">The attributes to change on the localization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptiongrouplocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use UpdateSubscriptionGroupLocalizationV2Async instead.")]
    public async Task<ResourceResponse<SubscriptionGroupLocalization>> UpdateSubscriptionGroupLocalizationAsync(
        string subscriptionGroupLocalizationId,
        SubscriptionGroupLocalizationUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupLocalization>>(
                path: $"/v1/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a subscription group localization, removing the group's translation for that
    /// language. Apple replaced this endpoint with
    /// <see cref="DeleteSubscriptionGroupLocalizationV2Async"/>, which deletes a localization that
    /// belongs to a subscription group version.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the localization.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptiongrouplocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use DeleteSubscriptionGroupLocalizationV2Async instead.")]
    public async Task DeleteSubscriptionGroupLocalizationAsync(
        string subscriptionGroupLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionGroupLocalizations/{subscriptionGroupLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits a subscription group for App Store review. Everything in the group that is ready to
    /// submit goes for review together, without waiting for a new app version. Apple replaced this
    /// endpoint with the review submission workflow, which submits a subscription group version:
    /// create a review submission with <see cref="CreateReviewSubmissionAsync"/>, add the version
    /// to it with <see cref="CreateReviewSubmissionItemAsync"/>, and submit it with
    /// <see cref="UpdateReviewSubmissionAsync"/>.
    /// </summary>
    /// <param name="request">The submission to create, pointing at the subscription group to submit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongroupsubmissions"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Submit through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
    public async Task<ResourceResponse<SubscriptionGroupSubmission>> CreateSubscriptionGroupSubmissionAsync(
        SubscriptionGroupSubmissionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGroupSubmission>>(
                path: "/v1/subscriptionGroupSubmissions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
