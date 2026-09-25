using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class InAppPurchaseTests
{
    [Fact]
    public async Task DecodesInAppPurchaseAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapInAppPurchase.json");

        var response = await client.GetInAppPurchaseAsync(
            "6446819279",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279",
            handler.CapturedRequest!.RequestUri!.OriginalString);
        Assert.Equal(HttpMethod.Get, handler.CapturedRequest.Method);

        var purchase = response.Data;

        Assert.Equal("inAppPurchases", purchase.Type);
        Assert.Equal("6446819279", purchase.Id);
        Assert.Equal("Pro Unlock", purchase.Attributes!.Name);
        Assert.Equal("com.example.app.pro_unlock", purchase.Attributes.ProductId);
        Assert.Equal(InAppPurchaseType.NonConsumable, purchase.Attributes.InAppPurchaseType);
        Assert.Equal(InAppPurchaseState.ReadyToSubmit, purchase.Attributes.State);
        Assert.Equal(
            "Unlocks every editing tool. No account is required to test it.",
            purchase.Attributes.ReviewNote);
        Assert.True(purchase.Attributes.FamilySharable);
        Assert.False(purchase.Attributes.ContentHosting);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279",
            purchase.Links!.Self);
    }

    [Fact]
    public async Task DecodesInAppPurchaseRelationships()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.iapInAppPurchase.json");

        var response = await client.GetInAppPurchaseAsync(
            "6446819279",
            cancellationToken: TestContext.Current.CancellationToken);

        var relationships = response.Data.Relationships!;

        var priceSchedule = relationships["iapPriceSchedule"].ToOne()!;

        Assert.Equal("inAppPurchasePriceSchedules", priceSchedule.Type);
        Assert.Equal("6446819279", priceSchedule.Id);

        var localizations = relationships["inAppPurchaseLocalizations"];
        var linkedLocalizations = localizations.ToMany();

        Assert.Equal(2, linkedLocalizations.Length);
        Assert.Equal("inAppPurchaseLocalizations", linkedLocalizations[0].Type);
        Assert.Equal("e2c1ef7b-3e04-4a1f-9d47-3f09b8f5d0aa", linkedLocalizations[0].Id);
        Assert.Equal("5a9f3b21-8d6c-4f0e-9a12-77c1e4b6f331", linkedLocalizations[1].Id);
        Assert.Equal(2, localizations.Meta!.Paging.Total);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/inAppPurchaseLocalizations",
            localizations.Links!.Related);

        Assert.Null(relationships["inAppPurchaseAvailability"].ToOne());
    }

#pragma warning disable CS0618 // Apple deprecated the v1 localization endpoints in 4.4.1; the library still supports them.
    [Fact]
    public async Task DecodesLocalizationListWithPagingMeta()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapInAppPurchaseLocalizations.json");

        var response = await client.ListLocalizationsForInAppPurchaseAsync(
            "6446819279",
            new AppStoreConnectQuery().Limit(2),
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/inAppPurchaseLocalizations?limit=2",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var english = response.Data[0];

        Assert.Equal("inAppPurchaseLocalizations", english.Type);
        Assert.Equal("e2c1ef7b-3e04-4a1f-9d47-3f09b8f5d0aa", english.Id);
        Assert.Equal("Pro Unlock", english.Attributes!.Name);
        Assert.Equal("en-US", english.Attributes.Locale);
        Assert.Equal("Unlock every editing tool, forever.", english.Attributes.Description);
        Assert.Equal(InAppPurchaseLocalizationState.Approved, english.Attributes.State);

        var owner = english.Relationships!["inAppPurchaseV2"].ToOne()!;

        Assert.Equal("inAppPurchases", owner.Type);
        Assert.Equal("6446819279", owner.Id);

        Assert.Equal("de-DE", response.Data[1].Attributes!.Locale);
        Assert.Equal(
            InAppPurchaseLocalizationState.PrepareForSubmission,
            response.Data[1].Attributes!.State);

        Assert.Equal(3, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/inAppPurchaseLocalizations?cursor=BQ.CDGZ7Bw&limit=2",
            response.Links.Next);
    }
