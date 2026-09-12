using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Finds and lists the apps of your team in App Store Connect.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Apps resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps"/>
    public async Task<ResourceListResponse<App>> ListAppsAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<App>>(
                path: "/v1/apps",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads information about a single app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and related-resource limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an Apps resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_"/>
    public async Task<ResourceResponse<App>> GetAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<App>>(
                path: $"/v1/apps/{appId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Updates an app's bundle ID, primary locale, subscription status URLs, content rights
    /// declaration, or streamlined purchasing setting.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="request">The request body that describes the changes to make.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Apps resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-apps-_id_"/>
    public async Task<ResourceResponse<App>> UpdateAppAsync(
        string appId,
        AppUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<App>>(
                path: $"/v1/apps/{appId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the in-app purchases of an app, excluding its auto-renewable subscriptions.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchases resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-inapppurchasesv2"/>
    public async Task<ResourceListResponse<InAppPurchaseV2>> ListInAppPurchasesForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseV2>>(
                path: $"/v1/apps/{appId}/inAppPurchasesV2",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the subscription groups of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, sorting, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Groups resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-subscriptiongroups"/>
    public async Task<ResourceListResponse<SubscriptionGroup>> ListSubscriptionGroupsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionGroup>>(
                path: $"/v1/apps/{appId}/subscriptionGroups",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the in-app purchases and subscriptions of an app that the App Store promotes on the
    /// app's product page.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Promoted Purchases resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-promotedpurchases"/>
    public async Task<ResourceListResponse<PromotedPurchase>> ListPromotedPurchasesForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<PromotedPurchase>>(
                path: $"/v1/apps/{appId}/promotedPurchases",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the billing grace period settings that apply to the subscriptions of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a Subscription Grace Periods resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-subscriptiongraceperiod"/>
    public async Task<ResourceResponse<SubscriptionGracePeriod>> GetSubscriptionGracePeriodForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGracePeriod>>(
                path: $"/v1/apps/{appId}/subscriptionGracePeriod",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the builds of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Builds resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-builds"/>
    public async Task<ResourceListResponse<Build>> ListBuildsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Build>>(
                path: $"/v1/apps/{appId}/builds",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the beta groups of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Beta Groups resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-betagroups"/>
    public async Task<ResourceListResponse<BetaGroup>> ListBetaGroupsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<BetaGroup>>(
                path: $"/v1/apps/{appId}/betaGroups",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prerelease versions of an app.
    /// </summary>
    /// <param name="appId">The opaque resource ID of the app.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Prerelease Versions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-prereleaseversions"/>
    public async Task<ResourceListResponse<PrereleaseVersion>> ListPrereleaseVersionsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<PrereleaseVersion>>(
                path: $"/v1/apps/{appId}/preReleaseVersions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the App Store territories, and the currency each one charges customers in.
    /// </summary>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Territories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-territories"/>
    public async Task<ResourceListResponse<Territory>> ListTerritoriesAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Territory>>(
                path: "/v1/territories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
