using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Adds a consumable, non-consumable, or non-renewing in-app purchase to an app. The new in-app
    /// purchase starts out with no localizations, no price, and no availability, so fill those in
    /// before you submit it for review. Localizations go on a version of the in-app purchase rather
    /// than on the in-app purchase itself. Create the version with
    /// <see cref="CreateInAppPurchaseVersionAsync"/>, then add each localization to it with
    /// <see cref="CreateInAppPurchaseLocalizationV2Async"/>.
    /// </summary>
    /// <param name="request">The in-app purchase to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v2-inapppurchases"/>
    public async Task<ResourceResponse<InAppPurchaseV2>> CreateInAppPurchaseAsync(
        InAppPurchaseV2CreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseV2>>(
                path: "/v2/inAppPurchases",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase, including where it sits in the review workflow.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseV2>> GetInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseV2>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the reference name, the review note, or the Family Sharing setting of an in-app
    /// purchase. The product identifier and the type are fixed once you create it.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v2-inapppurchases-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseV2>> UpdateInAppPurchaseAsync(
        string inAppPurchaseId,
        InAppPurchaseV2UpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseV2>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an in-app purchase. You can only delete one that customers never bought, so an
    /// in-app purchase that was ever approved has to be removed from sale instead.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v2-inapppurchases-_id_"/>
    public async Task DeleteInAppPurchaseAsync(
        string inAppPurchaseId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the versions of an in-app purchase with the state of each, so you can find the current
    /// draft, the most recently approved version, and any version in review. A version is a draft
    /// container for one review cycle. It holds the localizations and images that go through App
    /// Review together. Its localizations and images are editable only while it is
    /// <c>PREPARE_FOR_SUBMISSION</c>, so to change the metadata after review, create a new version
    /// with <see cref="CreateInAppPurchaseVersionAsync"/>.
    /// </summary>
    /// <remarks>
    /// Look for a draft here before you create a version. Filter <c>state</c> on
    /// <c>PREPARE_FOR_SUBMISSION</c> and reuse the draft you find. Apple says each in-app purchase
    /// has a version, and in-app purchases whose localizations went through the deprecated v1
    /// endpoint have been seen already carrying a version 1 in <c>PREPARE_FOR_SUBMISSION</c>.
    /// </remarks>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as the state filter, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Versions resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-versions"/>
    public async Task<ResourceListResponse<InAppPurchaseVersion>> ListVersionsForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseVersion>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/versions",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the display names and descriptions an in-app purchase has, one per language. Apple
    /// replaced this endpoint with <see cref="ListLocalizationsForInAppPurchaseVersionAsync"/>,
    /// which lists the localizations of one version.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-inapppurchaselocalizations"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use ListLocalizationsForInAppPurchaseVersionAsync instead.")]
    public async Task<ResourceListResponse<InAppPurchaseLocalization>> ListLocalizationsForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseLocalization>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/inAppPurchaseLocalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the promotional images of an in-app purchase. Apple replaced this endpoint with
    /// <see cref="ListImagesForInAppPurchaseVersionAsync"/>, which lists the images of one version.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Images resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-images"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use ListImagesForInAppPurchaseVersionAsync instead.")]
    public async Task<ResourceListResponse<InAppPurchaseImage>> ListImagesForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseImage>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/images",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the price points an in-app purchase can be sold at. A price point fixes both an amount and
    /// the territory it applies in, and you refer to one by its resource ID when you set a price schedule.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Price Points resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-pricepoints"/>
    public async Task<ResourceListResponse<InAppPurchasePricePoint>> ListPricePointsForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchasePricePoint>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/pricePoints",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the price schedule of an in-app purchase: its base territory, the prices you set by
    /// hand, and the prices the App Store derives from them.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Price Schedules resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-iappriceschedule"/>
    public async Task<ResourceResponse<InAppPurchasePriceSchedule>> GetPriceScheduleForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchasePriceSchedule>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/iapPriceSchedule",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads which App Store territories an in-app purchase is on sale in.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-inapppurchaseavailability"/>
    public async Task<ResourceResponse<InAppPurchaseAvailability>> GetAvailabilityForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAvailability>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/inAppPurchaseAvailability",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the screenshot of the purchase flow that App Review sees for an in-app purchase.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-appstorereviewscreenshot"/>
    public async Task<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>> GetAppStoreReviewScreenshotForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/appStoreReviewScreenshot",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the downloadable content Apple hosts for an in-app purchase.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Contents resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-content"/>
    public async Task<ResourceResponse<InAppPurchaseContent>> GetContentForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseContent>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/content",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the App Store product page promotion of an in-app purchase, if it has one.
    /// </summary>
    /// <param name="inAppPurchaseId">The opaque resource ID of the in-app purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Promoted Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchases-_id_-promotedpurchase"/>
    public async Task<ResourceResponse<PromotedPurchase>> GetPromotedPurchaseForInAppPurchaseAsync(
        string inAppPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PromotedPurchase>>(
                path: $"/v2/inAppPurchases/{inAppPurchaseId}/promotedPurchase",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a draft version of an in-app purchase. A version is a draft container for one review
    /// cycle. It holds the localizations and images that go through App Review together, while the
    /// in-app purchase keeps its product ID, type, and pricing. The new version starts out
    /// <c>PREPARE_FOR_SUBMISSION</c>, and its localizations and images are editable only in that
    /// state. Add them with <see cref="CreateInAppPurchaseLocalizationV2Async"/> and
    /// <see cref="CreateInAppPurchaseImageV2Async"/>, then submit the version by adding it to a
    /// review submission with <see cref="CreateReviewSubmissionItemAsync"/>. To change the metadata
    /// after review, create a new version.
    /// </summary>
    /// <remarks>
    /// Apple says each in-app purchase has a version, and in-app purchases whose localizations went
    /// through the deprecated v1 endpoint have been seen already carrying a version 1 in
    /// <c>PREPARE_FOR_SUBMISSION</c>. So list the existing versions with
    /// <see cref="ListVersionsForInAppPurchaseAsync"/>, filtering <c>state</c> on
    /// <c>PREPARE_FOR_SUBMISSION</c>, and reuse the draft you find before you create another. Apple
    /// describes a new version as capturing the in-app purchase's current localized metadata and
    /// images.
    /// </remarks>
    /// <param name="request">The version to create, with the in-app purchase it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseversions"/>
    public async Task<ResourceResponse<InAppPurchaseVersion>> CreateInAppPurchaseVersionAsync(
        InAppPurchaseVersionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseVersion>>(
                path: "/v1/inAppPurchaseVersions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase version, including where it sits in the App Review workflow. Poll
    /// it after you submit the version to follow it from <c>WAITING_FOR_REVIEW</c> through
    /// <c>IN_REVIEW</c> to <c>APPROVED</c> or <c>REJECTED</c>.
    /// </summary>
    /// <param name="inAppPurchaseVersionId">The opaque resource ID of the in-app purchase version.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Versions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseversions-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseVersion>> GetInAppPurchaseVersionAsync(
        string inAppPurchaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseVersion>>(
                path: $"/v1/inAppPurchaseVersions/{inAppPurchaseVersionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the display names and descriptions an in-app purchase version holds, one per language.
    /// </summary>
    /// <param name="inAppPurchaseVersionId">The opaque resource ID of the in-app purchase version.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of v2 In-App Purchase Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseversions-_id_-localizations"/>
    public async Task<ResourceListResponse<InAppPurchaseLocalizationV2>> ListLocalizationsForInAppPurchaseVersionAsync(
        string inAppPurchaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseLocalizationV2>>(
                path: $"/v1/inAppPurchaseVersions/{inAppPurchaseVersionId}/localizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the promotional images an in-app purchase version holds. The App Review screenshot
    /// isn't among them. It stays on the in-app purchase, and you upload it with
    /// <see cref="CreateInAppPurchaseAppStoreReviewScreenshotAsync"/>.
    /// </summary>
    /// <param name="inAppPurchaseVersionId">The opaque resource ID of the in-app purchase version.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of v2 In-App Purchase Images resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseversions-_id_-images"/>
    public async Task<ResourceListResponse<InAppPurchaseImageV2>> ListImagesForInAppPurchaseVersionAsync(
        string inAppPurchaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchaseImageV2>>(
                path: $"/v1/inAppPurchaseVersions/{inAppPurchaseVersionId}/images",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the promotional image of an in-app purchase version. Apple exposes both this single
    /// image and the list that <see cref="ListImagesForInAppPurchaseVersionAsync"/> returns, without
    /// documenting how the two relate. The App Review screenshot is separate. It stays on the in-app
    /// purchase, and you upload it with <see cref="CreateInAppPurchaseAppStoreReviewScreenshotAsync"/>.
    /// </summary>
    /// <param name="inAppPurchaseVersionId">The opaque resource ID of the in-app purchase version.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single v2 In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseversions-_id_-image"/>
    public async Task<ResourceResponse<InAppPurchaseImageV2>> GetImageForInAppPurchaseVersionAsync(
        string inAppPurchaseVersionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImageV2>>(
                path: $"/v1/inAppPurchaseVersions/{inAppPurchaseVersionId}/image",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the display name and description of an in-app purchase in one language to an in-app
    /// purchase version, which you create with <see cref="CreateInAppPurchaseVersionAsync"/>. Add
    /// localizations while the version is <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    /// <param name="request">The localization to create, with the version it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new v2 In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v2-inapppurchaselocalizations"/>
    public async Task<ResourceResponse<InAppPurchaseLocalizationV2>> CreateInAppPurchaseLocalizationV2Async(
        InAppPurchaseLocalizationV2CreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalizationV2>>(
                path: "/v2/inAppPurchaseLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one v2 in-app purchase localization.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single v2 In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchaselocalizations-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseLocalizationV2>> GetInAppPurchaseLocalizationV2Async(
        string inAppPurchaseLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalizationV2>>(
                path: $"/v2/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the display name or the description of an in-app purchase in one language, on a
    /// version that is still <c>PREPARE_FOR_SUBMISSION</c>. The locale is fixed once you create the
    /// localization.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated v2 In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v2-inapppurchaselocalizations-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseLocalizationV2>> UpdateInAppPurchaseLocalizationV2Async(
        string inAppPurchaseLocalizationId,
        InAppPurchaseLocalizationV2UpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalizationV2>>(
                path: $"/v2/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a localization from an in-app purchase version that is still
    /// <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v2-inapppurchaselocalizations-_id_"/>
    public async Task DeleteInAppPurchaseLocalizationV2Async(
        string inAppPurchaseLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v2/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the display name and description of an in-app purchase in one language. Every in-app
    /// purchase needs a localization for the app's primary language before you can submit it.
    /// Apple replaced this endpoint with <see cref="CreateInAppPurchaseLocalizationV2Async"/>, which
    /// adds the localization to a version.
    /// </summary>
    /// <param name="request">The localization to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaselocalizations"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use CreateInAppPurchaseLocalizationV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseLocalization>> CreateInAppPurchaseLocalizationAsync(
        InAppPurchaseLocalizationCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalization>>(
                path: "/v1/inAppPurchaseLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase localization. Apple replaced this endpoint with
    /// <see cref="GetInAppPurchaseLocalizationV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaselocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use GetInAppPurchaseLocalizationV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseLocalization>> GetInAppPurchaseLocalizationAsync(
        string inAppPurchaseLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalization>>(
                path: $"/v1/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the display name or the description of an in-app purchase in one language. The
    /// locale is fixed once you create the localization. Apple replaced this endpoint with
    /// <see cref="UpdateInAppPurchaseLocalizationV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaselocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use UpdateInAppPurchaseLocalizationV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseLocalization>> UpdateInAppPurchaseLocalizationAsync(
        string inAppPurchaseLocalizationId,
        InAppPurchaseLocalizationUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseLocalization>>(
                path: $"/v1/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes an in-app purchase localization, so the App Store falls back to another language.
    /// Apple replaced this endpoint with <see cref="DeleteInAppPurchaseLocalizationV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseLocalizationId">The opaque resource ID of the in-app purchase localization.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-inapppurchaselocalizations-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use DeleteInAppPurchaseLocalizationV2Async instead.")]
    public async Task DeleteInAppPurchaseLocalizationAsync(
        string inAppPurchaseLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/inAppPurchaseLocalizations/{inAppPurchaseLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the prices of an in-app purchase over time. The request replaces the whole schedule, so
    /// send every manual price you want to keep, and the App Store derives the price of every
    /// territory you leave out from the base territory.
    /// </summary>
    /// <param name="request">The price schedule to set, with the prices in its included array.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Price Schedules resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchasepriceschedules"/>
    public async Task<ResourceResponse<InAppPurchasePriceSchedule>> CreateInAppPurchasePriceScheduleAsync(
        InAppPurchasePriceScheduleCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchasePriceSchedule>>(
                path: "/v1/inAppPurchasePriceSchedules",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase price schedule.
    /// </summary>
    /// <param name="inAppPurchasePriceScheduleId">The opaque resource ID of the price schedule, which is the same as the ID of the in-app purchase it belongs to.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Price Schedules resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasepriceschedules-_id_"/>
    public async Task<ResourceResponse<InAppPurchasePriceSchedule>> GetInAppPurchasePriceScheduleAsync(
        string inAppPurchasePriceScheduleId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchasePriceSchedule>>(
                path: $"/v1/inAppPurchasePriceSchedules/{inAppPurchasePriceScheduleId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prices of a schedule that you set yourself, rather than the ones the App Store
    /// derived from the base territory.
    /// </summary>
    /// <param name="inAppPurchasePriceScheduleId">The opaque resource ID of the price schedule, which is the same as the ID of the in-app purchase it belongs to.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasepriceschedules-_id_-manualprices"/>
    public async Task<ResourceListResponse<InAppPurchasePrice>> ListManualPricesForInAppPurchasePriceScheduleAsync(
        string inAppPurchasePriceScheduleId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchasePrice>>(
                path: $"/v1/inAppPurchasePriceSchedules/{inAppPurchasePriceScheduleId}/manualPrices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prices of a schedule that the App Store derived from the base territory, one per
    /// territory you never set a price for yourself.
    /// </summary>
    /// <param name="inAppPurchasePriceScheduleId">The opaque resource ID of the price schedule, which is the same as the ID of the in-app purchase it belongs to.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of In-App Purchase Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasepriceschedules-_id_-automaticprices"/>
    public async Task<ResourceListResponse<InAppPurchasePrice>> ListAutomaticPricesForInAppPurchasePriceScheduleAsync(
        string inAppPurchasePriceScheduleId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchasePrice>>(
                path: $"/v1/inAppPurchasePriceSchedules/{inAppPurchasePriceScheduleId}/automaticPrices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the territory whose price a schedule converts into the price of every other territory.
    /// </summary>
    /// <param name="inAppPurchasePriceScheduleId">The opaque resource ID of the price schedule, which is the same as the ID of the in-app purchase it belongs to.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Territories resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasepriceschedules-_id_-baseterritory"/>
    public async Task<ResourceResponse<Territory>> GetBaseTerritoryForInAppPurchasePriceScheduleAsync(
        string inAppPurchasePriceScheduleId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Territory>>(
                path: $"/v1/inAppPurchasePriceSchedules/{inAppPurchasePriceScheduleId}/baseTerritory",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the price points in every other territory that match one price point. Use it to find the
    /// price in each territory that corresponds to a price you chose in the base territory.
    /// </summary>
    /// <param name="inAppPurchasePricePointId">The opaque resource ID of the in-app purchase price point.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of equivalent In-App Purchase Price Points resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasepricepoints-_id_-equalizations"/>
    public async Task<ResourceListResponse<InAppPurchasePricePoint>> ListEqualizationsForInAppPurchasePricePointAsync(
        string inAppPurchasePricePointId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<InAppPurchasePricePoint>>(
                path: $"/v1/inAppPurchasePricePoints/{inAppPurchasePricePointId}/equalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets which App Store territories an in-app purchase is on sale in. The request replaces the
    /// whole set of territories rather than adding to it.
    /// </summary>
    /// <param name="request">The availability to set.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseavailabilities"/>
    public async Task<ResourceResponse<InAppPurchaseAvailability>> CreateInAppPurchaseAvailabilityAsync(
        InAppPurchaseAvailabilityCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAvailability>>(
                path: "/v1/inAppPurchaseAvailabilities",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase availability.
    /// </summary>
    /// <param name="inAppPurchaseAvailabilityId">The opaque resource ID of the in-app purchase availability.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseavailabilities-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseAvailability>> GetInAppPurchaseAvailabilityAsync(
        string inAppPurchaseAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAvailability>>(
                path: $"/v1/inAppPurchaseAvailabilities/{inAppPurchaseAvailabilityId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the App Store territories an in-app purchase is on sale in.
    /// </summary>
    /// <param name="inAppPurchaseAvailabilityId">The opaque resource ID of the in-app purchase availability.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Territories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseavailabilities-_id_-availableterritories"/>
    public async Task<ResourceListResponse<Territory>> ListAvailableTerritoriesForInAppPurchaseAvailabilityAsync(
        string inAppPurchaseAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Territory>>(
                path: $"/v1/inAppPurchaseAvailabilities/{inAppPurchaseAvailabilityId}/availableTerritories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reserves a promotional image on an in-app purchase version, the image customers see on the
    /// App Store product page. The response carries the upload operations that say how to split the
    /// file and where to send each part; send them with <see cref="UploadAssetAsync"/>, then commit
    /// the image with <see cref="UpdateInAppPurchaseImageV2Async"/>. The App Review screenshot is
    /// separate. It stays on the in-app purchase, and you upload it with
    /// <see cref="CreateInAppPurchaseAppStoreReviewScreenshotAsync"/>.
    /// </summary>
    /// <remarks>
    /// Apple's 4.4.1 release notes say these endpoints manage review screenshots, but its version
    /// and migration guides describe a promotional image and keep the App Review screenshot on the
    /// in-app purchase. This library follows the guides; see <see cref="InAppPurchaseImageV2"/>.
    /// </remarks>
    /// <param name="request">The image to reserve, with its file name, its size, and the version it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new v2 In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v2-inapppurchaseimages"/>
    public async Task<ResourceResponse<InAppPurchaseImageV2>> CreateInAppPurchaseImageV2Async(
        InAppPurchaseImageV2CreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImageV2>>(
                path: "/v2/inAppPurchaseImages",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one promotional image of an in-app purchase version, including how far its upload got.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single v2 In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v2-inapppurchaseimages-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseImageV2>> GetInAppPurchaseImageV2Async(
        string inAppPurchaseImageId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImageV2>>(
                path: $"/v2/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Commits a promotional image of an in-app purchase version once you send every part of the
    /// file with <see cref="UploadAssetAsync"/>. Unlike the v1 image, the commit sets only
    /// <c>uploaded</c> and carries no checksum.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="request">The upload result to record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated v2 In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v2-inapppurchaseimages-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseImageV2>> UpdateInAppPurchaseImageV2Async(
        string inAppPurchaseImageId,
        InAppPurchaseImageV2UpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImageV2>>(
                path: $"/v2/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a promotional image from an in-app purchase version, whether or not you finished
    /// uploading it.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v2-inapppurchaseimages-_id_"/>
    public async Task DeleteInAppPurchaseImageV2Async(
        string inAppPurchaseImageId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v2/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reserves a promotional image for an in-app purchase. The response carries the upload
    /// operations that say how to split the file and where to send each part; upload them, then
    /// commit the image with <see cref="UpdateInAppPurchaseImageAsync"/>. Apple replaced this
    /// endpoint with <see cref="CreateInAppPurchaseImageV2Async"/>, which reserves the image on a
    /// version.
    /// </summary>
    /// <param name="request">The image to reserve, with its file name and size.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseimages"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use CreateInAppPurchaseImageV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseImage>> CreateInAppPurchaseImageAsync(
        InAppPurchaseImageCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImage>>(
                path: "/v1/inAppPurchaseImages",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one in-app purchase image, including how far its upload and review got. Apple replaced
    /// this endpoint with <see cref="GetInAppPurchaseImageV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseimages-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use GetInAppPurchaseImageV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseImage>> GetInAppPurchaseImageAsync(
        string inAppPurchaseImageId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImage>>(
                path: $"/v1/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Commits an in-app purchase image once every part of the file is uploaded, and hands App
    /// Store Connect the checksum to verify what it received. Apple replaced this endpoint with
    /// <see cref="UpdateInAppPurchaseImageV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="request">The upload result to record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaseimages-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use UpdateInAppPurchaseImageV2Async instead.")]
    public async Task<ResourceResponse<InAppPurchaseImage>> UpdateInAppPurchaseImageAsync(
        string inAppPurchaseImageId,
        InAppPurchaseImageUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseImage>>(
                path: $"/v1/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an in-app purchase image, whether or not you finished uploading it. Apple replaced
    /// this endpoint with <see cref="DeleteInAppPurchaseImageV2Async"/>.
    /// </summary>
    /// <param name="inAppPurchaseImageId">The opaque resource ID of the in-app purchase image.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-inapppurchaseimages-_id_"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Use DeleteInAppPurchaseImageV2Async instead.")]
    public async Task DeleteInAppPurchaseImageAsync(
        string inAppPurchaseImageId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/inAppPurchaseImages/{inAppPurchaseImageId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reserves the App Review screenshot of an in-app purchase. The response carries the upload
    /// operations that say how to split the file and where to send each part; upload them, then
    /// commit the screenshot with <see cref="UpdateInAppPurchaseAppStoreReviewScreenshotAsync"/>.
    /// </summary>
    /// <param name="request">The screenshot to reserve, with its file name and size.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchaseappstorereviewscreenshots"/>
    public async Task<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>> CreateInAppPurchaseAppStoreReviewScreenshotAsync(
        InAppPurchaseAppStoreReviewScreenshotCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>>(
                path: "/v1/inAppPurchaseAppStoreReviewScreenshots",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one App Store review screenshot, including how far its upload got.
    /// </summary>
    /// <param name="inAppPurchaseAppStoreReviewScreenshotId">The opaque resource ID of the App Store review screenshot.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchaseappstorereviewscreenshots-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>> GetInAppPurchaseAppStoreReviewScreenshotAsync(
        string inAppPurchaseAppStoreReviewScreenshotId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>>(
                path: $"/v1/inAppPurchaseAppStoreReviewScreenshots/{inAppPurchaseAppStoreReviewScreenshotId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Commits an App Store review screenshot once every part of the file is uploaded, and hands
    /// App Store Connect the checksum to verify what it received.
    /// </summary>
    /// <param name="inAppPurchaseAppStoreReviewScreenshotId">The opaque resource ID of the App Store review screenshot.</param>
    /// <param name="request">The upload result to record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated In-App Purchase App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-inapppurchaseappstorereviewscreenshots-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>> UpdateInAppPurchaseAppStoreReviewScreenshotAsync(
        string inAppPurchaseAppStoreReviewScreenshotId,
        InAppPurchaseAppStoreReviewScreenshotUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseAppStoreReviewScreenshot>>(
                path: $"/v1/inAppPurchaseAppStoreReviewScreenshots/{inAppPurchaseAppStoreReviewScreenshotId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an App Store review screenshot, whether or not you finished uploading it.
    /// </summary>
    /// <param name="inAppPurchaseAppStoreReviewScreenshotId">The opaque resource ID of the App Store review screenshot.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-inapppurchaseappstorereviewscreenshots-_id_"/>
    public async Task DeleteInAppPurchaseAppStoreReviewScreenshotAsync(
        string inAppPurchaseAppStoreReviewScreenshotId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/inAppPurchaseAppStoreReviewScreenshots/{inAppPurchaseAppStoreReviewScreenshotId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits an in-app purchase to App Review on its own, without waiting for an app version.
    /// The in-app purchase has to be complete first: a localization, a price schedule, its
    /// availability, and a review screenshot if it needs one. Apple replaced this endpoint with the
    /// review submission workflow: <see cref="CreateReviewSubmissionAsync"/>,
    /// <see cref="CreateReviewSubmissionItemAsync"/> with an in-app purchase version, and
    /// <see cref="UpdateReviewSubmissionAsync"/>.
    /// </summary>
    /// <param name="request">The in-app purchase to submit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new In-App Purchase Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-inapppurchasesubmissions"/>
    [Obsolete("Apple deprecated this endpoint in App Store Connect API 4.4.1. Submit through a review submission instead, with CreateReviewSubmissionAsync, CreateReviewSubmissionItemAsync and UpdateReviewSubmissionAsync.")]
    public async Task<ResourceResponse<InAppPurchaseSubmission>> CreateInAppPurchaseSubmissionAsync(
        InAppPurchaseSubmissionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseSubmission>>(
                path: "/v1/inAppPurchaseSubmissions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one hosted content record for an in-app purchase.
    /// </summary>
    /// <param name="inAppPurchaseContentId">The opaque resource ID of the in-app purchase content.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single In-App Purchase Contents resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-inapppurchasecontents-_id_"/>
    public async Task<ResourceResponse<InAppPurchaseContent>> GetInAppPurchaseContentAsync(
        string inAppPurchaseContentId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<InAppPurchaseContent>>(
                path: $"/v1/inAppPurchaseContents/{inAppPurchaseContentId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Promotes an in-app purchase or a subscription on an app's App Store product page. Point the
    /// request at either an in-app purchase or a subscription, not both.
    /// </summary>
    /// <param name="request">The promotion to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Promoted Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-promotedpurchases"/>
    public async Task<ResourceResponse<PromotedPurchase>> CreatePromotedPurchaseAsync(
        PromotedPurchaseCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PromotedPurchase>>(
                path: "/v1/promotedPurchases",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one promoted purchase, including where it sits in the review workflow.
    /// </summary>
    /// <param name="promotedPurchaseId">The opaque resource ID of the promoted purchase.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a single Promoted Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-promotedpurchases-_id_"/>
    public async Task<ResourceResponse<PromotedPurchase>> GetPromotedPurchaseAsync(
        string promotedPurchaseId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PromotedPurchase>>(
                path: $"/v1/promotedPurchases/{promotedPurchaseId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Turns a promotion on or off, or changes whether every customer sees it.
    /// </summary>
    /// <param name="promotedPurchaseId">The opaque resource ID of the promoted purchase.</param>
    /// <param name="request">The changes to apply.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Promoted Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-promotedpurchases-_id_"/>
    public async Task<ResourceResponse<PromotedPurchase>> UpdatePromotedPurchaseAsync(
        string promotedPurchaseId,
        PromotedPurchaseUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PromotedPurchase>>(
                path: $"/v1/promotedPurchases/{promotedPurchaseId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a promotion, so the in-app purchase stops appearing on the app's product page.
    /// </summary>
    /// <param name="promotedPurchaseId">The opaque resource ID of the promoted purchase.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-promotedpurchases-_id_"/>
    public async Task DeletePromotedPurchaseAsync(
        string promotedPurchaseId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/promotedPurchases/{promotedPurchaseId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
