using System;
using System.Threading.Tasks;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class AppStoreConnectQueryTests
{
    private const string EmptyPageResource = "query.appsEmptyPage.json";

    [Fact]
    public void BuildsSparseFieldsetParameterForItsResourceType()
    {
        var parameters = new AppStoreConnectQuery()
            .Fields("apps", "name", "bundleId")
            .ToQueryParameters();

        Assert.Equal(new[] { "name", "bundleId" }, parameters["fields[apps]"]);
        Assert.Single(parameters);
    }

    [Fact]
    public void BuildsBracketedFilterAndExistsParameters()
    {
        var parameters = new AppStoreConnectQuery()
            .Filter("bundleId", "com.example.reader", "com.example.writer")
            .Exists("gameCenterEnabledVersions", "true")
            .ToQueryParameters();

        Assert.Equal(new[] { "com.example.reader", "com.example.writer" }, parameters["filter[bundleId]"]);
        Assert.Equal(new[] { "true" }, parameters["exists[gameCenterEnabledVersions]"]);
    }

    [Fact]
    public void BuildsUnbracketedIncludeSortAndLimitParameters()
    {
        var parameters = new AppStoreConnectQuery()
            .Include("builds", "appStoreVersions")
            .Sort("-name", "bundleId")
            .Limit(200)
            .ToQueryParameters();

        Assert.Equal(new[] { "builds", "appStoreVersions" }, parameters["include"]);
        Assert.Equal(new[] { "-name", "bundleId" }, parameters["sort"]);
        Assert.Equal(new[] { "200" }, parameters["limit"]);
    }

    [Fact]
    public void BuildsRelatedResourceLimitSeparatelyFromTheTopLevelLimit()
    {
        var parameters = new AppStoreConnectQuery()
            .Limit(10)
            .Limit("builds", 5)
            .ToQueryParameters();

        Assert.Equal(new[] { "10" }, parameters["limit"]);
        Assert.Equal(new[] { "5" }, parameters["limit[builds]"]);
    }

    [Fact]
    public void BuildsArbitraryParameterUnderItsFullName()
    {
        var parameters = new AppStoreConnectQuery()
            .Parameter("filter[vendorNumber]", "80012345")
            .ToQueryParameters();

        Assert.Equal(new[] { "80012345" }, parameters["filter[vendorNumber]"]);
    }

    [Fact]
    public void ReplacesTheValuesWhenTheSameKeyIsSetTwice()
    {
        var parameters = new AppStoreConnectQuery()
            .Filter("bundleId", "com.example.first")
            .Filter("bundleId", "com.example.second", "com.example.third")
            .Fields("apps", "name")
            .Fields("apps", "bundleId", "sku")
            .Limit(10)
            .Limit(25)
            .ToQueryParameters();

        Assert.Equal(new[] { "com.example.second", "com.example.third" }, parameters["filter[bundleId]"]);
        Assert.Equal(new[] { "bundleId", "sku" }, parameters["fields[apps]"]);
        Assert.Equal(new[] { "25" }, parameters["limit"]);
        Assert.Equal(3, parameters.Count);
    }

    [Fact]
    public void ReturnsAFreshDictionaryThatDoesNotWriteBackIntoTheQuery()
    {
        var query = new AppStoreConnectQuery().Filter("bundleId", "com.example.reader");

        var first = query.ToQueryParameters();
        first["filter[bundleId]"] = new[] { "com.example.tampered" };
        first["sort"] = new[] { "-name" };

        var second = query.ToQueryParameters();

        Assert.NotSame(first, second);
        Assert.Equal(new[] { "com.example.reader" }, second["filter[bundleId]"]);
        Assert.False(second.ContainsKey("sort"));
    }

    [Fact]
    public void ReturnsCopiesOfTheValueArrays()
    {
        var query = new AppStoreConnectQuery().Fields("apps", "name", "bundleId");

        var first = query.ToQueryParameters();
        first["fields[apps]"][0] = "tampered";

        Assert.Equal(new[] { "name", "bundleId" }, query.ToQueryParameters()["fields[apps]"]);
    }

    [Fact]
    public void KeepsTheValuesItWasGivenWhenTheCallerMutatesTheSourceArray()
    {
        var fields = new[] { "name", "bundleId" };
        var query = new AppStoreConnectQuery().Fields("apps", fields);

        fields[0] = "tampered";

        Assert.Equal(new[] { "name", "bundleId" }, query.ToQueryParameters()["fields[apps]"]);
    }

    [Fact]
    public async Task SendsCommaJoinedFilterValuesAsOneQueryParameter()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        var query = new AppStoreConnectQuery()
            .Filter("bundleId", "com.example.reader", "com.example.writer");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var requestQuery = handler.CapturedRequest!.RequestUri!.Query;

        Assert.Equal("?filter[bundleId]=com.example.reader,com.example.writer", requestQuery);
    }

    [Fact]
    public async Task SendsBracketedKeysWithoutPercentEncodingThem()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        var query = new AppStoreConnectQuery()
            .Fields("apps", "name", "bundleId")
            .Limit("builds", 5)
            .Exists("gameCenterEnabledVersions", "true");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var requestQuery = handler.CapturedRequest!.RequestUri!.Query;

        Assert.Contains("fields[apps]=name,bundleId", requestQuery);
        Assert.Contains("limit[builds]=5", requestQuery);
        Assert.Contains("exists[gameCenterEnabledVersions]=true", requestQuery);
        Assert.DoesNotContain("%5B", requestQuery);
        Assert.DoesNotContain("%5D", requestQuery);
    }

    [Fact]
    public async Task SendsTheSeparatingCommaUnencodedButEscapesTheValues()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        var query = new AppStoreConnectQuery().Filter("name", "Enjna Reader", "Enjna Writer");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var requestQuery = handler.CapturedRequest!.RequestUri!.Query;

        Assert.Equal("?filter[name]=Enjna%20Reader,Enjna%20Writer", requestQuery);
    }

    [Fact]
    public async Task SendsSeveralParametersSeparatedByAmpersands()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        var query = new AppStoreConnectQuery()
            .Filter("bundleId", "com.example.reader")
            .Include("builds")
            .Limit(200);

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var requestUri = handler.CapturedRequest!.RequestUri!;
        var requestQuery = requestUri.Query;

        Assert.Equal("/v1/apps", requestUri.AbsolutePath);
        Assert.StartsWith("?", requestQuery);
        Assert.Equal(3, requestQuery.TrimStart('?').Split('&').Length);
        Assert.Contains("filter[bundleId]=com.example.reader", requestQuery);
        Assert.Contains("include=builds", requestQuery);
        Assert.Contains("limit=200", requestQuery);
    }

    [Fact]
    public async Task SendsNoQueryStringWhenThereIsNoQuery()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        await client.ListAppsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/apps",
            handler.CapturedRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task EscapesBracketsInsideAValue()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(EmptyPageResource);

        var query = new AppStoreConnectQuery().Filter("name", "My [Beta] Group");

        await client.ListAppsAsync(query, TestContext.Current.CancellationToken);

        var requestQuery = handler.CapturedRequest!.RequestUri!.Query;

        Assert.Equal("?filter[name]=My%20%5BBeta%5D%20Group", requestQuery);
    }

    [Fact]
    public void ThrowsWhenTheValuesAreNull()
    {
        var query = new AppStoreConnectQuery();

        Assert.Throws<ArgumentNullException>(() => query.Filter("bundleId", null!));
        Assert.Throws<ArgumentNullException>(() => query.Parameter(null!, "value"));
        Assert.Throws<ArgumentException>(() => query.Filter("bundleId", "com.example", null!));
    }

    [Fact]
    public void ThrowsWhenAnyBuilderMethodIsGivenANullArray()
    {
        var query = new AppStoreConnectQuery();

        Assert.Throws<ArgumentNullException>(() => query.Fields("apps", null!));
        Assert.Throws<ArgumentNullException>(() => query.Filter("bundleId", null!));
        Assert.Throws<ArgumentNullException>(() => query.Include(null!));
        Assert.Throws<ArgumentNullException>(() => query.Sort(null!));
        Assert.Throws<ArgumentNullException>(() => query.Parameter("filter[vendorNumber]", null!));
    }

    [Fact]
    public void ThrowsWhenAnyBuilderMethodIsGivenNoValues()
    {
        var query = new AppStoreConnectQuery();

        Assert.Equal("fields", Assert.Throws<ArgumentException>(() => query.Fields("apps")).ParamName);
        Assert.Equal("values", Assert.Throws<ArgumentException>(() => query.Filter("id")).ParamName);
        Assert.Equal("relationships", Assert.Throws<ArgumentException>(() => query.Include()).ParamName);
        Assert.Equal("sorts", Assert.Throws<ArgumentException>(() => query.Sort()).ParamName);
        Assert.Equal(
            "values",
            Assert.Throws<ArgumentException>(() => query.Parameter("filter[vendorNumber]")).ParamName);

        Assert.Throws<ArgumentException>(() => query.Filter("id", Array.Empty<string>()));
        Assert.Empty(query.ToQueryParameters());
    }

    [Fact]
    public void ThrowsWhenAnyBuilderMethodIsGivenABlankValue()
    {
        var query = new AppStoreConnectQuery();

        Assert.Throws<ArgumentException>(() => query.Fields("apps", "name", null!));
        Assert.Throws<ArgumentException>(() => query.Filter("bundleId", string.Empty));
        Assert.Throws<ArgumentException>(() => query.Include("builds", "   "));
        Assert.Throws<ArgumentException>(() => query.Sort(""));
        Assert.Throws<ArgumentException>(() => query.Parameter("filter[vendorNumber]", " "));
        Assert.Throws<ArgumentException>(() => query.Exists("gameCenterEnabledVersions", null!));

        Assert.Empty(query.ToQueryParameters());
    }

    [Fact]
    public void KeepsBuildingTheSameQueryAfterARejectedCall()
    {
        var query = new AppStoreConnectQuery().Filter("bundleId", "com.example.reader");

        Assert.Throws<ArgumentException>(() => query.Filter("id", Array.Empty<string>()));

        var parameters = query
            .Include("builds")
            .ToQueryParameters();

        Assert.Equal(new[] { "com.example.reader" }, parameters["filter[bundleId]"]);
        Assert.Equal(new[] { "builds" }, parameters["include"]);
        Assert.Equal(2, parameters.Count);
    }
}
