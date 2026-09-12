using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class SubscriptionPricingTests
{
    [Fact]
    public async Task DecodesPricePointAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPricePointsResponse.json");

        var response = await client.ListPricePointsForSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067/pricePoints",
            handler.CapturedRequest.RequestUri!.ToString());

        var pricePoints = response.Data;
        Assert.Equal(2, pricePoints.Length);
        Assert.Equal("subscriptionPricePoints", pricePoints[0].Type);

        var unitedStates = pricePoints[0].Attributes!;
        Assert.Equal("9.99", unitedStates.CustomerPrice);
        Assert.Equal("6.99", unitedStates.Proceeds);
        Assert.Equal("8.49", unitedStates.ProceedsYear2);
        Assert.Equal("USA", pricePoints[0].Relationships!["territory"].ToOne()!.Id);

        var canada = pricePoints[1].Attributes!;
        Assert.Equal("12.99", canada.CustomerPrice);
        Assert.Equal("CAN", pricePoints[1].Relationships!["territory"].ToOne()!.Id);

        Assert.Equal(175, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
    }

    [Fact]
    public async Task DecodesSubscriptionPriceAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPricesResponse.json");

        var response = await client.ListPricesForSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067/prices",
            handler.CapturedRequest!.RequestUri!.ToString());

        var prices = response.Data;
        var monthly = prices[0].Attributes!;
        Assert.Equal(new DateOnly(2024, 3, 1), monthly.StartDate);
        Assert.False(monthly.Preserved);
        Assert.Equal(SubscriptionPlanType.Monthly, monthly.PlanType);
        Assert.Equal(
            "eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ",
            prices[0].Relationships!["subscriptionPricePoint"].ToOne()!.Id);

        var upfront = prices[1].Attributes!;
        Assert.Null(upfront.StartDate);
        Assert.True(upfront.Preserved);
        Assert.Equal(SubscriptionPlanType.Upfront, upfront.PlanType);
    }

    [Fact]
    public async Task SendsCommaJoinedFilterValues()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPricesResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("territory", "USA", "CAN")
            .Filter("planType", "MONTHLY")
            .Limit(200);

        await client.ListPricesForSubscriptionAsync("6446075067", query, TestContext.Current.CancellationToken);

        var url = Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.ToString());
        Assert.Contains("filter[territory]=USA,CAN", url);
        Assert.Contains("filter[planType]=MONTHLY", url);
        Assert.Contains("limit=200", url);
    }

    [Fact]
    public async Task SendsPriceCreateUrlAndBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.subscriptionPriceCreateResponse.json",
            HttpStatusCode.Created);

        var request = new SubscriptionPriceCreateRequest
        {
            Data = new SubscriptionPriceCreateRequestData
            {
                Attributes = new SubscriptionPriceCreateRequestDataAttributes
                {
                    StartDate = new DateOnly(2025, 1, 15),
                    PreserveCurrentPrice = true,
                    PlanType = SubscriptionPlanType.Upfront
                },
                Relationships = new SubscriptionPriceCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    Territory = RelationshipDeclaration.To("territories", "DEU"),
                    SubscriptionPricePoint = RelationshipDeclaration.To(
                        "subscriptionPricePoints",
                        "eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJERVUifQ")
                }
            }
        };

        var response = await client.CreateSubscriptionPriceAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPrices",
            handler.CapturedRequest.RequestUri!.ToString());

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptionPrices", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.Equal("2025-01-15", attributes.GetProperty("startDate").GetString());
        Assert.True(attributes.GetProperty("preserveCurrentPrice").GetBoolean());
        Assert.Equal("UPFRONT", attributes.GetProperty("planType").GetString());

        var relationships = data.GetProperty("relationships");
        Assert.Equal(
            "6446075067",
            relationships.GetProperty("subscription").GetProperty("data").GetProperty("id").GetString());
        Assert.Equal(
            "territories",
            relationships.GetProperty("territory").GetProperty("data").GetProperty("type").GetString());
        Assert.Equal(
            "eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJERVUifQ",
            relationships.GetProperty("subscriptionPricePoint").GetProperty("data").GetProperty("id").GetString());

        Assert.Equal(new DateOnly(2025, 1, 15), response.Data.Attributes!.StartDate);
    }

    [Fact]
    public async Task DeletesSubscriptionPriceWithoutBodyOrResponse()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteSubscriptionPriceAsync(
            "eyJzIjoiNjQ0NjA3NTA2NyIsInQiOiJERVUifQ",
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPrices/eyJzIjoiNjQ0NjA3NTA2NyIsInQiOiJERVUifQ",
            handler.CapturedRequest.RequestUri!.ToString());
        Assert.Null(handler.CapturedRequestBody);
    }

    [Fact]
    public async Task DecodesIntroductoryOfferAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionIntroductoryOffersResponse.json");

        var response = await client.ListIntroductoryOffersForSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067/introductoryOffers",
            handler.CapturedRequest!.RequestUri!.ToString());

        var offers = response.Data;
        Assert.Equal(2, offers.Length);

        var trial = offers[0].Attributes!;
        Assert.Equal(new DateOnly(2024, 6, 1), trial.StartDate);
        Assert.Equal(new DateOnly(2024, 9, 30), trial.EndDate);
        Assert.Equal(SubscriptionOfferDuration.ThreeDays, trial.Duration);
        Assert.Equal(SubscriptionOfferMode.FreeTrial, trial.OfferMode);
        Assert.Equal(1, trial.NumberOfPeriods);
        Assert.Equal(SubscriptionPlanType.Monthly, trial.TargetSubscriptionPlanType);
        Assert.Equal("USA", offers[0].Relationships!["territory"].ToOne()!.Id);

        var payAsYouGo = offers[1].Attributes!;
        Assert.Null(payAsYouGo.EndDate);
        Assert.Equal(SubscriptionOfferDuration.SixMonths, payAsYouGo.Duration);
        Assert.Equal(SubscriptionOfferMode.PayAsYouGo, payAsYouGo.OfferMode);
        Assert.Equal(6, payAsYouGo.NumberOfPeriods);
        Assert.Equal(SubscriptionPlanType.Upfront, payAsYouGo.TargetSubscriptionPlanType);
    }

    [Fact]
    public async Task DecodesPromotionalOfferAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPromotionalOfferResponse.json");

        var response = await client.GetSubscriptionPromotionalOfferAsync(
            "c9d8e7f6-0001",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPromotionalOffers/c9d8e7f6-0001",
            handler.CapturedRequest!.RequestUri!.ToString());

        var offer = response.Data;
        Assert.Equal("subscriptionPromotionalOffers", offer.Type);
        Assert.Equal("Welcome back", offer.Attributes!.Name);
        Assert.Equal("WELCOMEBACK", offer.Attributes.OfferCode);
        Assert.Equal(SubscriptionOfferDuration.TwoWeeks, offer.Attributes.Duration);
        Assert.Equal(SubscriptionOfferMode.PayUpFront, offer.Attributes.OfferMode);
        Assert.Equal(3, offer.Attributes.NumberOfPeriods);
        Assert.Equal(SubscriptionPlanType.Upfront, offer.Attributes.TargetSubscriptionPlanType);

        var prices = offer.Relationships!["prices"].ToMany();
        Assert.Single(prices);
        Assert.Equal("subscriptionPromotionalOfferPrices", prices[0].Type);
        Assert.Equal("eyJvIjoiYzlkOGU3ZjYtMDAwMSIsInQiOiJVU0EifQ", prices[0].Id);
    }

    [Fact]
    public async Task SendsPromotionalOfferIdInPatchBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPromotionalOfferResponse.json");

        var request = new SubscriptionPromotionalOfferUpdateRequest
        {
            Data = new SubscriptionPromotionalOfferUpdateRequestData
            {
                Id = "c9d8e7f6-0001",
                Relationships = new SubscriptionPromotionalOfferUpdateRequestDataRelationships
                {
                    Prices = RelationshipDeclarationList.To(
                        "subscriptionPromotionalOfferPrices",
                        "price-usa",
                        "price-can")
                }
            }
        };

        await client.UpdateSubscriptionPromotionalOfferAsync(
            "c9d8e7f6-0001",
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPromotionalOffers/c9d8e7f6-0001",
            handler.CapturedRequest.RequestUri!.ToString());

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptionPromotionalOffers", data.GetProperty("type").GetString());
        Assert.Equal("c9d8e7f6-0001", data.GetProperty("id").GetString());

        var prices = data.GetProperty("relationships").GetProperty("prices").GetProperty("data");
        Assert.Equal(2, prices.GetArrayLength());
        Assert.Equal("subscriptionPromotionalOfferPrices", prices[0].GetProperty("type").GetString());
        Assert.Equal("price-can", prices[1].GetProperty("id").GetString());
    }

    [Fact]
    public async Task DecodesWinBackOfferAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionWinBackOfferResponse.json");

        var response = await client.GetWinBackOfferAsync(
            "e1f2a3b4-0001",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/winBackOffers/e1f2a3b4-0001",
            handler.CapturedRequest!.RequestUri!.ToString());

        var offer = response.Data.Attributes!;
        Assert.Equal("Lapsed annual win-back", offer.ReferenceName);
        Assert.Equal("winback_annual_2025", offer.OfferId);
        Assert.Equal(SubscriptionOfferDuration.OneMonth, offer.Duration);
        Assert.Equal(SubscriptionOfferMode.PayAsYouGo, offer.OfferMode);
        Assert.Equal(3, offer.PeriodCount);
        Assert.Equal(6, offer.CustomerEligibilityPaidSubscriptionDurationInMonths);
        Assert.Equal(2, offer.CustomerEligibilityTimeSinceLastSubscribedInMonths!.Minimum);
        Assert.Equal(12, offer.CustomerEligibilityTimeSinceLastSubscribedInMonths.Maximum);
        Assert.Equal(1, offer.CustomerEligibilityWaitBetweenOffersInMonths);
        Assert.Equal(new DateOnly(2025, 2, 1), offer.StartDate);
        Assert.Equal(new DateOnly(2025, 12, 31), offer.EndDate);
        Assert.Equal(WinBackOfferPriority.High, offer.Priority);
        Assert.Equal(WinBackOfferPromotionIntent.UseAutoGeneratedAssets, offer.PromotionIntent);
        Assert.Equal(SubscriptionPlanType.Monthly, offer.TargetSubscriptionPlanType);
    }

    [Fact]
    public async Task FallsBackToUnmappedForUnknownOfferEnumValues()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.subscriptionUnknownOfferEnumsResponse.json");

        var response = await client.GetSubscriptionPromotionalOfferAsync(
            "c9d8e7f6-9999",
            cancellationToken: TestContext.Current.CancellationToken);

        var offer = response.Data.Attributes!;

        Assert.Equal(SubscriptionOfferDuration._Unmapped, offer.Duration);
        Assert.Equal(SubscriptionOfferMode._Unmapped, offer.OfferMode);
        Assert.Equal(SubscriptionPlanType._Unmapped, offer.TargetSubscriptionPlanType);

        Assert.Equal("Offer from a newer API", offer.Name);
        Assert.Equal("FUTURE", offer.OfferCode);
    }

    [Fact]
    public async Task DecodesAvailabilityWithIncludedTerritories()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionAvailabilityResponse.json");

        var query = new AppStoreConnectQuery().Include("availableTerritories");

        var response = await client.GetSubscriptionAvailabilityAsync(
            "f0e9d8c7-0001",
            query,
            TestContext.Current.CancellationToken);

        var url = Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.ToString());
        Assert.StartsWith(
            "https://api.appstoreconnect.apple.com/v1/subscriptionAvailabilities/f0e9d8c7-0001?",
            url);
        Assert.Contains("include=availableTerritories", url);

        Assert.True(response.Data.Attributes!.AvailableInNewTerritories);

        var territories = response.Data.Relationships!["availableTerritories"].ToMany();
        Assert.Equal(2, territories.Length);
        Assert.Equal("USA", territories[0].Id);
        Assert.Equal("JPN", territories[1].Id);

        Assert.True(response.TryGetIncluded<Territory>("territories", "JPN", out var japan));
        Assert.Equal("JPY", japan.Attributes!.Currency);
        Assert.False(response.TryGetIncluded<Territory>("territories", "DEU", out _));
    }

    [Fact]
    public async Task DecodesPlanAvailabilityWithIncludedTerritories()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionPlanAvailabilityResponse.json");

        var query = new AppStoreConnectQuery().Include("availableTerritories");

        var response = await client.GetSubscriptionPlanAvailabilityAsync(
            "a1b2c3d4-0001",
            query,
            TestContext.Current.CancellationToken);

        var url = Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.ToString());
        Assert.StartsWith(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPlanAvailabilities/a1b2c3d4-0001?",
            url);
        Assert.Contains("include=availableTerritories", url);

        var planAvailability = response.Data;
        Assert.Equal("subscriptionPlanAvailabilities", planAvailability.Type);
        Assert.True(planAvailability.Attributes!.AvailableInNewTerritories);
        Assert.Equal(SubscriptionPlanType.Monthly, planAvailability.Attributes.PlanType);

        var territories = planAvailability.Relationships!["availableTerritories"].ToMany();
        Assert.Equal(2, territories.Length);
        Assert.Equal("USA", territories[0].Id);
        Assert.Equal("JPN", territories[1].Id);

        Assert.True(response.TryGetIncluded<Territory>("territories", "USA", out var unitedStates));
        Assert.Equal("USD", unitedStates.Attributes!.Currency);
    }

    [Fact]
    public async Task SendsPlanAvailabilityCreateUrlAndBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.subscriptionPlanAvailabilityResponse.json",
            HttpStatusCode.Created);

        var request = new SubscriptionPlanAvailabilityCreateRequest
        {
            Data = new SubscriptionPlanAvailabilityCreateRequestData
            {
                Attributes = new SubscriptionPlanAvailabilityCreateRequestDataAttributes
                {
                    AvailableInNewTerritories = true,
                    PlanType = SubscriptionPlanType.Monthly
                },
                Relationships = new SubscriptionPlanAvailabilityCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    AvailableTerritories = RelationshipDeclarationList.To("territories", "USA", "JPN")
                }
            }
        };

        var response = await client.CreateSubscriptionPlanAvailabilityAsync(
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPlanAvailabilities",
            handler.CapturedRequest.RequestUri!.ToString());

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptionPlanAvailabilities", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.True(attributes.GetProperty("availableInNewTerritories").GetBoolean());
        Assert.Equal("MONTHLY", attributes.GetProperty("planType").GetString());

        var relationships = data.GetProperty("relationships");
        Assert.Equal(
            "6446075067",
            relationships.GetProperty("subscription").GetProperty("data").GetProperty("id").GetString());

        var territories = relationships.GetProperty("availableTerritories").GetProperty("data");
        Assert.Equal(2, territories.GetArrayLength());
        Assert.Equal("territories", territories[0].GetProperty("type").GetString());
        Assert.Equal("USA", territories[0].GetProperty("id").GetString());
        Assert.Equal("JPN", territories[1].GetProperty("id").GetString());

        Assert.Equal(SubscriptionPlanType.Monthly, response.Data.Attributes!.PlanType);
    }

    [Fact]
    public async Task SendsAvailableTerritoriesLinkagesPatch()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.ReplaceAvailableTerritoriesForSubscriptionPlanAvailabilityAsync(
            "a1b2c3d4-0001",
            ["USA", "JPN", "DEU"],
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionPlanAvailabilities/a1b2c3d4-0001/relationships/availableTerritories",
            handler.CapturedRequest.RequestUri!.ToString());

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal(3, data.GetArrayLength());
        Assert.Equal("territories", data[0].GetProperty("type").GetString());
        Assert.Equal("USA", data[0].GetProperty("id").GetString());
        Assert.Equal("DEU", data[2].GetProperty("id").GetString());
    }
}
