using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class SubscriptionTests
{
    [Fact]
    public async Task DecodesSubscriptionAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionResponse.json");

        var response = await client.GetSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067",
            handler.CapturedRequest.RequestUri!.ToString());

        var subscription = response.Data;
        Assert.Equal("subscriptions", subscription.Type);
        Assert.Equal("6446075067", subscription.Id);
        Assert.Equal("Pro Monthly", subscription.Attributes!.Name);
        Assert.Equal("com.example.pro.monthly", subscription.Attributes.ProductId);
        Assert.True(subscription.Attributes.FamilySharable);
        Assert.Equal(SubscriptionState.Approved, subscription.Attributes.State);
        Assert.Equal(SubscriptionPeriod.OneMonth, subscription.Attributes.SubscriptionPeriod);
        Assert.Equal("Sign in with the demo account to see the subscription.", subscription.Attributes.ReviewNote);
        Assert.Equal(2, subscription.Attributes.GroupLevel);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067",
            subscription.Links!.Self);
    }

    [Fact]
    public async Task ReadsToOneAndToManyRelationshipsOfSubscription()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.subscriptionResponse.json");

        var response = await client.GetSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        var relationships = response.Data.Relationships!;

        var group = relationships["group"].ToOne();
        Assert.Equal("subscriptionGroups", group!.Type);
        Assert.Equal("20925550", group.Id);
        Assert.Empty(relationships["group"].ToMany());

        var prices = relationships["prices"].ToMany();
        Assert.Equal(2, prices.Length);
        Assert.Equal("subscriptionPrices", prices[0].Type);
        Assert.Equal("eyJzIjoiNjQ0NjA3NTA2NyIsInQiOiJVU0EifQ", prices[0].Id);
        Assert.Equal("eyJzIjoiNjQ0NjA3NTA2NyIsInQiOiJDQU4ifQ", prices[1].Id);
        Assert.Null(relationships["prices"].ToOne());
        Assert.Equal(2, relationships["prices"].Meta!.Paging.Total);
    }

    [Fact]
    public async Task SendsSubscriptionCreateUrlAndBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.subscriptionCreateResponse.json",
            HttpStatusCode.Created);

        var request = new SubscriptionCreateRequest
        {
            Data = new SubscriptionCreateRequestData
            {
                Attributes = new SubscriptionCreateRequestDataAttributes
                {
                    Name = "Pro Yearly",
                    ProductId = "com.example.pro.yearly",
                    FamilySharable = false,
                    SubscriptionPeriod = SubscriptionPeriod.OneYear,
                    GroupLevel = 1
                },
                Relationships = new SubscriptionCreateRequestDataRelationships
                {
                    Group = RelationshipDeclaration.To("subscriptionGroups", "20925550")
                }
            }
        };

        var response = await client.CreateSubscriptionAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions",
            handler.CapturedRequest.RequestUri!.ToString());
        Assert.Equal("application/json", handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptions", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.Equal("Pro Yearly", attributes.GetProperty("name").GetString());
        Assert.Equal("com.example.pro.yearly", attributes.GetProperty("productId").GetString());
        Assert.False(attributes.GetProperty("familySharable").GetBoolean());
        Assert.Equal("ONE_YEAR", attributes.GetProperty("subscriptionPeriod").GetString());
        Assert.Equal(1, attributes.GetProperty("groupLevel").GetInt32());

        Assert.False(attributes.TryGetProperty("reviewNote", out _));

        var group = data.GetProperty("relationships").GetProperty("group").GetProperty("data");
        Assert.Equal("subscriptionGroups", group.GetProperty("type").GetString());
        Assert.Equal("20925550", group.GetProperty("id").GetString());

        Assert.Equal("6446075099", response.Data.Id);
        Assert.Equal(SubscriptionState.MissingMetadata, response.Data.Attributes!.State);
    }

    [Fact]
    public async Task SendsSubscriptionIdInPatchBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionResponse.json");

        var request = new SubscriptionUpdateRequest
        {
            Data = new SubscriptionUpdateRequestData
            {
                Id = "6446075067",
                Attributes = new SubscriptionUpdateRequestDataAttributes
                {
                    Name = "Pro Monthly",
                    GroupLevel = 2
                }
            }
        };

        await client.UpdateSubscriptionAsync("6446075067", request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067",
            handler.CapturedRequest.RequestUri!.ToString());

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptions", data.GetProperty("type").GetString());
        Assert.Equal("6446075067", data.GetProperty("id").GetString());
        Assert.Equal("Pro Monthly", data.GetProperty("attributes").GetProperty("name").GetString());
        Assert.False(body.RootElement.TryGetProperty("included", out _));
    }

    [Fact]
    public async Task DeletesSubscriptionWithoutBodyOrResponse()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteSubscriptionAsync("6446075067", TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067",
            handler.CapturedRequest.RequestUri!.ToString());
        Assert.Null(handler.CapturedRequestBody);
    }

    [Fact]
    public async Task SendsBracketedQueryParametersAndCommaJoinedValues()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionResponse.json");

        var query = new AppStoreConnectQuery()
            .Fields("subscriptions", "name", "productId")
            .Include("group", "prices")
            .Limit("prices", 5);

        await client.GetSubscriptionAsync("6446075067", query, TestContext.Current.CancellationToken);

        var url = Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.ToString());
        Assert.StartsWith("https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067?", url);
        Assert.Contains("fields[subscriptions]=name,productId", url);
        Assert.Contains("include=group,prices", url);
        Assert.Contains("limit[prices]=5", url);
    }

    [Fact]
    public async Task DecodesSubscriptionGroupAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionGroupResponse.json");

        var response = await client.GetSubscriptionGroupAsync(
            "20925550",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroups/20925550",
            handler.CapturedRequest!.RequestUri!.ToString());

        var group = response.Data;
        Assert.Equal("subscriptionGroups", group.Type);
        Assert.Equal("20925550", group.Id);
        Assert.Equal("Example Pro Access", group.Attributes!.ReferenceName);

        var subscriptions = group.Relationships!["subscriptions"].ToMany();
        Assert.Equal(2, subscriptions.Length);
        Assert.Equal("6446075067", subscriptions[0].Id);
        Assert.Equal("6446075099", subscriptions[1].Id);
    }

