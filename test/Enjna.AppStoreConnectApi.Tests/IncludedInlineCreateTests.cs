using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

/// <summary>
/// Pins the JSON of every request body that carries an <c>included</c> array. Each expected body
/// below is the JSON the array produced while it was typed <c>object[]</c>, which is the shape
/// Apple already accepted. Apart from the price schedule, which is pinned in full, each payload
/// carries only the parts of <c>data</c> that the included entries are referenced from.
/// </summary>
public class IncludedInlineCreateTests
{
    private const string PriceScheduleResponse =
        "{\"data\":{\"type\":\"inAppPurchasePriceSchedules\",\"id\":\"6446819279\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/inAppPurchasePriceSchedules/6446819279\"}}";

    private const string SubscriptionResponse =
        "{\"data\":{\"type\":\"subscriptions\",\"id\":\"6446075067\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067\"}}";

    private const string SubscriptionOfferCodeResponse =
        "{\"data\":{\"type\":\"subscriptionOfferCodes\",\"id\":\"c1d2e3f4-0001\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptionOfferCodes/c1d2e3f4-0001\"}}";

    private const string InAppPurchaseOfferCodeResponse =
        "{\"data\":{\"type\":\"inAppPurchaseOfferCodes\",\"id\":\"a1b2c3d4-0002\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/inAppPurchaseOfferCodes/a1b2c3d4-0002\"}}";

    private const string IntroductoryOfferResponse =
        "{\"data\":{\"type\":\"subscriptionIntroductoryOffers\",\"id\":\"b2c3d4e5-0003\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptionIntroductoryOffers/b2c3d4e5-0003\"}}";

    private const string PromotionalOfferResponse =
        "{\"data\":{\"type\":\"subscriptionPromotionalOffers\",\"id\":\"c9d8e7f6-0001\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptionPromotionalOffers/c9d8e7f6-0001\"}}";

    private const string WinBackOfferResponse =
        "{\"data\":{\"type\":\"winBackOffers\",\"id\":\"e1f2a3b4-0001\"},\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/winBackOffers/e1f2a3b4-0001\"}}";

    private const string UsaPricePoint = "eyJzIjoiNjQ0NjgxOTI3OSIsInAiOiIxMDAwNyIsInQiOiJVU0EifQ";

    private const string UsaSubscriptionPricePoint = "eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ";

    private const string ExpectedPriceScheduleBody =
        "{\"data\":{\"type\":\"inAppPurchasePriceSchedules\",\"relationships\":{\"inAppPurchase\":{\"data\":{"
        + "\"type\":\"inAppPurchases\",\"id\":\"6446819279\"}},\"baseTerritory\":{\"data\":{\"type\":\"territories\","
        + "\"id\":\"USA\"}},\"manualPrices\":{\"data\":[{\"type\":\"inAppPurchasePrices\",\"id\":\"${price}\"}]}}},\"included\":["
        + "{\"type\":\"inAppPurchasePrices\",\"id\":\"${price}\",\"relationships\":{\"inAppPurchasePricePoint\":{\"data\":{"
        + "\"type\":\"inAppPurchasePricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjgxOTI3OSIsInAiOiIxMDAwNyIsInQiOiJVU0EifQ\"}}}}]}";

    private const string ExpectedPriceScheduleWithTerritoryBody =
        "{\"data\":{\"type\":\"inAppPurchasePriceSchedules\",\"relationships\":{\"inAppPurchase\":{\"data\":{"
        + "\"type\":\"inAppPurchases\",\"id\":\"6446819279\"}},\"baseTerritory\":{\"data\":{\"type\":\"territories\","
        + "\"id\":\"USA\"}},\"manualPrices\":{\"data\":[{\"type\":\"inAppPurchasePrices\",\"id\":\"${price}\"}]}}},\"included\":["
        + "{\"type\":\"inAppPurchasePrices\",\"id\":\"${price}\",\"attributes\":{\"startDate\":\"2025-01-15\"},"
        + "\"relationships\":{\"inAppPurchasePricePoint\":{\"data\":{\"type\":\"inAppPurchasePricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjgxOTI3OSIsInAiOiIxMDAwNyIsInQiOiJVU0EifQ\"}}}},{\"type\":\"territories\","
        + "\"id\":\"USA\"}]}";

