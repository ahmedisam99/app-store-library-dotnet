using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Finds and lists the beta groups of all your apps.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Groups resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betagroups"/>
    public async Task<ResourceListResponse<BetaGroup>> ListBetaGroupsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaGroup>>(
                path: "/v1/betaGroups",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a beta group associated with an app, optionally enabling TestFlight public links.
    /// </summary>
    /// <param name="request">The request body that describes the beta group to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the newly created Beta Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betagroups"/>
    public async Task<ResourceResponse<BetaGroup>> CreateBetaGroupAsync(
        BetaGroupCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaGroup>>(
                path: "/v1/betaGroups",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single beta group.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and related-resource limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Beta Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betagroups-_id_"/>
    public async Task<ResourceResponse<BetaGroup>> GetBetaGroupAsync(
        string betaGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaGroup>>(
                path: $"/v1/betaGroups/{betaGroupId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Modifies a beta group's name, public link settings, and feedback settings.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="request">The request body that describes the changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Beta Groups resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-betagroups-_id_"/>
    public async Task<ResourceResponse<BetaGroup>> UpdateBetaGroupAsync(
        string betaGroupId,
        BetaGroupUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaGroup>>(
                path: $"/v1/betaGroups/{betaGroupId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a beta group and removes all of its testers' access to test the app.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betagroups-_id_"/>
    public async Task DeleteBetaGroupAsync(
        string betaGroupId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/betaGroups/{betaGroupId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the beta testers a beta group contains.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Testers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betagroups-_id_-betatesters"/>
    public async Task<ResourceListResponse<BetaTester>> ListBetaTestersForBetaGroupAsync(
        string betaGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaTester>>(
                path: $"/v1/betaGroups/{betaGroupId}/betaTesters",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the builds a beta group can test.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Builds resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betagroups-_id_-builds"/>
    public async Task<ResourceListResponse<Build>> ListBuildsForBetaGroupAsync(
        string betaGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Build>>(
                path: $"/v1/betaGroups/{betaGroupId}/builds",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the app a beta group belongs to.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Apps resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betagroups-_id_-app"/>
    public async Task<ResourceResponse<App>> GetAppForBetaGroupAsync(
        string betaGroupId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<App>>(
                path: $"/v1/betaGroups/{betaGroupId}/app",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds beta testers to a beta group, which lets them test the builds the group has access to.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="betaTesterIds">The opaque resource IDs of the beta testers to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta tester IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betagroups-_id_-relationships-betatesters"/>
    public async Task AddBetaTestersToBetaGroupAsync(
        string betaGroupId,
        string[] betaTesterIds,
        CancellationToken cancellationToken = default)
    {
        if (betaTesterIds is null)
        {
            throw new ArgumentNullException(nameof(betaTesterIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaGroups/{betaGroupId}/relationships/betaTesters",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaTesters", betaTesterIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes beta testers from a beta group, which revokes their access to the group's builds.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="betaTesterIds">The opaque resource IDs of the beta testers to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta tester IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betagroups-_id_-relationships-betatesters"/>
    public async Task RemoveBetaTestersFromBetaGroupAsync(
        string betaGroupId,
        string[] betaTesterIds,
        CancellationToken cancellationToken = default)
    {
        if (betaTesterIds is null)
        {
            throw new ArgumentNullException(nameof(betaTesterIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaGroups/{betaGroupId}/relationships/betaTesters",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaTesters", betaTesterIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds builds to a beta group, which makes them available to the group's testers.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="buildIds">The opaque resource IDs of the builds to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the build IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betagroups-_id_-relationships-builds"/>
    public async Task AddBuildsToBetaGroupAsync(
        string betaGroupId,
        string[] buildIds,
        CancellationToken cancellationToken = default)
    {
        if (buildIds is null)
        {
            throw new ArgumentNullException(nameof(buildIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaGroups/{betaGroupId}/relationships/builds",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("builds", buildIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes builds from a beta group, which stops the group's testers from installing them.
    /// </summary>
    /// <param name="betaGroupId">The opaque resource ID of the beta group.</param>
    /// <param name="buildIds">The opaque resource IDs of the builds to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the build IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betagroups-_id_-relationships-builds"/>
    public async Task RemoveBuildsFromBetaGroupAsync(
        string betaGroupId,
        string[] buildIds,
        CancellationToken cancellationToken = default)
    {
        if (buildIds is null)
        {
            throw new ArgumentNullException(nameof(buildIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaGroups/{betaGroupId}/relationships/builds",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("builds", buildIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Finds and lists the beta testers of all apps, builds, and beta groups.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Testers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betatesters"/>
    public async Task<ResourceListResponse<BetaTester>> ListBetaTestersAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaTester>>(
                path: "/v1/betaTesters",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a beta tester assigned to a group, a build, or an app.
    /// </summary>
    /// <param name="request">The request body that describes the beta tester to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the newly created Beta Testers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betatesters"/>
    public async Task<ResourceResponse<BetaTester>> CreateBetaTesterAsync(
        BetaTesterCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaTester>>(
                path: "/v1/betaTesters",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single beta tester.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and related-resource limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Beta Testers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betatesters-_id_"/>
    public async Task<ResourceResponse<BetaTester>> GetBetaTesterAsync(
        string betaTesterId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaTester>>(
                path: $"/v1/betaTesters/{betaTesterId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a beta tester's ability to test all apps.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betatesters-_id_"/>
    public async Task DeleteBetaTesterAsync(
        string betaTesterId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/betaTesters/{betaTesterId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the apps a beta tester can test.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Apps resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betatesters-_id_-apps"/>
    public async Task<ResourceListResponse<App>> ListAppsForBetaTesterAsync(
        string betaTesterId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<App>>(
                path: $"/v1/betaTesters/{betaTesterId}/apps",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the beta groups a beta tester belongs to.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Groups resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betatesters-_id_-betagroups"/>
    public async Task<ResourceListResponse<BetaGroup>> ListBetaGroupsForBetaTesterAsync(
        string betaTesterId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaGroup>>(
                path: $"/v1/betaTesters/{betaTesterId}/betaGroups",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the builds an individually assigned beta tester can test.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Builds resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betatesters-_id_-builds"/>
    public async Task<ResourceListResponse<Build>> ListBuildsForBetaTesterAsync(
        string betaTesterId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Build>>(
                path: $"/v1/betaTesters/{betaTesterId}/builds",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a beta tester to one or more beta groups.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="betaGroupIds">The opaque resource IDs of the beta groups to join.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta group IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betatesters-_id_-relationships-betagroups"/>
    public async Task AddBetaGroupsToBetaTesterAsync(
        string betaTesterId,
        string[] betaGroupIds,
        CancellationToken cancellationToken = default)
    {
        if (betaGroupIds is null)
        {
            throw new ArgumentNullException(nameof(betaGroupIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaTesters/{betaTesterId}/relationships/betaGroups",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaGroups", betaGroupIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a beta tester from one or more beta groups, revoking their access to test builds
    /// associated with those groups.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="betaGroupIds">The opaque resource IDs of the beta groups to leave.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta group IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betatesters-_id_-relationships-betagroups"/>
    public async Task RemoveBetaGroupsFromBetaTesterAsync(
        string betaTesterId,
        string[] betaGroupIds,
        CancellationToken cancellationToken = default)
    {
        if (betaGroupIds is null)
        {
            throw new ArgumentNullException(nameof(betaGroupIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaTesters/{betaTesterId}/relationships/betaGroups",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaGroups", betaGroupIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Individually assigns a beta tester to one or more builds.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="buildIds">The opaque resource IDs of the builds to assign.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the build IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betatesters-_id_-relationships-builds"/>
    public async Task AddBuildsToBetaTesterAsync(
        string betaTesterId,
        string[] buildIds,
        CancellationToken cancellationToken = default)
    {
        if (buildIds is null)
        {
            throw new ArgumentNullException(nameof(buildIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaTesters/{betaTesterId}/relationships/builds",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("builds", buildIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes an individually assigned beta tester's access to test one or more builds.
    /// </summary>
    /// <param name="betaTesterId">The opaque resource ID of the beta tester.</param>
    /// <param name="buildIds">The opaque resource IDs of the builds to unassign.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the build IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betatesters-_id_-relationships-builds"/>
    public async Task RemoveBuildsFromBetaTesterAsync(
        string betaTesterId,
        string[] buildIds,
        CancellationToken cancellationToken = default)
    {
        if (buildIds is null)
        {
            throw new ArgumentNullException(nameof(buildIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/betaTesters/{betaTesterId}/relationships/builds",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("builds", buildIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes one or more beta testers' access to test any builds of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="betaTesterIds">The opaque resource IDs of the beta testers to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta tester IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-apps-_id_-relationships-betatesters"/>
    public async Task RemoveBetaTestersFromAppAsync(
        string appId,
        string[] betaTesterIds,
        CancellationToken cancellationToken = default)
    {
        if (betaTesterIds is null)
        {
            throw new ArgumentNullException(nameof(betaTesterIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/apps/{appId}/relationships/betaTesters",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaTesters", betaTesterIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a beta tester the TestFlight invitation for an app again. Use it for a tester who never
    /// opened the invitation App Store Connect sent when you added them to the app.
    /// </summary>
    /// <param name="request">The request body that describes the invitation to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the newly created Beta Tester Invitations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betatesterinvitations"/>
    public async Task<ResourceResponse<BetaTesterInvitation>> CreateBetaTesterInvitationAsync(
        BetaTesterInvitationCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaTesterInvitation>>(
                path: "/v1/betaTesterInvitations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