#pragma warning disable CS0618 // Apple deprecated the v1 localization endpoints in 4.4.1; the library still supports them.
    [Fact]
    public async Task DecodesSubscriptionGroupLocalizationsPage()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionGroupLocalizationsResponse.json");

        var response = await client.ListLocalizationsForSubscriptionGroupAsync(
            "20925550",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroups/20925550/subscriptionGroupLocalizations",
            handler.CapturedRequest!.RequestUri!.ToString());

        var localizations = response.Data;
        Assert.Equal(2, localizations.Length);
        var english = localizations[0].Attributes!;
        Assert.Equal("Example Pro", english.Name);
        Assert.Equal("Example", english.CustomAppName);
        Assert.Equal("en-US", english.Locale);
        Assert.Equal(SubscriptionGroupLocalizationState.Approved, english.State);

        var spanish = localizations[1].Attributes!;
        Assert.Equal("Ejemplo Pro", spanish.Name);
        Assert.Null(spanish.CustomAppName);
        Assert.Equal(SubscriptionGroupLocalizationState.PrepareForSubmission, spanish.State);

        Assert.Equal(4, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroups/20925550/subscriptionGroupLocalizations?cursor=BQ.MglVUw",
            response.Links.Next);
    }
#pragma warning restore CS0618

#pragma warning disable CS0618 // Apple deprecated the v1 localization endpoints in 4.4.1; the library still supports them.
    [Fact]
    public async Task DecodesSubscriptionLocalizationAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionLocalizationResponse.json");

        var response = await client.GetSubscriptionLocalizationAsync(
            "9f8e7d6c-1111",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionLocalizations/9f8e7d6c-1111",
            handler.CapturedRequest!.RequestUri!.ToString());

        var localization = response.Data;
        Assert.Equal("subscriptionLocalizations", localization.Type);
        Assert.Equal("Pro Monthly", localization.Attributes!.Name);
        Assert.Equal("en-US", localization.Attributes.Locale);
        Assert.Equal("Unlimited access, billed every month.", localization.Attributes.Description);
        Assert.Equal(SubscriptionLocalizationState.WaitingForReview, localization.Attributes.State);
        Assert.Equal("6446075067", localization.Relationships!["subscription"].ToOne()!.Id);
    }