    private const string ExpectedSubscriptionUpdateBody =
        "{\"data\":{\"type\":\"subscriptions\",\"id\":\"6446075067\",\"relationships\":{\"introductoryOffers\":{\"data\":[{"
        + "\"type\":\"subscriptionIntroductoryOffers\",\"id\":\"${introductory}\"}]},\"promotionalOffers\":{\"data\":[{"
        + "\"type\":\"subscriptionPromotionalOffers\",\"id\":\"${promotional}\"}]},\"prices\":{\"data\":[{"
        + "\"type\":\"subscriptionPrices\",\"id\":\"${price}\"}]}}},\"included\":[{\"type\":\"subscriptionPrices\","
        + "\"id\":\"${price}\",\"attributes\":{\"startDate\":\"2025-01-15\",\"preserveCurrentPrice\":true,"
        + "\"planType\":\"MONTHLY\"},\"relationships\":{\"subscription\":{\"data\":{\"type\":\"subscriptions\","
        + "\"id\":\"6446075067\"}},\"territory\":{\"data\":{\"type\":\"territories\",\"id\":\"USA\"}},"
        + "\"subscriptionPricePoint\":{\"data\":{\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}},{"
        + "\"type\":\"subscriptionIntroductoryOffers\",\"id\":\"${introductory}\",\"attributes\":{"
        + "\"startDate\":\"2025-02-01\",\"endDate\":\"2025-03-01\",\"duration\":\"THREE_DAYS\",\"offerMode\":\"FREE_TRIAL\","
        + "\"numberOfPeriods\":1},\"relationships\":{\"subscription\":{\"data\":{\"type\":\"subscriptions\","
        + "\"id\":\"6446075067\"}},\"territory\":{\"data\":{\"type\":\"territories\",\"id\":\"USA\"}},"
        + "\"subscriptionPricePoint\":{\"data\":{\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}},{"
        + "\"type\":\"subscriptionPromotionalOffers\",\"id\":\"${promotional}\",\"attributes\":{\"duration\":\"TWO_WEEKS\","
        + "\"name\":\"Welcome back\",\"numberOfPeriods\":3,\"offerCode\":\"WELCOMEBACK\",\"offerMode\":\"PAY_UP_FRONT\"},"
        + "\"relationships\":{\"subscription\":{\"data\":{\"type\":\"subscriptions\",\"id\":\"6446075067\"}}}}]}";

    private const string ExpectedSubscriptionOfferCodeBody =
        "{\"data\":{\"type\":\"subscriptionOfferCodes\",\"attributes\":{\"name\":\"Three Months On Us\","
        + "\"customerEligibilities\":[\"NEW\",\"EXPIRED\"],\"offerEligibility\":\"STACK_WITH_INTRO_OFFERS\","
        + "\"duration\":\"THREE_MONTHS\",\"offerMode\":\"FREE_TRIAL\",\"numberOfPeriods\":1},\"relationships\":{"
        + "\"subscription\":{\"data\":{\"type\":\"subscriptions\",\"id\":\"6446075067\"}},\"prices\":{\"data\":[{"
        + "\"type\":\"subscriptionOfferCodePrices\",\"id\":\"${price}\"}]}}},\"included\":[{"
        + "\"type\":\"subscriptionOfferCodePrices\",\"id\":\"${price}\",\"relationships\":{\"territory\":{"
        + "\"data\":{\"type\":\"territories\",\"id\":\"USA\"}},\"subscriptionPricePoint\":{\"data\":{"
        + "\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}}]}";

    private const string ExpectedInAppPurchaseOfferCodeBody =
        "{\"data\":{\"type\":\"inAppPurchaseOfferCodes\",\"attributes\":{\"name\":\"Launch Week Coins\","
        + "\"customerEligibilities\":[\"NON_SPENDER\",\"CHURNED_SPENDER\"]},\"relationships\":{\"inAppPurchase\":{"
        + "\"data\":{\"type\":\"inAppPurchases\",\"id\":\"6446819279\"}},\"prices\":{\"data\":[{"
        + "\"type\":\"inAppPurchaseOfferPrices\",\"id\":\"${price}\"}]}}},\"included\":[{"
        + "\"type\":\"inAppPurchaseOfferPrices\",\"id\":\"${price}\",\"relationships\":{\"territory\":{\"data\":{"
        + "\"type\":\"territories\",\"id\":\"USA\"}},\"pricePoint\":{\"data\":{"
        + "\"type\":\"inAppPurchasePricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjgxOTI3OSIsInAiOiIxMDAwNyIsInQiOiJVU0EifQ\"}}}}]}";

