using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class AppTests
{
    [Fact]
    public async Task DecodesAppListAttributes()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Length);

        var app = response.Data[0];
        Assert.Equal("apps", app.Type);
        Assert.Equal("6446939457", app.Id);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/apps/6446939457", app.Links?.Self);

        var attributes = app.Attributes;
        Assert.NotNull(attributes);
        Assert.Equal("Example Notes", attributes.Name);
        Assert.Equal("com.example.notes", attributes.BundleId);
        Assert.Equal("EXAMPLENOTES", attributes.Sku);
        Assert.Equal("en-US", attributes.PrimaryLocale);
        Assert.False(attributes.IsOrEverWasMadeForKids);
        Assert.Equal("https://example.com/notifications/v2", attributes.SubscriptionStatusUrl);
        Assert.Equal(SubscriptionStatusUrlVersion.V2, attributes.SubscriptionStatusUrlVersion);
        Assert.Equal("https://sandbox.example.com/notifications/v1", attributes.SubscriptionStatusUrlForSandbox);
        Assert.Equal(SubscriptionStatusUrlVersion.V1, attributes.SubscriptionStatusUrlVersionForSandbox);
        Assert.Equal(AppContentRightsDeclaration.UsesThirdPartyContent, attributes.ContentRightsDeclaration);
        Assert.True(attributes.StreamlinedPurchasingEnabled);
        Assert.Equal("https://example.com/accessibility", attributes.AccessibilityUrl);

        var second = response.Data[1];
        Assert.Equal("com.example.tasks", second.Attributes?.BundleId);
        Assert.Equal(
            AppContentRightsDeclaration.DoesNotUseThirdPartyContent,
            second.Attributes?.ContentRightsDeclaration);
        Assert.Null(second.Attributes?.SubscriptionStatusUrl);
    }

    [Fact]
    public async Task DecodesAppListPagingMetadata()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(5, response.Meta?.Paging.Total);
        Assert.Equal(2, response.Meta?.Paging.Limit);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/apps?limit=2", response.Links.First);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/apps?cursor=BQ.o3Q9NQ&limit=2", response.Links.Next);
    }

    [Fact]
    public async Task FindsIncludedResourceByTypeAndId()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(response.TryGetIncluded<BetaGroup>(
            "betaGroups",
            "0d5e9ee1-4c02-4b7a-9f3b-2b0c9a1d7e11",
            out var betaGroup));
        Assert.Equal("Internal QA", betaGroup.Attributes?.Name);
        Assert.True(betaGroup.Attributes?.IsInternalGroup);

        Assert.True(response.TryGetIncluded<PrereleaseVersion>(
            "preReleaseVersions",
            "9a1cf2d3-7f11-4f5b-8a4d-6c2f1e3b7d05",
            out var prereleaseVersion));
        Assert.Equal("1.4.0", prereleaseVersion.Attributes?.Version);
        Assert.Equal(Platform.Ios, prereleaseVersion.Attributes?.Platform);
    }

    [Fact]
    public async Task DoesNotFindIncludedResourceWithMismatchedTypeOrId()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.False(response.TryGetIncluded<BetaGroup>("betaGroups", "does-not-exist", out var missingId));
        Assert.Null(missingId);

        Assert.False(response.TryGetIncluded<BetaGroup>(
            "builds",
            "0d5e9ee1-4c02-4b7a-9f3b-2b0c9a1d7e11",
            out var missingType));
        Assert.Null(missingType);
    }

    [Fact]
    public async Task ReadsRelationshipLinkageFromApp()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var response = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var relationships = response.Data[0].Relationships;
        Assert.NotNull(relationships);

        var betaGroups = relationships["betaGroups"].ToMany();
        Assert.Single(betaGroups);
        Assert.Equal("betaGroups", betaGroups[0].Type);
        Assert.Equal("0d5e9ee1-4c02-4b7a-9f3b-2b0c9a1d7e11", betaGroups[0].Id);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/apps/6446939457/betaGroups",
            relationships["betaGroups"].Links?.Related);

        Assert.Null(relationships["betaGroups"].ToOne());
    }

    [Fact]
    public async Task SendsFieldsIncludeSortAndLimitQueryParameters()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var query = new AppStoreConnectQuery()
            .Fields("apps", "name", "bundleId")
            .Include("betaGroups", "preReleaseVersions")
            .Sort("name")
            .Limit(2)
            .Limit("betaGroups", 5)
            .Exists("gameCenterEnabledVersions", "true");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1/apps", request.RequestUri!.AbsolutePath);

        var queryString = DecodedQuery(handler);
        Assert.Contains("fields[apps]=name,bundleId", queryString);
        Assert.Contains("include=betaGroups,preReleaseVersions", queryString);
        Assert.Contains("sort=name", queryString);
        Assert.Contains("limit=2", queryString);
        Assert.Contains("limit[betaGroups]=5", queryString);
        Assert.Contains("exists[gameCenterEnabledVersions]=true", queryString);
    }

    [Fact]
    public async Task SendsCommaJoinedFilterValues()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appListResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("id", "6446939457", "1198765432")
            .Filter("bundleId", "com.example.notes")
            .Filter("appStoreVersions.appStoreState", "READY_FOR_SALE", "IN_REVIEW");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var queryString = DecodedQuery(handler);
        Assert.Contains("filter[id]=6446939457,1198765432", queryString);
        Assert.Contains("filter[bundleId]=com.example.notes", queryString);
        Assert.Contains("filter[appStoreVersions.appStoreState]=READY_FOR_SALE,IN_REVIEW", queryString);
    }

    [Fact]
    public async Task SendsBearerTokenAndDefaultHeaders()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appResponse.json");

        var response = await client.GetAppAsync("6446939457", cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("6446939457", response.Data.Id);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/apps/6446939457", request.RequestUri!.ToString());
        Assert.Contains("application/json", request.Headers.Accept.ToString());

        var authorization = request.Headers.Authorization;
        Assert.NotNull(authorization);
        Assert.Equal("Bearer", authorization.Scheme);
        Assert.Equal(3, authorization.Parameter!.Split('.').Length);
    }

    [Fact]
    public async Task SendsAppUpdateRequestBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appResponse.json");

        var request = new AppUpdateRequest
        {
            Data = new AppUpdateRequestData
            {
                Id = "6446939457",
                Attributes = new AppUpdateRequestDataAttributes
                {
                    SubscriptionStatusUrl = "https://example.com/notifications/v2",
                    SubscriptionStatusUrlVersion = SubscriptionStatusUrlVersion.V2,
                    ContentRightsDeclaration = AppContentRightsDeclaration.DoesNotUseThirdPartyContent
                }
            }
        };

        await client.UpdateAppAsync("6446939457", request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal("/v1/apps/6446939457", handler.CapturedRequest.RequestUri!.AbsolutePath);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("apps", data.GetProperty("type").GetString());
        Assert.Equal("6446939457", data.GetProperty("id").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.Equal("https://example.com/notifications/v2", attributes.GetProperty("subscriptionStatusUrl").GetString());
        Assert.Equal("V2", attributes.GetProperty("subscriptionStatusUrlVersion").GetString());
        Assert.Equal(
            "DOES_NOT_USE_THIRD_PARTY_CONTENT",
            attributes.GetProperty("contentRightsDeclaration").GetString());
        Assert.False(attributes.TryGetProperty("bundleId", out _));
    }

    [Fact]
    public async Task RequestsRelatedResourcesForApp()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString("models.testflightBetaGroupsResponse.json"));
        handler.EnqueueJson(TestUtilities.ReadResourceAsString("models.testflightPrereleaseVersionsResponse.json"));
        var client = TestUtilities.CreateClient(handler);

        await client.ListBetaGroupsForAppAsync("6446939457", cancellationToken: TestContext.Current.CancellationToken);
        await client.ListPrereleaseVersionsForAppAsync(
            "6446939457",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/v1/apps/6446939457/betaGroups", handler.CapturedRequests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/v1/apps/6446939457/preReleaseVersions", handler.CapturedRequests[1].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task DecodesTerritoryCurrency()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appTerritoriesResponse.json");

        var response = await client.ListTerritoriesAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/v1/territories", handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Length);
        Assert.Equal("territories", response.Data[0].Type);
        Assert.Equal("USA", response.Data[0].Id);
        Assert.Equal("USD", response.Data[0].Attributes?.Currency);
        Assert.Equal("JPY", response.Data[1].Attributes?.Currency);
    }

    [Fact]
    public async Task ThrowsApiExceptionWithErrorDetails()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appErrorResponse.json", HttpStatusCode.Conflict);

        var exception = await Assert.ThrowsAsync<APIException>(() => client.UpdateAppAsync(
            "6446939457",
            new AppUpdateRequest
            {
                Data = new AppUpdateRequestData
                {
                    Id = "6446939457",
                    Attributes = new AppUpdateRequestDataAttributes
                    {
                        SubscriptionStatusUrl = "http://example.com/notifications"
                    }
                }
            },
            TestContext.Current.CancellationToken));

        Assert.Equal(409, exception.HttpStatusCode);
        Assert.Equal("The subscription status URL must use HTTPS.", exception.Message);

        Assert.NotNull(exception.Errors);
        Assert.Single(exception.Errors!);
        Assert.Equal("ENTITY_ERROR.ATTRIBUTE.INVALID", exception.Errors![0].Code);
        Assert.Equal("409", exception.Errors![0].Status);
        Assert.Equal("/data/attributes/subscriptionStatusUrl", exception.Errors![0].Source?.Pointer);
        Assert.Contains("must use HTTPS", exception.ResponseBody!);
    }

    private static string DecodedQuery(TestHttpMessageHandler handler)
    {
        return Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.Query);
    }
}
