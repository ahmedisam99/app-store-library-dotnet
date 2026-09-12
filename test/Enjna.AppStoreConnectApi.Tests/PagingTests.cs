using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class PagingTests
{
    private const string Page1Resource = "paging.appsPage1.json";
    private const string Page2Resource = "paging.appsPage2.json";
    private const string RelativeNextResource = "paging.appsPageRelativeNext.json";

    [Fact]
    public async Task ReturnsNullWhenTheNextPageLinkIsAbsent()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var lastPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        var afterLastPage = await client.GetNextPageAsync(lastPage, TestContext.Current.CancellationToken);

        Assert.Null(lastPage.Links.Next);
        Assert.Null(afterLastPage);
        Assert.Single(handler.CapturedRequests);
    }

    [Fact]
    public async Task FollowsAnAbsoluteNextPageLink()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page1Resource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        var secondPage = await client.GetNextPageAsync(firstPage, TestContext.Current.CancellationToken);

        Assert.Equal(2, handler.CapturedRequests.Count);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/apps?cursor=BQ.QhQ7Nw&limit=2",
            handler.CapturedRequests[1].RequestUri!.AbsoluteUri);

        Assert.NotNull(secondPage);
        Assert.Equal("6446901003", Assert.Single(secondPage.Data).Id);
    }

    [Fact]
    public async Task SendsABearerTokenWithTheNextPageRequest()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page1Resource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);
        await client.GetNextPageAsync(firstPage, TestContext.Current.CancellationToken);

        var authorization = handler.CapturedRequests[1].Headers.Authorization;

        Assert.NotNull(authorization);
        Assert.Equal("Bearer", authorization.Scheme);
        Assert.Equal(3, authorization.Parameter!.Split('.').Length);
    }

    [Fact]
    public async Task ResolvesARelativeNextPageLinkAgainstTheBaseUrl()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(RelativeNextResource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/v1/apps?cursor=REL.7hJk9Q&limit=1", firstPage.Links.Next);

        var secondPage = await client.GetNextPageAsync(firstPage, TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/apps?cursor=REL.7hJk9Q&limit=1",
            handler.CapturedRequests[1].RequestUri!.AbsoluteUri);

        Assert.NotNull(secondPage);
    }

    [Fact]
    public async Task EnumeratesEveryResourceOfEveryPageInOrder()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page1Resource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var ids = new List<string?>();
        var bundleIds = new List<string?>();

        var resources = client.EnumerateResourcesAsync(firstPage, TestContext.Current.CancellationToken);

        await foreach (var app in resources)
        {
            ids.Add(app.Id);
            bundleIds.Add(app.Attributes?.BundleId);
        }

        Assert.Equal(new[] { "6446901001", "6446901002", "6446901003" }, ids);
        Assert.Equal(
            new[] { "com.example.reader", "com.example.writer", "com.example.player" },
            bundleIds);
        Assert.Equal(2, handler.CapturedRequests.Count);
    }

    [Fact]
    public async Task EnumeratesThePagesThemselvesStartingWithTheGivenOne()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page1Resource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        var client = TestUtilities.CreateClient(handler);

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var pages = new List<ResourceListResponse<App>>();

        await foreach (var page in client.EnumeratePagesAsync(firstPage, TestContext.Current.CancellationToken))
        {
            pages.Add(page);
        }

        Assert.Equal(2, pages.Count);
        Assert.Same(firstPage, pages[0]);
        Assert.Equal(2, pages[0].Data.Length);
        Assert.Single(pages[1].Data);
        Assert.Null(pages[1].Links.Next);
    }

    [Fact]
    public async Task ReadsThePagingTotalAndLimitOfAPage()
    {
        var (client, _) = TestUtilities.GetClientWithJson(Page1Resource);

        var page = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(3, page.Meta!.Paging.Total);
        Assert.Equal(2, page.Meta.Paging.Limit);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/apps?limit=2", page.Links.Self);
    }

    [Fact]
    public async Task KeepsAPathPrefixedBaseUrlForEveryPage()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page1Resource))
            .EnqueueJson(TestUtilities.ReadResourceAsString(Page2Resource));

        using var client = new AppStoreConnectAPIClient(
            TestUtilities.GetSigningKey(),
            TestUtilities.KeyId,
            TestUtilities.IssuerId,
            new HttpClient(handler),
            "https://gateway.corp/asc");

        var firstPage = await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        await client.GetNextPageAsync(firstPage, TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://gateway.corp/asc/v1/apps",
            handler.CapturedRequests[0].RequestUri!.AbsoluteUri);

        Assert.Equal(
            "https://gateway.corp/asc/v1/apps?cursor=BQ.QhQ7Nw&limit=2",
            handler.CapturedRequests[1].RequestUri!.AbsoluteUri);
    }

}