    private const string ExpectedIntroductoryOfferBody =
        "{\"data\":{\"type\":\"subscriptionIntroductoryOffers\",\"attributes\":{\"duration\":\"THREE_MONTHS\","
        + "\"offerMode\":\"PAY_UP_FRONT\",\"numberOfPeriods\":1},\"relationships\":{\"subscription\":{\"data\":{"
        + "\"type\":\"subscriptions\",\"id\":\"6446075067\"}},\"territory\":{\"data\":{\"type\":\"territories\","
        + "\"id\":\"USA\"}},\"subscriptionPricePoint\":{\"data\":{\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}},\"included\":[{"
        + "\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}]}";

    private const string ExpectedPromotionalOfferCreateBody =
        "{\"data\":{\"type\":\"subscriptionPromotionalOffers\",\"attributes\":{\"duration\":\"ONE_MONTH\","
        + "\"name\":\"Welcome back\",\"numberOfPeriods\":3,\"offerCode\":\"WELCOMEBACK\","
        + "\"offerMode\":\"PAY_AS_YOU_GO\"},\"relationships\":{\"subscription\":{\"data\":{"
        + "\"type\":\"subscriptions\",\"id\":\"6446075067\"}},\"prices\":{\"data\":[{"
        + "\"type\":\"subscriptionPromotionalOfferPrices\",\"id\":\"${price}\"}]}}},\"included\":[{"
        + "\"type\":\"subscriptionPromotionalOfferPrices\",\"id\":\"${price}\",\"relationships\":{\"territory\":{"
        + "\"data\":{\"type\":\"territories\",\"id\":\"USA\"}},\"subscriptionPricePoint\":{\"data\":{"
        + "\"type\":\"subscriptionPricePoints\","
        + "\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}}]}";

    private const string ExpectedPromotionalOfferUpdateBody =
        "{\"data\":{\"type\":\"subscriptionPromotionalOffers\",\"id\":\"c9d8e7f6-0001\",\"relationships\":{\"prices\":{"
        + "\"data\":[{\"type\":\"subscriptionPromotionalOfferPrices\",\"id\":\"${price}\"}]}}},\"included\":[{"
        + "\"type\":\"subscriptionPromotionalOfferPrices\",\"id\":\"${price}\",\"relationships\":{\"territory\":{\"data\":{"
        + "\"type\":\"territories\",\"id\":\"USA\"}},\"subscriptionPricePoint\":{\"data\":{"
        + "\"type\":\"subscriptionPricePoints\",\"id\":\"eyJzIjoiNjQ0NjA3NTA2NyIsInAiOiIxMDAwMSIsInQiOiJVU0EifQ\"}}}}]}";

    private const string ExpectedWinBackOfferBody =
        "{\"data\":{\"type\":\"winBackOffers\",\"attributes\":{\"referenceName\":\"Lapsed annual win-back\","
        + "\"offerId\":\"winback_annual_2025\",\"duration\":\"ONE_MONTH\",\"offerMode\":\"PAY_AS_YOU_GO\","
        + "\"periodCount\":3,\"customerEligibilityPaidSubscriptionDurationInMonths\":6,"
        + "\"customerEligibilityTimeSinceLastSubscribedInMonths\":{\"minimum\":2,\"maximum\":12},"
        + "\"startDate\":\"2025-02-01\",\"priority\":\"HIGH\"},\"relationships\":{\"subscription\":{\"data\":{"
        + "\"type\":\"subscriptions\",\"id\":\"6446075067\"}},\"prices\":{\"data\":[{"
        + "\"type\":\"winBackOfferPrices\",\"id\":\"${price}\"}]}}},\"included\":[{\"type\":\"winBackOfferPrices\","
        + "\"id\":\"${price}\"}]}";

