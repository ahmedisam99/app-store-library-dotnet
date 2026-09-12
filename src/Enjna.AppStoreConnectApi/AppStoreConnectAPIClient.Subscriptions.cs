using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Creates an auto-renewable subscription in one of your app's subscription groups.
    /// </summary>
    /// <param name="request">The subscription to create, including the subscription group it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscriptions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptions"/>
    public async Task<ResourceResponse<Subscription>> CreateSubscriptionAsync(
        SubscriptionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Subscription>>(
                path: "/v1/subscriptions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the information of a single auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and per-relationship paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscriptions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_"/>
    public async Task<ResourceResponse<Subscription>> GetSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Subscription>>(
                path: $"/v1/subscriptions/{subscriptionId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the metadata of an auto-renewable subscription, and optionally replaces its prices
    /// and its introductory or promotional offers.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription to update.</param>
    /// <param name="request">The attributes and relationships to change.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscriptions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptions-_id_"/>
    public async Task<ResourceResponse<Subscription>> UpdateSubscriptionAsync(
        string subscriptionId,
        SubscriptionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<Subscription>>(
                path: $"/v1/subscriptions/{subscriptionId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an auto-renewable subscription that you haven't yet made available for purchase.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the subscription.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptions-_id_"/>
    public async Task DeleteSubscriptionAsync(
        string subscriptionId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptions/{subscriptionId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the localized metadata of an auto-renewable subscription, one entry per locale.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Localizations resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-subscriptionlocalizations"/>
    public async Task<ResourceListResponse<SubscriptionLocalization>> ListLocalizationsForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionLocalization>>(
                path: $"/v1/subscriptions/{subscriptionId}/subscriptionLocalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the prices of an auto-renewable subscription, across territories and billing plans.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as the <c>planType</c>, <c>subscriptionPricePoint</c>, and <c>territory</c> filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-prices"/>
    public async Task<ResourceListResponse<SubscriptionPrice>> ListPricesForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPrice>>(
                path: $"/v1/subscriptions/{subscriptionId}/prices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the price points available to an auto-renewable subscription. You set a subscription price
    /// by pointing at one of these price points rather than by naming an amount.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as the <c>territory</c>, <c>upfrontPricePointId</c>, and <c>planType</c> filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Price Points resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-pricepoints"/>
    public async Task<ResourceListResponse<SubscriptionPricePoint>> ListPricePointsForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPricePoint>>(
                path: $"/v1/subscriptions/{subscriptionId}/pricePoints",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the introductory offers of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as the <c>territory</c> filter, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Introductory Offers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-introductoryoffers"/>
    public async Task<ResourceListResponse<SubscriptionIntroductoryOffer>> ListIntroductoryOffersForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionIntroductoryOffer>>(
                path: $"/v1/subscriptions/{subscriptionId}/introductoryOffers",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the promotional offers of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as the <c>territory</c> filter, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Promotional Offers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-promotionaloffers"/>
    public async Task<ResourceListResponse<SubscriptionPromotionalOffer>> ListPromotionalOffersForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPromotionalOffer>>(
                path: $"/v1/subscriptions/{subscriptionId}/promotionalOffers",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the offer codes of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as the <c>territory</c> filter, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Offer Codes resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-offercodes"/>
    public async Task<ResourceListResponse<SubscriptionOfferCode>> ListOfferCodesForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionOfferCode>>(
                path: $"/v1/subscriptions/{subscriptionId}/offerCodes",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the win-back offers of an auto-renewable subscription, which win back customers who
    /// previously let the subscription lapse.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Win-Back Offers resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-winbackoffers"/>
    public async Task<ResourceListResponse<WinBackOffer>> ListWinBackOffersForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<WinBackOffer>>(
                path: $"/v1/subscriptions/{subscriptionId}/winBackOffers",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the territory availability of an auto-renewable subscription.
    /// </summary>
    /// <remarks>
    /// Apple deprecated this endpoint. Use
    /// <see cref="ListPlanAvailabilitiesForSubscriptionAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    /// instead, which reports availability separately for each of the subscription's billing plans.
    /// </remarks>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and the limit on the available territories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-subscriptionavailability"/>
    /// <seealso cref="ListPlanAvailabilitiesForSubscriptionAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    public async Task<ResourceResponse<SubscriptionAvailability>> GetAvailabilityForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAvailability>>(
                path: $"/v1/subscriptions/{subscriptionId}/subscriptionAvailability",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the territory availability of each billing plan of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Plan Availabilities resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-planavailabilities"/>
    public async Task<ResourceListResponse<SubscriptionPlanAvailability>> ListPlanAvailabilitiesForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPlanAvailability>>(
                path: $"/v1/subscriptions/{subscriptionId}/planAvailabilities",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the App Review screenshot of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-appstorereviewscreenshot"/>
    public async Task<ResourceResponse<SubscriptionAppStoreReviewScreenshot>> GetAppStoreReviewScreenshotForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAppStoreReviewScreenshot>>(
                path: $"/v1/subscriptions/{subscriptionId}/appStoreReviewScreenshot",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the promotional images of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Images resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-images"/>
    public async Task<ResourceListResponse<SubscriptionImage>> ListImagesForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionImage>>(
                path: $"/v1/subscriptions/{subscriptionId}/images",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the promotion of an auto-renewable subscription, which is how the subscription appears
    /// as a promoted in-app purchase on your App Store product page.
    /// </summary>
    /// <param name="subscriptionId">The opaque resource ID of the subscription.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Promoted Purchases resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptions-_id_-promotedpurchase"/>
    public async Task<ResourceResponse<PromotedPurchase>> GetPromotedPurchaseForSubscriptionAsync(
        string subscriptionId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<PromotedPurchase>>(
                path: $"/v1/subscriptions/{subscriptionId}/promotedPurchase",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds localized metadata for one locale to an auto-renewable subscription.
    /// </summary>
    /// <param name="request">The localization to create, including the subscription it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionlocalizations"/>
    public async Task<ResourceResponse<SubscriptionLocalization>> CreateSubscriptionLocalizationAsync(
        SubscriptionLocalizationCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionLocalization>>(
                path: "/v1/subscriptionLocalizations",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the localized metadata of an auto-renewable subscription for one locale.
    /// </summary>
    /// <param name="subscriptionLocalizationId">The opaque resource ID of the localization.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionlocalizations-_id_"/>
    public async Task<ResourceResponse<SubscriptionLocalization>> GetSubscriptionLocalizationAsync(
        string subscriptionLocalizationId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionLocalization>>(
                path: $"/v1/subscriptionLocalizations/{subscriptionLocalizationId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the localized name or description of an auto-renewable subscription for one locale.
    /// </summary>
    /// <param name="subscriptionLocalizationId">The opaque resource ID of the localization to update.</param>
    /// <param name="request">The attributes to change.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Localizations resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionlocalizations-_id_"/>
    public async Task<ResourceResponse<SubscriptionLocalization>> UpdateSubscriptionLocalizationAsync(
        string subscriptionLocalizationId,
        SubscriptionLocalizationUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionLocalization>>(
                path: $"/v1/subscriptionLocalizations/{subscriptionLocalizationId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the localized metadata of an auto-renewable subscription for one locale.
    /// </summary>
    /// <param name="subscriptionLocalizationId">The opaque resource ID of the localization to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the localization.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionlocalizations-_id_"/>
    public async Task DeleteSubscriptionLocalizationAsync(
        string subscriptionLocalizationId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionLocalizations/{subscriptionLocalizationId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Submits an auto-renewable subscription and its metadata to App Review.
    /// </summary>
    /// <param name="request">The submission to create, which names the subscription to submit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Subscription Submissions resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionsubmissions"/>
    public async Task<ResourceResponse<SubscriptionSubmission>> CreateSubscriptionSubmissionAsync(
        SubscriptionSubmissionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionSubmission>>(
                path: "/v1/subscriptionSubmissions",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reserves an App Review screenshot for an auto-renewable subscription. Upload the file's
    /// bytes with the upload operations of the response, then commit it with
    /// <see cref="UpdateSubscriptionAppStoreReviewScreenshotAsync"/>.
    /// </summary>
    /// <param name="request">The screenshot to reserve, including its file name and size.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the reserved Subscription App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionappstorereviewscreenshots"/>
    public async Task<ResourceResponse<SubscriptionAppStoreReviewScreenshot>> CreateSubscriptionAppStoreReviewScreenshotAsync(
        SubscriptionAppStoreReviewScreenshotCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAppStoreReviewScreenshot>>(
                path: "/v1/subscriptionAppStoreReviewScreenshots",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the information of a single subscription App Review screenshot, including the state of
    /// its asset delivery.
    /// </summary>
    /// <param name="subscriptionAppStoreReviewScreenshotId">The opaque resource ID of the screenshot.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionappstorereviewscreenshots-_id_"/>
    public async Task<ResourceResponse<SubscriptionAppStoreReviewScreenshot>> GetSubscriptionAppStoreReviewScreenshotAsync(
        string subscriptionAppStoreReviewScreenshotId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAppStoreReviewScreenshot>>(
                path: $"/v1/subscriptionAppStoreReviewScreenshots/{subscriptionAppStoreReviewScreenshotId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Commits a subscription App Review screenshot once every upload operation finished, by
    /// sending the checksum of the file you uploaded.
    /// </summary>
    /// <param name="subscriptionAppStoreReviewScreenshotId">The opaque resource ID of the reserved screenshot.</param>
    /// <param name="request">The checksum of the uploaded file, and the flag that marks the upload complete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription App Store Review Screenshots resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionappstorereviewscreenshots-_id_"/>
    public async Task<ResourceResponse<SubscriptionAppStoreReviewScreenshot>> UpdateSubscriptionAppStoreReviewScreenshotAsync(
        string subscriptionAppStoreReviewScreenshotId,
        SubscriptionAppStoreReviewScreenshotUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAppStoreReviewScreenshot>>(
                path: $"/v1/subscriptionAppStoreReviewScreenshots/{subscriptionAppStoreReviewScreenshotId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a subscription App Review screenshot.
    /// </summary>
    /// <param name="subscriptionAppStoreReviewScreenshotId">The opaque resource ID of the screenshot to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the screenshot.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionappstorereviewscreenshots-_id_"/>
    public async Task DeleteSubscriptionAppStoreReviewScreenshotAsync(
        string subscriptionAppStoreReviewScreenshotId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionAppStoreReviewScreenshots/{subscriptionAppStoreReviewScreenshotId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reserves a promotional image for an auto-renewable subscription. Upload the file's bytes
    /// with the upload operations of the response, then commit it with
    /// <see cref="UpdateSubscriptionImageAsync"/>.
    /// </summary>
    /// <param name="request">The image to reserve, including its file name and size.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the reserved Subscription Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionimages"/>
    public async Task<ResourceResponse<SubscriptionImage>> CreateSubscriptionImageAsync(
        SubscriptionImageCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionImage>>(
                path: "/v1/subscriptionImages",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the information of a single subscription promotional image, including its upload and
    /// review state.
    /// </summary>
    /// <param name="subscriptionImageId">The opaque resource ID of the image.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionimages-_id_"/>
    public async Task<ResourceResponse<SubscriptionImage>> GetSubscriptionImageAsync(
        string subscriptionImageId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionImage>>(
                path: $"/v1/subscriptionImages/{subscriptionImageId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Commits a subscription promotional image once every upload operation finished, by sending
    /// the checksum of the file you uploaded.
    /// </summary>
    /// <param name="subscriptionImageId">The opaque resource ID of the reserved image.</param>
    /// <param name="request">The checksum of the uploaded file, and the flag that marks the upload complete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Images resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionimages-_id_"/>
    public async Task<ResourceResponse<SubscriptionImage>> UpdateSubscriptionImageAsync(
        string subscriptionImageId,
        SubscriptionImageUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionImage>>(
                path: $"/v1/subscriptionImages/{subscriptionImageId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a promotional image of an auto-renewable subscription.
    /// </summary>
    /// <param name="subscriptionImageId">The opaque resource ID of the image to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once App Store Connect deleted the image.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionimages-_id_"/>
    public async Task DeleteSubscriptionImageAsync(
        string subscriptionImageId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionImages/{subscriptionImageId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
