using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Schedules what a subscription costs in one territory, from a day you choose onward.
    /// </summary>
    /// <param name="request">The price to schedule, and the subscription and price point it belongs to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Prices resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionprices"/>
    public async Task<ResourceResponse<SubscriptionPrice>> CreateSubscriptionPriceAsync(
        SubscriptionPriceCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPrice>>(
                path: "/v1/subscriptionPrices",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a scheduled subscription price. A price that already took effect can't be deleted.
    /// </summary>
    /// <param name="subscriptionPriceId">The opaque resource ID of the price to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepted the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionprices-_id_"/>
    public async Task DeleteSubscriptionPriceAsync(
        string subscriptionPriceId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionPrices/{subscriptionPriceId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one subscription price point: the customer price in its territory, and what you earn
    /// from it.
    /// </summary>
    /// <param name="subscriptionPricePointId">The opaque resource ID of the price point.</param>
    /// <param name="query">Optional query parameters such as fields and includes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Price Points resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionpricepoints-_id_"/>
    public async Task<ResourceResponse<SubscriptionPricePoint>> GetSubscriptionPricePointAsync(
        string subscriptionPricePointId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPricePoint>>(
                path: $"/v1/subscriptionPricePoints/{subscriptionPricePointId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the price points of other territories that are equivalent to a price point. Use them to
    /// price a subscription consistently around the world.
    /// </summary>
    /// <param name="subscriptionPricePointId">The opaque resource ID of the price point to equalize.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of equivalent Subscription Price Points resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionpricepoints-_id_-equalizations"/>
    public async Task<ResourceListResponse<SubscriptionPricePoint>> ListEqualizationsForSubscriptionPricePointAsync(
        string subscriptionPricePointId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPricePoint>>(
                path: $"/v1/subscriptionPricePoints/{subscriptionPricePointId}/equalizations",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the territories a subscription is for sale in.
    /// </summary>
    /// <remarks>
    /// Apple deprecated this endpoint. Use
    /// <see cref="CreateSubscriptionPlanAvailabilityAsync(SubscriptionPlanAvailabilityCreateRequest, CancellationToken)"/>
    /// instead. It sets availability per billing plan, and the plan availability it creates can be
    /// changed later, which this resource has no endpoint for.
    /// </remarks>
    /// <param name="request">The subscription, the territories it sells in, and whether it follows new territories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Availabilities resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionavailabilities"/>
    /// <seealso cref="CreateSubscriptionPlanAvailabilityAsync(SubscriptionPlanAvailabilityCreateRequest, CancellationToken)"/>
    public async Task<ResourceResponse<SubscriptionAvailability>> CreateSubscriptionAvailabilityAsync(
        SubscriptionAvailabilityCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAvailability>>(
                path: "/v1/subscriptionAvailabilities",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the territory availability of a subscription.
    /// </summary>
    /// <remarks>
    /// Apple deprecated this endpoint. Use
    /// <see cref="GetSubscriptionPlanAvailabilityAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    /// instead, which reads availability per billing plan.
    /// </remarks>
    /// <param name="subscriptionAvailabilityId">The opaque resource ID of the subscription availability.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionavailabilities-_id_"/>
    /// <seealso cref="GetSubscriptionPlanAvailabilityAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    public async Task<ResourceResponse<SubscriptionAvailability>> GetSubscriptionAvailabilityAsync(
        string subscriptionAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionAvailability>>(
                path: $"/v1/subscriptionAvailabilities/{subscriptionAvailabilityId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the territories a subscription is for sale in.
    /// </summary>
    /// <remarks>
    /// Apple deprecated this endpoint. Use
    /// <see cref="ListAvailableTerritoriesForSubscriptionPlanAvailabilityAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    /// instead, which lists the territories of one billing plan.
    /// </remarks>
    /// <param name="subscriptionAvailabilityId">The opaque resource ID of the subscription availability.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Territories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionavailabilities-_id_-availableterritories"/>
    /// <seealso cref="ListAvailableTerritoriesForSubscriptionPlanAvailabilityAsync(string, AppStoreConnectQuery, CancellationToken)"/>
    public async Task<ResourceListResponse<Territory>> ListAvailableTerritoriesForSubscriptionAvailabilityAsync(
        string subscriptionAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Territory>>(
                path: $"/v1/subscriptionAvailabilities/{subscriptionAvailabilityId}/availableTerritories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the territories one billing plan of a subscription is for sale in.
    /// </summary>
    /// <param name="request">The subscription, the plan, the territories it sells in, and whether it follows new territories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Plan Availabilities resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionplanavailabilities"/>
    public async Task<ResourceResponse<SubscriptionPlanAvailability>> CreateSubscriptionPlanAvailabilityAsync(
        SubscriptionPlanAvailabilityCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPlanAvailability>>(
                path: "/v1/subscriptionPlanAvailabilities",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads the territory availability of one billing plan of a subscription.
    /// </summary>
    /// <param name="subscriptionPlanAvailabilityId">The opaque resource ID of the plan availability.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and the limit on the available territories.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Plan Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionplanavailabilities-_id_"/>
    public async Task<ResourceResponse<SubscriptionPlanAvailability>> GetSubscriptionPlanAvailabilityAsync(
        string subscriptionPlanAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPlanAvailability>>(
                path: $"/v1/subscriptionPlanAvailabilities/{subscriptionPlanAvailabilityId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the territories a billing plan is for sale in, or whether it follows new territories.
    /// </summary>
    /// <param name="subscriptionPlanAvailabilityId">The opaque resource ID of the plan availability.</param>
    /// <param name="request">The attributes and relationships to change.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Plan Availabilities resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionplanavailabilities-_id_"/>
    public async Task<ResourceResponse<SubscriptionPlanAvailability>> UpdateSubscriptionPlanAvailabilityAsync(
        string subscriptionPlanAvailabilityId,
        SubscriptionPlanAvailabilityUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPlanAvailability>>(
                path: $"/v1/subscriptionPlanAvailabilities/{subscriptionPlanAvailabilityId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the territories one billing plan of a subscription is for sale in.
    /// </summary>
    /// <param name="subscriptionPlanAvailabilityId">The opaque resource ID of the plan availability.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Territories resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionplanavailabilities-_id_-availableterritories"/>
    public async Task<ResourceListResponse<Territory>> ListAvailableTerritoriesForSubscriptionPlanAvailabilityAsync(
        string subscriptionPlanAvailabilityId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<Territory>>(
                path: $"/v1/subscriptionPlanAvailabilities/{subscriptionPlanAvailabilityId}/availableTerritories",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Replaces the territories a billing plan is for sale in with the ones you pass. Territories
    /// you leave out stop selling the plan.
    /// </summary>
    /// <param name="subscriptionPlanAvailabilityId">The opaque resource ID of the plan availability.</param>
    /// <param name="territoryIds">The opaque resource IDs of the territories the plan sells in.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepts the change.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the territory IDs are <c>null</c>.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionplanavailabilities-_id_-relationships-availableterritories"/>
    public async Task ReplaceAvailableTerritoriesForSubscriptionPlanAvailabilityAsync(
        string subscriptionPlanAvailabilityId,
        string[] territoryIds,
        CancellationToken cancellationToken = default)
    {
        if (territoryIds is null)
        {
            throw new ArgumentNullException(nameof(territoryIds));
        }

        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionPlanAvailabilities/{subscriptionPlanAvailabilityId}/relationships/availableTerritories",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: RelationshipDeclarationList.To("territories", territoryIds),
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads an app's billing grace period settings.
    /// </summary>
    /// <param name="subscriptionGracePeriodId">The opaque resource ID of the billing grace period.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Grace Periods resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptiongraceperiods-_id_"/>
    public async Task<ResourceResponse<SubscriptionGracePeriod>> GetSubscriptionGracePeriodAsync(
        string subscriptionGracePeriodId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGracePeriod>>(
                path: $"/v1/subscriptionGracePeriods/{subscriptionGracePeriodId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Turns an app's billing grace period on or off, and sets how long it lasts.
    /// </summary>
    /// <param name="subscriptionGracePeriodId">The opaque resource ID of the billing grace period.</param>
    /// <param name="request">The settings to change.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Grace Periods resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptiongraceperiods-_id_"/>
    public async Task<ResourceResponse<SubscriptionGracePeriod>> UpdateSubscriptionGracePeriodAsync(
        string subscriptionGracePeriodId,
        SubscriptionGracePeriodUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionGracePeriod>>(
                path: $"/v1/subscriptionGracePeriods/{subscriptionGracePeriodId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an introductory offer for a subscription, which customers who have never subscribed
    /// to its subscription group can redeem once.
    /// </summary>
    /// <param name="request">The offer to create, and the subscription, territory, and price point it applies to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Introductory Offers resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionintroductoryoffers"/>
    public async Task<ResourceResponse<SubscriptionIntroductoryOffer>> CreateSubscriptionIntroductoryOfferAsync(
        SubscriptionIntroductoryOfferCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionIntroductoryOffer>>(
                path: "/v1/subscriptionIntroductoryOffers",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes when an introductory offer stops being available.
    /// </summary>
    /// <param name="subscriptionIntroductoryOfferId">The opaque resource ID of the introductory offer.</param>
    /// <param name="request">The end date to set.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Introductory Offers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionintroductoryoffers-_id_"/>
    public async Task<ResourceResponse<SubscriptionIntroductoryOffer>> UpdateSubscriptionIntroductoryOfferAsync(
        string subscriptionIntroductoryOfferId,
        SubscriptionIntroductoryOfferUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionIntroductoryOffer>>(
                path: $"/v1/subscriptionIntroductoryOffers/{subscriptionIntroductoryOfferId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an introductory offer that hasn't started yet.
    /// </summary>
    /// <param name="subscriptionIntroductoryOfferId">The opaque resource ID of the introductory offer to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepted the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionintroductoryoffers-_id_"/>
    public async Task DeleteSubscriptionIntroductoryOfferAsync(
        string subscriptionIntroductoryOfferId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionIntroductoryOffers/{subscriptionIntroductoryOfferId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a promotional offer for a subscription, which your app signs and presents to a
    /// current or lapsed subscriber.
    /// </summary>
    /// <param name="request">The offer to create, together with the prices it charges in each territory.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Promotional Offers resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-subscriptionpromotionaloffers"/>
    public async Task<ResourceResponse<SubscriptionPromotionalOffer>> CreateSubscriptionPromotionalOfferAsync(
        SubscriptionPromotionalOfferCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPromotionalOffer>>(
                path: "/v1/subscriptionPromotionalOffers",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one promotional offer.
    /// </summary>
    /// <param name="subscriptionPromotionalOfferId">The opaque resource ID of the promotional offer.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Subscription Promotional Offers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionpromotionaloffers-_id_"/>
    public async Task<ResourceResponse<SubscriptionPromotionalOffer>> GetSubscriptionPromotionalOfferAsync(
        string subscriptionPromotionalOfferId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPromotionalOffer>>(
                path: $"/v1/subscriptionPromotionalOffers/{subscriptionPromotionalOfferId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the prices a promotional offer charges.
    /// </summary>
    /// <param name="subscriptionPromotionalOfferId">The opaque resource ID of the promotional offer.</param>
    /// <param name="request">The prices to set, listed in the request's included array.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Subscription Promotional Offers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-subscriptionpromotionaloffers-_id_"/>
    public async Task<ResourceResponse<SubscriptionPromotionalOffer>> UpdateSubscriptionPromotionalOfferAsync(
        string subscriptionPromotionalOfferId,
        SubscriptionPromotionalOfferUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<SubscriptionPromotionalOffer>>(
                path: $"/v1/subscriptionPromotionalOffers/{subscriptionPromotionalOfferId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a promotional offer.
    /// </summary>
    /// <param name="subscriptionPromotionalOfferId">The opaque resource ID of the promotional offer to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepted the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-subscriptionpromotionaloffers-_id_"/>
    public async Task DeleteSubscriptionPromotionalOfferAsync(
        string subscriptionPromotionalOfferId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/subscriptionPromotionalOffers/{subscriptionPromotionalOfferId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists what a promotional offer costs in each territory.
    /// </summary>
    /// <param name="subscriptionPromotionalOfferId">The opaque resource ID of the promotional offer.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Subscription Promotional Offer Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-subscriptionpromotionaloffers-_id_-prices"/>
    public async Task<ResourceListResponse<SubscriptionPromotionalOfferPrice>> ListPricesForSubscriptionPromotionalOfferAsync(
        string subscriptionPromotionalOfferId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<SubscriptionPromotionalOfferPrice>>(
                path: $"/v1/subscriptionPromotionalOffers/{subscriptionPromotionalOfferId}/prices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a win-back offer for a subscription, which the App Store presents to customers whose
    /// subscription lapsed and who match the offer's eligibility rules.
    /// </summary>
    /// <param name="request">The offer to create, together with the prices it charges in each territory.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Win-Back Offers resource you created.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-winbackoffers"/>
    public async Task<ResourceResponse<WinBackOffer>> CreateWinBackOfferAsync(
        WinBackOfferCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<WinBackOffer>>(
                path: "/v1/winBackOffers",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads one win-back offer.
    /// </summary>
    /// <param name="winBackOfferId">The opaque resource ID of the win-back offer.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the Win-Back Offers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-winbackoffers-_id_"/>
    public async Task<ResourceResponse<WinBackOffer>> GetWinBackOfferAsync(
        string winBackOfferId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<WinBackOffer>>(
                path: $"/v1/winBackOffers/{winBackOfferId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Changes a win-back offer's schedule, its priority, or the rules that decide who qualifies
    /// for it.
    /// </summary>
    /// <param name="winBackOfferId">The opaque resource ID of the win-back offer.</param>
    /// <param name="request">The attributes to change.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the updated Win-Back Offers resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/patch-v1-winbackoffers-_id_"/>
    public async Task<ResourceResponse<WinBackOffer>> UpdateWinBackOfferAsync(
        string winBackOfferId,
        WinBackOfferUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<WinBackOffer>>(
                path: $"/v1/winBackOffers/{winBackOfferId}",
                method: HttpMethod.Patch,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a win-back offer.
    /// </summary>
    /// <param name="winBackOfferId">The opaque resource ID of the win-back offer to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the App Store Connect API accepted the deletion.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-winbackoffers-_id_"/>
    public async Task DeleteWinBackOfferAsync(
        string winBackOfferId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/winBackOffers/{winBackOfferId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists what a win-back offer costs in each territory.
    /// </summary>
    /// <param name="winBackOfferId">The opaque resource ID of the win-back offer.</param>
    /// <param name="query">Optional query parameters such as filters, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Win-Back Offer Prices resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-winbackoffers-_id_-prices"/>
    public async Task<ResourceListResponse<WinBackOfferPrice>> ListPricesForWinBackOfferAsync(
        string winBackOfferId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<WinBackOfferPrice>>(
                path: $"/v1/winBackOffers/{winBackOfferId}/prices",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