    [Fact]
    public async Task SendsProvenPriceScheduleBody()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(PriceScheduleResponse, HttpStatusCode.Created);

        await client.CreateInAppPurchasePriceScheduleAsync(
            BuildProvenPriceScheduleRequest(),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/inAppPurchasePriceSchedules",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(ExpectedPriceScheduleBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task DescribesProvenPriceScheduleBody()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(PriceScheduleResponse, HttpStatusCode.Created);

        await client.CreateInAppPurchasePriceScheduleAsync(
            BuildProvenPriceScheduleRequest(),
            TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("inAppPurchasePriceSchedules", data.GetProperty("type").GetString());

        var relationships = data.GetProperty("relationships");
        var inAppPurchase = relationships.GetProperty("inAppPurchase").GetProperty("data");

        Assert.Equal("inAppPurchases", inAppPurchase.GetProperty("type").GetString());
        Assert.Equal("6446819279", inAppPurchase.GetProperty("id").GetString());

        var baseTerritory = relationships.GetProperty("baseTerritory").GetProperty("data");

        Assert.Equal("territories", baseTerritory.GetProperty("type").GetString());
        Assert.Equal("USA", baseTerritory.GetProperty("id").GetString());

        var manualPrices = relationships.GetProperty("manualPrices").GetProperty("data");

        Assert.Equal(1, manualPrices.GetArrayLength());
        Assert.Equal("inAppPurchasePrices", manualPrices[0].GetProperty("type").GetString());
        Assert.Equal("${price}", manualPrices[0].GetProperty("id").GetString());

        var included = body.RootElement.GetProperty("included");

        Assert.Equal(1, included.GetArrayLength());
        Assert.Equal("inAppPurchasePrices", included[0].GetProperty("type").GetString());
        Assert.Equal("${price}", included[0].GetProperty("id").GetString());

        var pricePoint = included[0]
            .GetProperty("relationships")
            .GetProperty("inAppPurchasePricePoint")
            .GetProperty("data");

        Assert.Equal("inAppPurchasePricePoints", pricePoint.GetProperty("type").GetString());
        Assert.Equal(UsaPricePoint, pricePoint.GetProperty("id").GetString());
    }

    [Fact]
    public async Task SendsPriceScheduleIncludedArrayHoldingBothAllowedTypes()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(PriceScheduleResponse, HttpStatusCode.Created);

        var request = new InAppPurchasePriceScheduleCreateRequest
        {
            Data = new InAppPurchasePriceScheduleCreateRequestData
            {
                Relationships = new InAppPurchasePriceScheduleCreateRequestDataRelationships
                {
                    InAppPurchase = RelationshipDeclaration.To("inAppPurchases", "6446819279"),
                    BaseTerritory = RelationshipDeclaration.To("territories", "USA"),
                    ManualPrices = RelationshipDeclarationList.To("inAppPurchasePrices", "${price}")
                }
            },
            Included =
            [
                new InAppPurchasePriceInlineCreate
                {
                    Id = "${price}",
                    Attributes = new InAppPurchasePriceInlineCreateAttributes
                    {
                        StartDate = new DateOnly(2025, 1, 15)
                    },
                    Relationships = new InAppPurchasePriceInlineCreateRelationships
                    {
                        InAppPurchasePricePoint = RelationshipDeclaration.To(
                            "inAppPurchasePricePoints",
                            UsaPricePoint)
                    }
                },
                new TerritoryInlineCreate { Id = "USA" }
            ]
        };

        await client.CreateInAppPurchasePriceScheduleAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedPriceScheduleWithTerritoryBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task RefusesAnIncludedEntryApplesSchemaDoesNotAllow()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestUtilities.CreateClient(handler);

        var request = new InAppPurchasePriceScheduleCreateRequest
        {
            Data = new InAppPurchasePriceScheduleCreateRequestData
            {
                Relationships = new InAppPurchasePriceScheduleCreateRequestDataRelationships
                {
                    InAppPurchase = RelationshipDeclaration.To("inAppPurchases", "6446819279"),
                    BaseTerritory = RelationshipDeclaration.To("territories", "USA"),
                    ManualPrices = RelationshipDeclarationList.To("inAppPurchasePrices", "${price}")
                }
            },
            Included = [new UnknownIncludedResource()]
        };

        await Assert.ThrowsAsync<NotSupportedException>(
            () => client.CreateInAppPurchasePriceScheduleAsync(request, TestContext.Current.CancellationToken));

        Assert.Empty(handler.CapturedRequests);
    }

