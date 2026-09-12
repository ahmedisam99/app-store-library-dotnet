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
    /// Finds and lists the builds of all your apps in App Store Connect.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Builds resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds"/>
    public async Task<ResourceListResponse<Build>> ListBuildsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Build>>(
                path: "/v1/builds",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the information of a single build.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Builds resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_"/>
    public async Task<ResourceResponse<Build>> GetBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Build>>(
                path: $"/v1/builds/{buildId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Expires a build, or changes its encryption exemption or its app encryption declaration.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build to update.</param>
    /// <param name="request">The request body that describes the changes to make.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Builds resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-builds-_id_"/>
    public async Task<ResourceResponse<Build>> UpdateBuildAsync(
        string buildId,
        BuildUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Build>>(
                path: $"/v1/builds/{buildId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the app a build belongs to.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Apps resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-app"/>
    public async Task<ResourceResponse<App>> GetAppForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<App>>(
                path: $"/v1/builds/{buildId}/app",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the TestFlight details, such as the internal and external testing states, of a build.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Build Beta Details resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-buildbetadetail"/>
    public async Task<ResourceResponse<BuildBetaDetail>> GetBetaDetailForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BuildBetaDetail>>(
                path: $"/v1/builds/{buildId}/buildBetaDetail",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the localized "What to Test" text of a build, one entry per locale.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Build Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-betabuildlocalizations"/>
    public async Task<ResourceListResponse<BetaBuildLocalization>> ListBetaBuildLocalizationsForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaBuildLocalization>>(
                path: $"/v1/builds/{buildId}/betaBuildLocalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the individual testers you assigned to a build directly, rather than through a group.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Testers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-individualtesters"/>
    public async Task<ResourceListResponse<BetaTester>> ListIndividualTestersForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaTester>>(
                path: $"/v1/builds/{buildId}/individualTesters",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the pre-release version a build belongs to.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Prerelease Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-prereleaseversion"/>
    public async Task<ResourceResponse<PrereleaseVersion>> GetPrereleaseVersionForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PrereleaseVersion>>(
                path: $"/v1/builds/{buildId}/preReleaseVersion",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the beta app review submission of a build, which tells you where the build stands in
    /// Apple's review for external testing.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Beta App Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-builds-_id_-betaappreviewsubmission"/>
    public async Task<ResourceResponse<BetaAppReviewSubmission>> GetBetaAppReviewSubmissionForBuildAsync(
        string buildId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaAppReviewSubmission>>(
                path: $"/v1/builds/{buildId}/betaAppReviewSubmission",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gives one or more beta groups access to a build.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="betaGroupIds">The opaque resource IDs of the beta groups to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta group IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-builds-_id_-relationships-betagroups"/>
    public async Task AddBetaGroupsToBuildAsync(
        string buildId,
        string[] betaGroupIds,
        CancellationToken cancellationToken = default)
    {
        if (betaGroupIds is null)
        {
            throw new ArgumentNullException(nameof(betaGroupIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/builds/{buildId}/relationships/betaGroups",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaGroups", betaGroupIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Takes one or more beta groups' access to a build away.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="betaGroupIds">The opaque resource IDs of the beta groups to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta group IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-builds-_id_-relationships-betagroups"/>
    public async Task RemoveBetaGroupsFromBuildAsync(
        string buildId,
        string[] betaGroupIds,
        CancellationToken cancellationToken = default)
    {
        if (betaGroupIds is null)
        {
            throw new ArgumentNullException(nameof(betaGroupIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/builds/{buildId}/relationships/betaGroups",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaGroups", betaGroupIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Gives one or more individual testers access to a build, without adding them to a group.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="betaTesterIds">The opaque resource IDs of the beta testers to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta tester IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-builds-_id_-relationships-individualtesters"/>
    public async Task AddIndividualTestersToBuildAsync(
        string buildId,
        string[] betaTesterIds,
        CancellationToken cancellationToken = default)
    {
        if (betaTesterIds is null)
        {
            throw new ArgumentNullException(nameof(betaTesterIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/builds/{buildId}/relationships/individualTesters",
                method: HttpMethod.Post,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaTesters", betaTesterIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Takes one or more individual testers' access to a build away.
    /// </summary>
    /// <param name="buildId">The opaque resource ID of the build.</param>
    /// <param name="betaTesterIds">The opaque resource IDs of the beta testers to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the beta tester IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-builds-_id_-relationships-individualtesters"/>
    public async Task RemoveIndividualTestersFromBuildAsync(
        string buildId,
        string[] betaTesterIds,
        CancellationToken cancellationToken = default)
    {
        if (betaTesterIds is null)
        {
            throw new ArgumentNullException(nameof(betaTesterIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/builds/{buildId}/relationships/individualTesters",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: RelationshipDeclarationList.To("betaTesters", betaTesterIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Finds and lists the build beta details of all builds.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Build Beta Details resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-buildbetadetails"/>
    public async Task<ResourceListResponse<BuildBetaDetail>> ListBuildBetaDetailsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BuildBetaDetail>>(
                path: "/v1/buildBetaDetails",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single build beta detail resource.
    /// </summary>
    /// <param name="buildBetaDetailId">The opaque resource ID of the build beta detail.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Build Beta Details resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-buildbetadetails-_id_"/>
    public async Task<ResourceResponse<BuildBetaDetail>> GetBuildBetaDetailAsync(
        string buildBetaDetailId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BuildBetaDetail>>(
                path: $"/v1/buildBetaDetails/{buildBetaDetailId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Turns the automatic notification of testers for a build on or off.
    /// </summary>
    /// <param name="buildBetaDetailId">The opaque resource ID of the build beta detail to update.</param>
    /// <param name="request">The request body that describes the changes to make.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Build Beta Details resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-buildbetadetails-_id_"/>
    public async Task<ResourceResponse<BuildBetaDetail>> UpdateBuildBetaDetailAsync(
        string buildBetaDetailId,
        BuildBetaDetailUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BuildBetaDetail>>(
                path: $"/v1/buildBetaDetails/{buildBetaDetailId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Finds and lists beta build localizations, the localized "What to Test" text of your builds.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Build Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betabuildlocalizations"/>
    public async Task<ResourceListResponse<BetaBuildLocalization>> ListBetaBuildLocalizationsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaBuildLocalization>>(
                path: "/v1/betaBuildLocalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds "What to Test" text for a build in one locale.
    /// </summary>
    /// <param name="request">The request body that describes the localization to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Beta Build Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betabuildlocalizations"/>
    public async Task<ResourceResponse<BetaBuildLocalization>> CreateBetaBuildLocalizationAsync(
        BetaBuildLocalizationCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaBuildLocalization>>(
                path: "/v1/betaBuildLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the "What to Test" text of one build in one locale.
    /// </summary>
    /// <param name="betaBuildLocalizationId">The opaque resource ID of the localization.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Beta Build Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betabuildlocalizations-_id_"/>
    public async Task<ResourceResponse<BetaBuildLocalization>> GetBetaBuildLocalizationAsync(
        string betaBuildLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaBuildLocalization>>(
                path: $"/v1/betaBuildLocalizations/{betaBuildLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Rewrites the "What to Test" text of one build in one locale.
    /// </summary>
    /// <param name="betaBuildLocalizationId">The opaque resource ID of the localization to update.</param>
    /// <param name="request">The request body that describes the changes to make.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Beta Build Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-betabuildlocalizations-_id_"/>
    public async Task<ResourceResponse<BetaBuildLocalization>> UpdateBetaBuildLocalizationAsync(
        string betaBuildLocalizationId,
        BetaBuildLocalizationUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaBuildLocalization>>(
                path: $"/v1/betaBuildLocalizations/{betaBuildLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the "What to Test" text of one build in one locale.
    /// </summary>
    /// <param name="betaBuildLocalizationId">The opaque resource ID of the localization to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-betabuildlocalizations-_id_"/>
    public async Task DeleteBetaBuildLocalizationAsync(
        string betaBuildLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/betaBuildLocalizations/{betaBuildLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Finds and lists the beta app review submissions of one or more builds.
    /// </summary>
    /// <param name="buildIds">The opaque resource IDs of the builds to list submissions for. At least one build ID is required.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta App Review Submissions resources.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the build IDs are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the build IDs are empty, or one of them is <c>null</c> or blank.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betaappreviewsubmissions"/>
    public async Task<ResourceListResponse<BetaAppReviewSubmission>> ListBetaAppReviewSubmissionsAsync(
        string[] buildIds,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        if (buildIds is null)
        {
            throw new ArgumentNullException(nameof(buildIds));
        }

        if (buildIds.Length == 0)
        {
            throw new ArgumentException("At least one build ID is required.", nameof(buildIds));
        }

        foreach (var buildId in buildIds)
        {
            if (string.IsNullOrWhiteSpace(buildId))
            {
                throw new ArgumentException("A build ID is null or blank.", nameof(buildIds));
            }
        }

        var queryParameters = query?.ToQueryParameters() ?? new Dictionary<string, string[]>(StringComparer.Ordinal);
        queryParameters["filter[build]"] = buildIds;

        return await MakeRequestAsync<ResourceListResponse<BetaAppReviewSubmission>>(
                path: "/v1/betaAppReviewSubmissions",
                method: HttpMethod.Get,
                queryParameters: queryParameters,
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits a build for the beta review that external testing needs.
    /// </summary>
    /// <param name="request">The request body that names the build to submit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Beta App Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-betaappreviewsubmissions"/>
    public async Task<ResourceResponse<BetaAppReviewSubmission>> CreateBetaAppReviewSubmissionAsync(
        BetaAppReviewSubmissionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaAppReviewSubmission>>(
                path: "/v1/betaAppReviewSubmissions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single beta app review submission.
    /// </summary>
    /// <param name="betaAppReviewSubmissionId">The opaque resource ID of the submission.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Beta App Review Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-betaappreviewsubmissions-_id_"/>
    public async Task<ResourceResponse<BetaAppReviewSubmission>> GetBetaAppReviewSubmissionAsync(
        string betaAppReviewSubmissionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<BetaAppReviewSubmission>>(
                path: $"/v1/betaAppReviewSubmissions/{betaAppReviewSubmissionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Finds and lists the pre-release versions of your apps, the version numbers you uploaded
    /// builds for.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Prerelease Versions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-prereleaseversions"/>
    public async Task<ResourceListResponse<PrereleaseVersion>> ListPrereleaseVersionsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<PrereleaseVersion>>(
                path: "/v1/preReleaseVersions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single pre-release version.
    /// </summary>
    /// <param name="prereleaseVersionId">The opaque resource ID of the pre-release version.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Prerelease Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-prereleaseversions-_id_"/>
    public async Task<ResourceResponse<PrereleaseVersion>> GetPrereleaseVersionAsync(
        string prereleaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PrereleaseVersion>>(
                path: $"/v1/preReleaseVersions/{prereleaseVersionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the builds you uploaded for one pre-release version.
    /// </summary>
    /// <param name="prereleaseVersionId">The opaque resource ID of the pre-release version.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Builds resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-prereleaseversions-_id_-builds"/>
    public async Task<ResourceListResponse<Build>> ListBuildsForPrereleaseVersionAsync(
        string prereleaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Build>>(
                path: $"/v1/preReleaseVersions/{prereleaseVersionId}/builds",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
