using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Finds and lists the App Store categories an app can claim, both the top-level categories and
    /// the subcategories underneath them.
    /// </summary>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of App Categories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-appcategories"/>
    public async Task<ResourceListResponse<AppCategory>> ListAppCategoriesAsync(
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AppCategory>>(
                path: "/v1/appCategories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads information about a single App Store category.
    /// </summary>
    /// <param name="appCategoryId">The opaque resource ID of the app category.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single App Categories resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-appcategories-_id_"/>
    public async Task<ResourceResponse<AppCategory>> GetAppCategoryAsync(
        string appCategoryId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AppCategory>>(
                path: $"/v1/appCategories/{appCategoryId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the App Store category a subcategory belongs to.
    /// </summary>
    /// <param name="appCategoryId">The opaque resource ID of the app category.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single App Categories resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-appcategories-_id_-parent"/>
    public async Task<ResourceResponse<AppCategory>> GetParentForAppCategoryAsync(
        string appCategoryId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AppCategory>>(
                path: $"/v1/appCategories/{appCategoryId}/parent",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the subcategories that belong to an App Store category.
    /// </summary>
    /// <param name="appCategoryId">The opaque resource ID of the app category.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of App Categories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-appcategories-_id_-subcategories"/>
    public async Task<ResourceListResponse<AppCategory>> ListSubcategoriesForAppCategoryAsync(
        string appCategoryId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AppCategory>>(
                path: $"/v1/appCategories/{appCategoryId}/subcategories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Updates whether the App Store shows an app tag, which is how you opt an app out of a tag the
    /// App Store applied to it.
    /// </summary>
    /// <param name="appTagId">The opaque resource ID of the app tag.</param>
    /// <param name="request">The request body that describes the changes to make.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated App Tags resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-apptags-_id_"/>
    public async Task<ResourceResponse<AppTag>> UpdateAppTagAsync(
        string appTagId,
        AppTagUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AppTag>>(
                path: $"/v1/appTags/{appTagId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the App Store territories an app tag applies in.
    /// </summary>
    /// <param name="appTagId">The opaque resource ID of the app tag.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Territories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apptags-_id_-territories"/>
    public async Task<ResourceListResponse<Territory>> ListTerritoriesForAppTagAsync(
        string appTagId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Territory>>(
                path: $"/v1/appTags/{appTagId}/territories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