    [Fact]
    public async Task SendsSubscriptionUpdateIncludedArrayHoldingAllThreeAllowedTypes()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(SubscriptionResponse);

        var request = new SubscriptionUpdateRequest
        {
            Data = new SubscriptionUpdateRequestData
            {
                Id = "6446075067",
                Relationships = new SubscriptionUpdateRequestDataRelationships
                {
                    IntroductoryOffers = RelationshipDeclarationList.To(
                        "subscriptionIntroductoryOffers",
                        "${introductory}"),
                    PromotionalOffers = RelationshipDeclarationList.To(
                        "subscriptionPromotionalOffers",
                        "${promotional}"),
                    Prices = RelationshipDeclarationList.To("subscriptionPrices", "${price}")
                }
            },
            Included =
            [
                new SubscriptionPriceInlineCreate
                {
                    Id = "${price}",
                    Attributes = new SubscriptionPriceInlineCreateAttributes
                    {
                        StartDate = new DateOnly(2025, 1, 15),
                        PreserveCurrentPrice = true,
                        PlanType = SubscriptionPlanType.Monthly
                    },
                    Relationships = new SubscriptionPriceInlineCreateRelationships
                    {
                        Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        SubscriptionPricePoint = RelationshipDeclaration.To(
                            "subscriptionPricePoints",
                            UsaSubscriptionPricePoint)
                    }
                },
                new SubscriptionIntroductoryOfferInlineCreate
                {
                    Id = "${introductory}",
                    Attributes = new SubscriptionIntroductoryOfferInlineCreateAttributes
                    {
                        StartDate = new DateOnly(2025, 2, 1),
                        EndDate = new DateOnly(2025, 3, 1),
                        Duration = SubscriptionOfferDuration.ThreeDays,
                        OfferMode = SubscriptionOfferMode.FreeTrial,
                        NumberOfPeriods = 1
                    },
                    Relationships = new SubscriptionIntroductoryOfferInlineCreateRelationships
                    {
                        Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        SubscriptionPricePoint = RelationshipDeclaration.To(
                            "subscriptionPricePoints",
                            UsaSubscriptionPricePoint)
                    }
                },
                new SubscriptionPromotionalOfferInlineCreate
                {
                    Id = "${promotional}",
                    Attributes = new SubscriptionPromotionalOfferInlineCreateAttributes
                    {
                        Duration = SubscriptionOfferDuration.TwoWeeks,
                        Name = "Welcome back",
                        NumberOfPeriods = 3,
                        OfferCode = "WELCOMEBACK",
                        OfferMode = SubscriptionOfferMode.PayUpFront
                    },
                    Relationships = new SubscriptionPromotionalOfferInlineCreateRelationships
                    {
                        Subscription = RelationshipDeclaration.To("subscriptions", "6446075067")
                    }
                }
            ]
        };

        await client.UpdateSubscriptionAsync("6446075067", request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedSubscriptionUpdateBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsSubscriptionOfferCodeIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(SubscriptionOfferCodeResponse, HttpStatusCode.Created);

        var request = new SubscriptionOfferCodeCreateRequest
        {
            Data = new SubscriptionOfferCodeCreateRequestData
            {
                Attributes = new SubscriptionOfferCodeCreateRequestDataAttributes
                {
                    Name = "Three Months On Us",
                    CustomerEligibilities =
                    [
                        SubscriptionCustomerEligibility.New,
                        SubscriptionCustomerEligibility.Expired
                    ],
                    OfferEligibility = SubscriptionOfferEligibility.StackWithIntroOffers,
                    Duration = SubscriptionOfferDuration.ThreeMonths,
                    OfferMode = SubscriptionOfferMode.FreeTrial,
                    NumberOfPeriods = 1
                },
                Relationships = new SubscriptionOfferCodeCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    Prices = RelationshipDeclarationList.To("subscriptionOfferCodePrices", "${price}")
                }
            },
            Included =
            [
                new SubscriptionOfferCodePriceInlineCreate
                {
                    Id = "${price}",
                    Relationships = new SubscriptionOfferCodePriceInlineCreateRelationships
                    {
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        SubscriptionPricePoint = RelationshipDeclaration.To(
                            "subscriptionPricePoints",
                            UsaSubscriptionPricePoint)
                    }
                }
            ]
        };

        await client.CreateSubscriptionOfferCodeAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedSubscriptionOfferCodeBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsInAppPurchaseOfferCodeIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(
            InAppPurchaseOfferCodeResponse,
            HttpStatusCode.Created);

        var request = new InAppPurchaseOfferCodeCreateRequest
        {
            Data = new InAppPurchaseOfferCodeCreateRequestData
            {
                Attributes = new InAppPurchaseOfferCodeCreateRequestDataAttributes
                {
                    Name = "Launch Week Coins",
                    CustomerEligibilities =
                    [
                        InAppPurchaseOfferCodeCustomerEligibility.NonSpender,
                        InAppPurchaseOfferCodeCustomerEligibility.ChurnedSpender
                    ]
                },
                Relationships = new InAppPurchaseOfferCodeCreateRequestDataRelationships
                {
                    InAppPurchase = RelationshipDeclaration.To("inAppPurchases", "6446819279"),
                    Prices = RelationshipDeclarationList.To("inAppPurchaseOfferPrices", "${price}")
                }
            },
            Included =
            [
                new InAppPurchaseOfferPriceInlineCreate
                {
                    Id = "${price}",
                    Relationships = new InAppPurchaseOfferPriceInlineCreateRelationships
                    {
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        PricePoint = RelationshipDeclaration.To("inAppPurchasePricePoints", UsaPricePoint)
                    }
                }
            ]
        };

        await client.CreateInAppPurchaseOfferCodeAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedInAppPurchaseOfferCodeBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsIntroductoryOfferIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(IntroductoryOfferResponse, HttpStatusCode.Created);

