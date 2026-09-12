using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class OfferCodeTests
{
    [Fact]
    public async Task DecodesOfferCodeListAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.offercodeInAppPurchaseOfferCodes.json");

        var response = await client.ListOfferCodesForInAppPurchaseAsync(
            "6446819279",
            new AppStoreConnectQuery().Limit(2),
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/offerCodes?limit=2",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var launchWeek = response.Data[0];

        Assert.Equal("inAppPurchaseOfferCodes", launchWeek.Type);
        Assert.Equal("1f6d0c2a-9b45-4f9e-8f1c-2a7d5b3e9c11", launchWeek.Id);
        Assert.Equal("Launch Week Coins", launchWeek.Attributes!.Name);
        Assert.Equal(2500, launchWeek.Attributes.ProductionCodeCount);
        Assert.Equal(10, launchWeek.Attributes.SandboxCodeCount);
        Assert.True(launchWeek.Attributes.Active);
        Assert.Equal(
            new[]
            {
                InAppPurchaseOfferCodeCustomerEligibility.NonSpender,
                InAppPurchaseOfferCodeCustomerEligibility.ChurnedSpender
            },
            launchWeek.Attributes.CustomerEligibilities);

        var prices = launchWeek.Relationships!["prices"];
        var linkedPrices = prices.ToMany();

        Assert.Equal(2, linkedPrices.Length);
        Assert.Equal("inAppPurchaseOfferPrices", linkedPrices[0].Type);
        Assert.Equal("eyJzIjoiNjQ0NjgxOTI3OSIsInQiOiJVU0EifQ", linkedPrices[0].Id);
        Assert.Equal("eyJzIjoiNjQ0NjgxOTI3OSIsInQiOiJKUE4ifQ", linkedPrices[1].Id);
        Assert.Equal(2, prices.Meta!.Paging.Total);

        Assert.Empty(launchWeek.Relationships["oneTimeUseCodes"].ToMany());

        Assert.False(response.Data[1].Attributes!.Active);

        Assert.Equal(5, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/offerCodes?cursor=AQ.PXlqU3A&limit=2",
            response.Links.Next);
    }

    [Fact]
    public async Task DecodesSubscriptionOfferCodeAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.offercodeSubscriptionOfferCode.json");

        var response = await client.GetSubscriptionOfferCodeAsync(
            "7c2b1d5e-4a83-4f61-9e0b-5d8a2c6f7b90",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionOfferCodes/7c2b1d5e-4a83-4f61-9e0b-5d8a2c6f7b90",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        var offerCode = response.Data;

        Assert.Equal("subscriptionOfferCodes", offerCode.Type);
        Assert.Equal("7c2b1d5e-4a83-4f61-9e0b-5d8a2c6f7b90", offerCode.Id);
        Assert.Equal("Three Months On Us", offerCode.Attributes!.Name);
        Assert.Equal(SubscriptionOfferDuration.ThreeMonths, offerCode.Attributes.Duration);
        Assert.Equal(SubscriptionOfferMode.FreeTrial, offerCode.Attributes.OfferMode);
        Assert.Equal(
            SubscriptionOfferEligibility.StackWithIntroOffers,
            offerCode.Attributes.OfferEligibility);
        Assert.Equal(SubscriptionPlanType.Monthly, offerCode.Attributes.TargetSubscriptionPlanType);
        Assert.Equal(1, offerCode.Attributes.NumberOfPeriods);
        Assert.Equal(5000, offerCode.Attributes.TotalNumberOfCodes);
        Assert.Equal(4800, offerCode.Attributes.ProductionCodeCount);
        Assert.Equal(200, offerCode.Attributes.SandboxCodeCount);
        Assert.True(offerCode.Attributes.Active);
        Assert.True(offerCode.Attributes.AutoRenewEnabled);
        Assert.Equal(
            new[]
            {
                SubscriptionCustomerEligibility.New,
                SubscriptionCustomerEligibility.Expired
            },
            offerCode.Attributes.CustomerEligibilities);

        var subscription = offerCode.Relationships!["subscription"].ToOne()!;

        Assert.Equal("subscriptions", subscription.Type);
        Assert.Equal("6446900112", subscription.Id);

        var customCodes = offerCode.Relationships["customCodes"].ToMany();

        Assert.Equal(2, customCodes.Length);
        Assert.Equal("subscriptionOfferCodeCustomCodes", customCodes[0].Type);
        Assert.Equal("9d4e8f10-6b27-4c33-a5e9-1f0b7c2d8a44", customCodes[0].Id);
    }

    [Fact]
    public async Task DecodesUnknownOfferCodeEnvironmentAsUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.offercodeOneTimeUseCodes.json");

        var response = await client.ListOneTimeUseCodesForInAppPurchaseOfferCodeAsync(
            "1f6d0c2a-9b45-4f9e-8f1c-2a7d5b3e9c11",
            cancellationToken: TestContext.Current.CancellationToken);

        var production = response.Data[0];

        Assert.Equal(OfferCodeEnvironment.Production, production.Attributes!.Environment);
        Assert.Equal(2500, production.Attributes.NumberOfCodes);
        Assert.Equal(
            new DateTimeOffset(2026, 2, 14, 9, 31, 20, TimeSpan.FromHours(-8)),
            production.Attributes.CreatedDate);
        Assert.Equal(new DateOnly(2026, 6, 30), production.Attributes.ExpirationDate);
        Assert.True(production.Attributes.Active);

        var unknownEnvironment = response.Data[1];

        Assert.Equal(OfferCodeEnvironment._Unmapped, unknownEnvironment.Attributes!.Environment);

        Assert.Equal("8f3d2c61-7b04-4e19-a3c5-6d1b9e4f8222", unknownEnvironment.Id);
        Assert.Equal(10, unknownEnvironment.Attributes.NumberOfCodes);
    }

    [Fact]
    public async Task SendsCommaJoinedFilterValues()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.offercodePrices.json");

        var response = await client.ListPricesForInAppPurchaseOfferCodeAsync(
            "1f6d0c2a-9b45-4f9e-8f1c-2a7d5b3e9c11",
            new AppStoreConnectQuery().Filter("territory", "USA", "JPN"),
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/inAppPurchaseOfferCodes/1f6d0c2a-9b45-4f9e-8f1c-2a7d5b3e9c11/prices?filter[territory]=USA,JPN",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var territory = response.Data[0].Relationships!["territory"].ToOne()!;

        Assert.Equal("territories", territory.Type);
        Assert.Equal("USA", territory.Id);

        var pricePoint = response.Data[0].Relationships!["pricePoint"].ToOne()!;

        Assert.Equal("inAppPurchasePricePoints", pricePoint.Type);
        Assert.Equal("eyJzIjoiNjQ0NjgxOTI3OSIsInQiOiJVU0EiLCJwIjoiMTAwMDAifQ", pricePoint.Id);

        Assert.Equal("JPN", response.Data[1].Relationships!["territory"].ToOne()!.Id);
    }

    [Fact]
    public async Task SendsCreateInAppPurchaseOfferCodeRequestBody()
    {
        var (client, handler) =
            TestUtilities.GetClientWithJson("models.offercodeInAppPurchaseOfferCodeCreated.json");

        var request = new InAppPurchaseOfferCodeCreateRequest
        {
            Data = new InAppPurchaseOfferCodeCreateRequestData
            {
                Attributes = new InAppPurchaseOfferCodeCreateRequestDataAttributes
                {
                    Name = "Holiday Coins",
                    CustomerEligibilities =
                    [
                        InAppPurchaseOfferCodeCustomerEligibility.NonSpender
                    ]
                },
                Relationships = new InAppPurchaseOfferCodeCreateRequestDataRelationships
                {
                    InAppPurchase = RelationshipDeclaration.To("inAppPurchases", "6446819279"),
                    Prices = RelationshipDeclarationList.To("inAppPurchaseOfferPrices", "${price-usa}")
                }
            },
            Included =
            [
                new InAppPurchaseOfferPriceInlineCreate
                {
                    Id = "${price-usa}",
                    Relationships = new InAppPurchaseOfferPriceInlineCreateRelationships
                    {
                        Territory = RelationshipDeclaration.To("territories", "USA"),
                        PricePoint = RelationshipDeclaration.To(
                            "inAppPurchasePricePoints",
                            "eyJzIjoiNjQ0NjgxOTI3OSIsInQiOiJVU0EiLCJwIjoiMTAwMDAifQ")
                    }
                }
            ]
        };

        var response = await client.CreateInAppPurchaseOfferCodeAsync(
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/inAppPurchaseOfferCodes",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("inAppPurchaseOfferCodes", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");

        Assert.Equal("Holiday Coins", attributes.GetProperty("name").GetString());

        var eligibilities = attributes.GetProperty("customerEligibilities");

        Assert.Equal(1, eligibilities.GetArrayLength());
        Assert.Equal("NON_SPENDER", eligibilities[0].GetString());

        var relationships = data.GetProperty("relationships");
        var inAppPurchase = relationships.GetProperty("inAppPurchase").GetProperty("data");

        Assert.Equal("inAppPurchases", inAppPurchase.GetProperty("type").GetString());
        Assert.Equal("6446819279", inAppPurchase.GetProperty("id").GetString());

        var prices = relationships.GetProperty("prices").GetProperty("data");

        Assert.Equal(1, prices.GetArrayLength());
        Assert.Equal("inAppPurchaseOfferPrices", prices[0].GetProperty("type").GetString());
        Assert.Equal("${price-usa}", prices[0].GetProperty("id").GetString());

        var included = body.RootElement.GetProperty("included");

        Assert.Equal(1, included.GetArrayLength());
        Assert.Equal("inAppPurchaseOfferPrices", included[0].GetProperty("type").GetString());
        Assert.Equal("${price-usa}", included[0].GetProperty("id").GetString());

        var includedRelationships = included[0].GetProperty("relationships");

        Assert.Equal(
            "USA",
            includedRelationships.GetProperty("territory").GetProperty("data").GetProperty("id").GetString());
        Assert.Equal(
            "inAppPurchasePricePoints",
            includedRelationships.GetProperty("pricePoint").GetProperty("data").GetProperty("type").GetString());

        Assert.Equal("c5a91b38-4d72-4e08-b6f1-9a3c2e7d4055", response.Data.Id);
        Assert.True(response.Data.Attributes!.Active);
    }

    [Fact]
    public async Task ReturnsOneTimeUseCodeValuesVerbatim()
    {
        const string csv = "Code,Expiration Date\r\nX3F7KJ9PLQ,2026-06-30\r\nM8T2WD5RBN,2026-06-30\r\n";

        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Encoding.UTF8.GetBytes(csv), "text/csv");

        using var client = TestUtilities.CreateClient(handler);

        var values = await client.GetValuesForInAppPurchaseOfferCodeOneTimeUseCodeAsync(
            "4b1e7a90-3c58-4d22-8f6b-2e9a0c7d5111",
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/inAppPurchaseOfferCodeOneTimeUseCodes/4b1e7a90-3c58-4d22-8f6b-2e9a0c7d5111/values",
            handler.CapturedRequest!.RequestUri!.OriginalString);
        Assert.Equal("text/csv", handler.CapturedRequest.Headers.Accept.ToString());
        Assert.Equal(csv, values);
    }
}