#pragma warning restore CS0618

    [Fact]
    public async Task DecodesOfferCodeCustomerEligibilities()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionOfferCodesResponse.json");

        var response = await client.ListOfferCodesForSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067/offerCodes",
            handler.CapturedRequest!.RequestUri!.ToString());

        var offerCodes = response.Data;
        Assert.Equal(2, offerCodes.Length);

        var winter = offerCodes[0].Attributes!;
        Assert.Equal("Winter campaign", winter.Name);
        Assert.Equal(
            new[] { SubscriptionCustomerEligibility.New, SubscriptionCustomerEligibility.Expired },
            winter.CustomerEligibilities);
        Assert.Equal(SubscriptionOfferEligibility.StackWithIntroOffers, winter.OfferEligibility);
        Assert.Equal(SubscriptionOfferDuration.ThreeMonths, winter.Duration);
        Assert.Equal(SubscriptionOfferMode.FreeTrial, winter.OfferMode);
        Assert.Equal(SubscriptionPlanType.Monthly, winter.TargetSubscriptionPlanType);
        Assert.Equal(25000, winter.TotalNumberOfCodes);
        Assert.True(winter.Active);
        Assert.False(winter.AutoRenewEnabled);
    }

    [Fact]
    public async Task FallsBackToUnmappedForUnknownEligibilityAndPlanType()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.subscriptionOfferCodesResponse.json");

        var response = await client.ListOfferCodesForSubscriptionAsync(
            "6446075067",
            cancellationToken: TestContext.Current.CancellationToken);

        var future = response.Data[1].Attributes!;

        Assert.Equal(
            new[] { SubscriptionCustomerEligibility.Existing, SubscriptionCustomerEligibility._Unmapped },
            future.CustomerEligibilities);
        Assert.Equal(SubscriptionPlanType._Unmapped, future.TargetSubscriptionPlanType);

        Assert.Equal(SubscriptionOfferMode.PayAsYouGo, future.OfferMode);
        Assert.Equal(SubscriptionOfferDuration.OneMonth, future.Duration);
    }

    [Fact]
    public async Task ThrowsApiExceptionWithErrorDetails()
    {
        var (client, _) = TestUtilities.GetClientWithJson(
            "models.subscriptionErrorResponse.json",
            HttpStatusCode.Conflict);

        var request = new SubscriptionCreateRequest
        {
            Data = new SubscriptionCreateRequestData
            {
                Attributes = new SubscriptionCreateRequestDataAttributes
                {
                    Name = "Pro Monthly",
                    ProductId = "com.example.pro.monthly"
                },
                Relationships = new SubscriptionCreateRequestDataRelationships
                {
                    Group = RelationshipDeclaration.To("subscriptionGroups", "20925550")
                }
            }
        };

        var exception = await Assert.ThrowsAsync<APIException>(
            () => client.CreateSubscriptionAsync(request, TestContext.Current.CancellationToken));

        Assert.Equal(409, exception.HttpStatusCode);
        Assert.Equal("The product ID com.example.pro.monthly is already in use.", exception.Message);

        Assert.Single(exception.Errors!);

        var error = exception.Errors![0];
        Assert.Equal("409", error.Status);
        Assert.Equal("STATE_ERROR.SUBSCRIPTION.PRODUCT_ID_ALREADY_EXISTS", error.Code);
        Assert.Equal("/data/attributes/productId", error.Source!.Pointer);
        Assert.Contains("PRODUCT_ID_ALREADY_EXISTS", exception.ResponseBody!);
    }
}