        var request = new SubscriptionIntroductoryOfferCreateRequest
        {
            Data = new SubscriptionIntroductoryOfferCreateRequestData
            {
                Attributes = new SubscriptionIntroductoryOfferCreateRequestDataAttributes
                {
                    Duration = SubscriptionOfferDuration.ThreeMonths,
                    OfferMode = SubscriptionOfferMode.PayUpFront,
                    NumberOfPeriods = 1
                },
                Relationships = new SubscriptionIntroductoryOfferCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    Territory = RelationshipDeclaration.To("territories", "USA"),
                    SubscriptionPricePoint = RelationshipDeclaration.To(
                        "subscriptionPricePoints",
                        UsaSubscriptionPricePoint)
                }
            },
            Included =
            [
                new SubscriptionPricePointInlineCreate { Id = UsaSubscriptionPricePoint }
            ]
        };

        await client.CreateSubscriptionIntroductoryOfferAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedIntroductoryOfferBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsPromotionalOfferCreateIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(PromotionalOfferResponse, HttpStatusCode.Created);

        var request = new SubscriptionPromotionalOfferCreateRequest
        {
            Data = new SubscriptionPromotionalOfferCreateRequestData
            {
                Attributes = new SubscriptionPromotionalOfferCreateRequestDataAttributes
                {
                    Duration = SubscriptionOfferDuration.OneMonth,
                    Name = "Welcome back",
                    NumberOfPeriods = 3,
                    OfferCode = "WELCOMEBACK",
                    OfferMode = SubscriptionOfferMode.PayAsYouGo
                },
                Relationships = new SubscriptionPromotionalOfferCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    Prices = RelationshipDeclarationList.To("subscriptionPromotionalOfferPrices", "${price}")
                }
            },
            Included =
            [
                new SubscriptionPromotionalOfferPriceInlineCreate
                {
                    Id = "${price}",
                    Relationships = new SubscriptionPromotionalOfferPriceInlineCreateRelationships
                    {
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        SubscriptionPricePoint = RelationshipDeclaration.To(
                            "subscriptionPricePoints",
                            UsaSubscriptionPricePoint)
                    }
                }
            ]
        };

        await client.CreateSubscriptionPromotionalOfferAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedPromotionalOfferCreateBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsPromotionalOfferUpdateIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(PromotionalOfferResponse);

        var request = new SubscriptionPromotionalOfferUpdateRequest
        {
            Data = new SubscriptionPromotionalOfferUpdateRequestData
            {
                Id = "c9d8e7f6-0001",
                Relationships = new SubscriptionPromotionalOfferUpdateRequestDataRelationships
                {
                    Prices = RelationshipDeclarationList.To("subscriptionPromotionalOfferPrices", "${price}")
                }
            },
            Included =
            [
                new SubscriptionPromotionalOfferPriceInlineCreate
                {
                    Id = "${price}",
                    Relationships = new SubscriptionPromotionalOfferPriceInlineCreateRelationships
                    {
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        SubscriptionPricePoint = RelationshipDeclaration.To(
                            "subscriptionPricePoints",
                            UsaSubscriptionPricePoint)
                    }
                }
            ]
        };

        await client.UpdateSubscriptionPromotionalOfferAsync(
            "c9d8e7f6-0001",
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedPromotionalOfferUpdateBody, handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsWinBackOfferIncludedArray()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(WinBackOfferResponse, HttpStatusCode.Created);

        var request = new WinBackOfferCreateRequest
        {
            Data = new WinBackOfferCreateRequestData
            {
                Attributes = new WinBackOfferCreateRequestDataAttributes
                {
                    ReferenceName = "Lapsed annual win-back",
                    OfferId = "winback_annual_2025",
                    Duration = SubscriptionOfferDuration.OneMonth,
                    OfferMode = SubscriptionOfferMode.PayAsYouGo,
                    PeriodCount = 3,
                    CustomerEligibilityPaidSubscriptionDurationInMonths = 6,
                    CustomerEligibilityTimeSinceLastSubscribedInMonths = new IntegerRange
                    {
                        Minimum = 2,
                        Maximum = 12
                    },
                    StartDate = new DateOnly(2025, 2, 1),
                    Priority = WinBackOfferPriority.High
                },
                Relationships = new WinBackOfferCreateRequestDataRelationships
                {
                    Subscription = RelationshipDeclaration.To("subscriptions", "6446075067"),
                    Prices = RelationshipDeclarationList.To("winBackOfferPrices", "${price}")
                }
            },
            Included =
            [
                new WinBackOfferPriceInlineCreate { Id = "${price}" }
            ]
        };

        await client.CreateWinBackOfferAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(ExpectedWinBackOfferBody, handler.CapturedRequestBody);
    }

    /// <summary>
    /// Builds the price schedule request whose body Apple already accepted.
    /// </summary>
    private static InAppPurchasePriceScheduleCreateRequest BuildProvenPriceScheduleRequest()
    {
        return new InAppPurchasePriceScheduleCreateRequest
        {
            Data = new InAppPurchasePriceScheduleCreateRequestData
            {
                Relationships = new InAppPurchasePriceScheduleCreateRequestDataRelationships
                {
                    InAppPurchase = RelationshipDeclaration.To("inAppPurchases", "6446819279"),
                    BaseTerritory = RelationshipDeclaration.To("territories", "USA"),
                    ManualPrices = RelationshipDeclarationList.To("inAppPurchasePrices", "${price}")
                }
            },
            Included =
            [
                new InAppPurchasePriceInlineCreate
                {
                    Id = "${price}",
                    Relationships = new InAppPurchasePriceInlineCreateRelationships
                    {
                        InAppPurchasePricePoint = RelationshipDeclaration.To(
                            "inAppPurchasePricePoints",
                            UsaPricePoint)
                    }
                }
            ]
        };
    }

    /// <summary>
    /// An entry of a resource type Apple's schema does not allow in a price schedule's
    /// <c>included</c> array. The interface cannot stop a caller outside this library from writing
    /// one, so the serializer is what refuses it.
    /// </summary>
    private sealed class UnknownIncludedResource : IInAppPurchasePriceScheduleCreateRequestIncludedResource
    {
        [JsonPropertyName("type")]
        // ReSharper disable once UnusedMember.Local
        public string Type { get; set; } = "inAppPurchaseAvailabilities";
    }
}