#pragma warning restore CS0618

    [Fact]
    public async Task DecodesPricePointListWithTerritoryLinkage()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapInAppPurchasePricePoints.json");

        var response = await client.ListPricePointsForInAppPurchaseAsync(
            "6446819279",
            new AppStoreConnectQuery().Limit(2),
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/pricePoints?limit=2",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var unitedStates = response.Data[0];

        Assert.Equal("inAppPurchasePricePoints", unitedStates.Type);
        Assert.Equal("eyJzIjoiNjQ0NjgxOTI3OSIsInAiOiIxMDAwNyIsInQiOiJVU0EifQ", unitedStates.Id);
        Assert.Equal("4.99", unitedStates.Attributes!.CustomerPrice);
        Assert.Equal("3.49", unitedStates.Attributes.Proceeds);

        var territory = unitedStates.Relationships!["territory"].ToOne()!;

        Assert.Equal("territories", territory.Type);
        Assert.Equal("USA", territory.Id);

        var canada = response.Data[1];

        Assert.Equal("6.99", canada.Attributes!.CustomerPrice);
        Assert.Equal("4.89", canada.Attributes.Proceeds);
        Assert.Equal("CAN", canada.Relationships!["territory"].ToOne()!.Id);

        Assert.Equal(175, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
        Assert.Equal("BQ.MglVUw", response.Meta.Paging.NextCursor);
    }

    [Fact]
    public async Task DecodesUnknownInAppPurchaseEnumValuesAsUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.iapInAppPurchaseUnknownEnums.json");

        var response = await client.GetInAppPurchaseAsync(
            "6446819280",
            cancellationToken: TestContext.Current.CancellationToken);

        var attributes = response.Data.Attributes!;

        Assert.Equal(InAppPurchaseType._Unmapped, attributes.InAppPurchaseType);
        Assert.Equal(InAppPurchaseState._Unmapped, attributes.State);

        Assert.Equal("Season Pass", attributes.Name);
        Assert.Equal("com.example.app.season_pass", attributes.ProductId);
        Assert.Null(attributes.ReviewNote);
    }

    [Fact]
    public async Task SendsIncludeAndRelationshipLimitQueryParameters()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapInAppPurchase.json");

        var query = new AppStoreConnectQuery()
            .Include("inAppPurchaseLocalizations")
            .Limit("inAppPurchaseLocalizations", 50)
            .Fields("inAppPurchases", "name", "productId", "state");

        await client.GetInAppPurchaseAsync(
            "6446819279",
            query,
            TestContext.Current.CancellationToken);

        var url = handler.CapturedRequest!.RequestUri!.OriginalString;

        Assert.StartsWith("https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279?", url);
        Assert.Contains("include=inAppPurchaseLocalizations", url);
        Assert.Contains("limit[inAppPurchaseLocalizations]=50", url);
        Assert.Contains("fields[inAppPurchases]=name,productId,state", url);
    }

    [Fact]
    public async Task SendsCreateInAppPurchaseRequestBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapInAppPurchaseCreated.json");

        var request = new InAppPurchaseV2CreateRequest
        {
            Data = new InAppPurchaseV2CreateRequestData
            {
                Attributes = new InAppPurchaseV2CreateRequestDataAttributes
                {
                    Name = "Coin Pack",
                    ProductId = "com.example.app.coin_pack",
                    InAppPurchaseType = InAppPurchaseType.Consumable,
                    ReviewNote = "Grants 500 coins.",
                    FamilySharable = false
                },
                Relationships = new InAppPurchaseV2CreateRequestDataRelationships
                {
                    App = RelationshipDeclaration.To("apps", "6446800000")
                }
            }
        };

        var response = await client.CreateInAppPurchaseAsync(
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchases",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("inAppPurchases", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");

        Assert.Equal("Coin Pack", attributes.GetProperty("name").GetString());
        Assert.Equal("com.example.app.coin_pack", attributes.GetProperty("productId").GetString());
        Assert.Equal("CONSUMABLE", attributes.GetProperty("inAppPurchaseType").GetString());
        Assert.Equal("Grants 500 coins.", attributes.GetProperty("reviewNote").GetString());
        Assert.False(attributes.GetProperty("familySharable").GetBoolean());

        var app = data.GetProperty("relationships").GetProperty("app").GetProperty("data");

        Assert.Equal("apps", app.GetProperty("type").GetString());
        Assert.Equal("6446800000", app.GetProperty("id").GetString());

        Assert.False(data.TryGetProperty("id", out _));

        Assert.Equal("6449128843", response.Data.Id);
        Assert.Equal(InAppPurchaseState.MissingMetadata, response.Data.Attributes!.State);
    }
}
