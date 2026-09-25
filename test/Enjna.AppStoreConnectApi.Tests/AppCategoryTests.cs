using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class AppCategoryTests
{
    private const string ParentCategoryId = "GAMES";

    private const string SubcategoryId = "GAMES_PUZZLE";

    private const string AppTagId = "eyJhIjoiNjQ0NjkzOTQ1NyIsInQiOiJqb3VybmFsaW5nIn0";

    [Fact]
    public async Task DecodesAppCategoryListAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appcategoryListResponse.json");

        var response = await client.ListAppCategoriesAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories",
            handler.CapturedRequest.RequestUri!.ToString());

        Assert.Equal(2, response.Data.Length);

        var games = response.Data[0];
        Assert.Equal("appCategories", games.Type);
        Assert.Equal(ParentCategoryId, games.Id);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/appCategories/GAMES", games.Links?.Self);
        Assert.Equal(
            new[] { Platform.Ios, Platform.MacOs, Platform.TvOs },
            games.Attributes?.Platforms);

        Assert.Equal(
            new[] { Platform.Ios, Platform.MacOs, Platform.VisionOs },
            response.Data[1].Attributes?.Platforms);

        Assert.Equal(28, response.Meta?.Paging.Total);
        Assert.Equal(2, response.Meta?.Paging.Limit);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories?cursor=BQ.o3Q9NQ&limit=2",
            response.Links.Next);
    }

    [Fact]
    public async Task ReadsParentAndSubcategoryRelationshipsOfAppCategory()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appcategoryListResponse.json");

        var response = await client.ListAppCategoriesAsync(cancellationToken: TestContext.Current.CancellationToken);

        var relationships = response.Data[0].Relationships;
        Assert.NotNull(relationships);

        var parent = relationships["parent"];
        Assert.True(parent.HasNullLinkage);
        Assert.Null(parent.ToOne());

        var subcategories = relationships["subcategories"].ToMany();
        Assert.Equal(2, subcategories.Length);
        Assert.Equal("appCategories", subcategories[0].Type);
        Assert.Equal(SubcategoryId, subcategories[0].Id);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories/GAMES/subcategories",
            relationships["subcategories"].Links?.Related);

        Assert.Empty(response.Data[1].Relationships!["subcategories"].ToMany());

        Assert.True(response.TryGetIncluded<AppCategory>("appCategories", SubcategoryId, out var included));
        Assert.Equal(
            new[] { Platform.Ios, Platform.MacOs, Platform.TvOs },
            included.Attributes?.Platforms);
    }

    [Fact]
    public async Task ReadsSingleAppCategory()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appcategoryResponse.json");

        var response = await client.GetAppCategoryAsync(
            SubcategoryId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories/GAMES_PUZZLE",
            handler.CapturedRequest.RequestUri!.ToString());

        Assert.Equal(SubcategoryId, response.Data.Id);
        Assert.Equal(ParentCategoryId, response.Data.Relationships!["parent"].ToOne()?.Id);
        Assert.True(response.TryGetIncluded<AppCategory>("appCategories", ParentCategoryId, out var parent));
        Assert.Equal("appCategories", parent.Type);
    }

    [Fact]
    public async Task MapsUnknownPlatformOfAppCategoryToUnmappedValue()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.appcategoryUnknownPlatformResponse.json");

        var response = await client.GetAppCategoryAsync(
            SubcategoryId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            new[] { Platform.Ios, Platform._Unmapped },
            response.Data.Attributes?.Platforms);
    }

    [Fact]
    public async Task ReadsParentOfAppCategory()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appcategoryParentResponse.json");

        var response = await client.GetParentForAppCategoryAsync(
            SubcategoryId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories/GAMES_PUZZLE/parent",
            handler.CapturedRequest.RequestUri!.ToString());

        Assert.Equal(ParentCategoryId, response.Data.Id);
        Assert.Equal("https://api.appstoreconnect.apple.com/v1/appCategories/GAMES", response.Data.Links?.Self);
    }

    [Fact]
    public async Task ListsSubcategoriesOfAppCategory()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appcategorySubcategoriesResponse.json");

        var response = await client.ListSubcategoriesForAppCategoryAsync(
            ParentCategoryId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/appCategories/GAMES/subcategories",
            handler.CapturedRequest.RequestUri!.ToString());

        Assert.Equal(2, response.Data.Length);
        Assert.Equal(SubcategoryId, response.Data[0].Id);
        Assert.Equal("GAMES_STRATEGY", response.Data[1].Id);
        Assert.Equal(2, response.Meta?.Paging.Total);
    }

    [Fact]
    public async Task SendsFilterIncludeAndLimitQueryParametersForAppCategories()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.appcategoryListResponse.json");

        var query = new AppStoreConnectQuery()
            .Fields("appCategories", "platforms", "subcategories")
            .Filter("platforms", "IOS", "MAC_OS")
            .Exists("parent", "false")
            .Include("subcategories")
            .Limit(2)
            .Limit("subcategories", 5);

        await client.ListAppCategoriesAsync(query, TestContext.Current.CancellationToken);

        Assert.Equal("/v1/appCategories", handler.CapturedRequest!.RequestUri!.AbsolutePath);

        var queryString = DecodedQuery(handler);
        Assert.Contains("fields[appCategories]=platforms,subcategories", queryString);
        Assert.Contains("filter[platforms]=IOS,MAC_OS", queryString);
        Assert.Contains("exists[parent]=false", queryString);
        Assert.Contains("include=subcategories", queryString);
        Assert.Contains("limit=2", queryString);
        Assert.Contains("limit[subcategories]=5", queryString);
    }

    [Fact]
    public async Task SendsAppTagUpdateRequestBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.apptagResponse.json");

        var request = new AppTagUpdateRequest
        {
            Data = new AppTagUpdateRequestData
            {
                Id = AppTagId,
                Attributes = new AppTagUpdateRequestDataAttributes
                {
                    VisibleInAppStore = false
                }
            }
        };

        var response = await client.UpdateAppTagAsync(AppTagId, request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/appTags/{AppTagId}",
            handler.CapturedRequest.RequestUri!.ToString());
        Assert.Equal("application/json", handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("appTags", data.GetProperty("type").GetString());
        Assert.Equal(AppTagId, data.GetProperty("id").GetString());
        Assert.False(data.GetProperty("attributes").GetProperty("visibleInAppStore").GetBoolean());

        Assert.Equal("Journaling", response.Data.Attributes?.Name);
        Assert.False(response.Data.Attributes?.VisibleInAppStore);
        Assert.Equal("USA", response.Data.Relationships!["territories"].ToMany()[0].Id);
        Assert.True(response.TryGetIncluded<Territory>("territories", "USA", out var territory));
        Assert.Equal("USD", territory.Attributes?.Currency);
    }

    [Fact]
    public async Task LeavesAppTagVisibilityOutOfTheRequestWhenItIsNeverAssigned()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.apptagResponse.json");

        var request = new AppTagUpdateRequest
        {
            Data = new AppTagUpdateRequestData
            {
                Id = AppTagId,
                Attributes = new AppTagUpdateRequestDataAttributes()
            }
        };

        await client.UpdateAppTagAsync(AppTagId, request, TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");
        Assert.False(attributes.TryGetProperty("visibleInAppStore", out _));
    }

    [Fact]
    public async Task SendsExplicitNullWhenAppTagVisibilityIsAssignedNull()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.apptagResponse.json");

        var request = new AppTagUpdateRequest
        {
            Data = new AppTagUpdateRequestData
            {
                Id = AppTagId,
                Attributes = new AppTagUpdateRequestDataAttributes
                {
                    VisibleInAppStore = null
                }
            }
        };

        await client.UpdateAppTagAsync(AppTagId, request, TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");
        Assert.Equal(JsonValueKind.Null, attributes.GetProperty("visibleInAppStore").ValueKind);
    }

    [Fact]
    public async Task ListsTerritoriesForAppTag()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.apptagTerritoriesResponse.json");

        var response = await client.ListTerritoriesForAppTagAsync(
            AppTagId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/appTags/{AppTagId}/territories",
            handler.CapturedRequest.RequestUri!.ToString());

        Assert.Equal(2, response.Data.Length);
        Assert.Equal("territories", response.Data[0].Type);
        Assert.Equal("USA", response.Data[0].Id);
        Assert.Equal("USD", response.Data[0].Attributes?.Currency);
        Assert.Equal("JPY", response.Data[1].Attributes?.Currency);
    }

    [Fact]
    public async Task ThrowsApiExceptionWhenTheAppTagUpdateIsRejected()
    {
        var (client, _) = TestUtilities.GetClientWithJson(
            "models.appcategoryErrorResponse.json",
            HttpStatusCode.Conflict);

        var exception = await Assert.ThrowsAsync<APIException>(() => client.UpdateAppTagAsync(
            AppTagId,
            new AppTagUpdateRequest
            {
                Data = new AppTagUpdateRequestData
                {
                    Id = AppTagId,
                    Attributes = new AppTagUpdateRequestDataAttributes
                    {
                        VisibleInAppStore = false
                    }
                }
            },
            TestContext.Current.CancellationToken));

        Assert.Equal(409, exception.HttpStatusCode);
        Assert.Equal("The app tag can't be hidden while the app is in review.", exception.Message);
        Assert.Single(exception.Errors!);
        Assert.Equal("ENTITY_ERROR.ATTRIBUTE.INVALID", exception.Errors![0].Code);
        Assert.Equal("/data/attributes/visibleInAppStore", exception.Errors![0].Source?.Pointer);
    }

    private static string DecodedQuery(TestHttpMessageHandler handler)
    {
        return Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.Query);
    }
}
