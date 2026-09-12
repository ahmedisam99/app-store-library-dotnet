using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Creates a subscription group for an app. Every auto-renewable subscription belongs to a
    /// group, so create the group before you create the subscriptions it holds.
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
    /// Lists the localizations of a subscription group, one per language you translated the group's
    /// display name into.
    /// </summary>
    /// <param name="subscriptionGroupId">The opaque resource ID of the subscription group to read the localizations of.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Group Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongroups-_id_-subscriptiongrouplocalizations"/>
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
    /// </summary>
    /// <param name="request">The localization to create, including the subscription group it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongrouplocalizations"/>
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
    /// Reads a single subscription group localization.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to read.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongrouplocalizations-_id_"/>
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
    /// locale is fixed once you create the localization.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to update.</param>
    /// <param name="request">The attributes to change on the localization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Group Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptiongrouplocalizations-_id_"/>
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
    /// Deletes a subscription group localization, removing the group's translation for that language.
    /// </summary>
    /// <param name="subscriptionGroupLocalizationId">The opaque resource ID of the localization to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the localization.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptiongrouplocalizations-_id_"/>
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
    /// submit goes for review together, without waiting for a new app version.
    /// </summary>
    /// <param name="request">The submission to create, pointing at the subscription group to submit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Group Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptiongroupsubmissions"/>
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
