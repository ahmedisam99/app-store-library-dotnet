using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class SubscriptionGroupVersionTests
{
    private const string VersionId = "7c1d2e3f-4a5b-4c6d-8e9f-0a1b2c3d4e5f";

    private const string LocalizationId = "a1b2c3d4-e5f6-4789-8abc-def012345678";

    [Fact]
    public async Task CreatesVersionThenLocalizationRelatedToThatVersion()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.subscriptiongroupversionCreatedResponse.json"),
                HttpStatusCode.Created)
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.subscriptiongroupversionLocalizationResponse.json"),
                HttpStatusCode.Created);
        var client = TestUtilities.CreateClient(handler);

        var version = await client.CreateSubscriptionGroupVersionAsync(
            new SubscriptionGroupVersionCreateRequest
            {
                Data = new SubscriptionGroupVersionCreateRequestData
                {
                    Relationships = new SubscriptionGroupVersionCreateRequestDataRelationships
                    {
                        SubscriptionGroup = RelationshipDeclaration.To("subscriptionGroups", "2000036297")
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(VersionId, version.Data.Id);
        Assert.Equal(SubscriptionGroupVersionState.PrepareForSubmission, version.Data.Attributes!.State);

        var localization = await client.CreateSubscriptionGroupLocalizationV2Async(
            new SubscriptionGroupLocalizationV2CreateRequest
            {
                Data = new SubscriptionGroupLocalizationV2CreateRequestData
                {
                    Attributes = new SubscriptionGroupLocalizationV2CreateRequestDataAttributes
                    {
                        Name = "Ukulele Lessons",
                        Locale = "en-AU",
                        CustomAppName = "The Best Ukulele Lessons"
                    },
                    Relationships = new SubscriptionGroupLocalizationV2CreateRequestDataRelationships
                    {
                        Version = RelationshipDeclaration.To("subscriptionGroupVersions", version.Data.Id)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        handler.AssertAllResponsesConsumed();

        var versionRequest = handler.CapturedRequests[0];

        Assert.Equal(HttpMethod.Post, versionRequest.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions",
            versionRequest.RequestUri!.OriginalString);

        using (var body = JsonDocument.Parse(handler.CapturedRequestBodies[0]!))
        {
            var data = body.RootElement.GetProperty("data");

            Assert.Equal("subscriptionGroupVersions", data.GetProperty("type").GetString());
            Assert.False(data.TryGetProperty("id", out _));
            Assert.False(data.TryGetProperty("attributes", out _));

            var relationships = data.GetProperty("relationships");
            var group = relationships.GetProperty("subscriptionGroup").GetProperty("data");

            Assert.Single(relationships.EnumerateObject());
            Assert.Equal("subscriptionGroups", group.GetProperty("type").GetString());
            Assert.Equal("2000036297", group.GetProperty("id").GetString());
        }

        var localizationRequest = handler.CapturedRequests[1];

        Assert.Equal(HttpMethod.Post, localizationRequest.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/subscriptionGroupLocalizations",
            localizationRequest.RequestUri!.OriginalString);

        using (var body = JsonDocument.Parse(handler.CapturedRequestBodies[1]!))
        {
            var data = body.RootElement.GetProperty("data");

            Assert.Equal("subscriptionGroupLocalizations", data.GetProperty("type").GetString());
            Assert.False(data.TryGetProperty("id", out _));

            var attributes = data.GetProperty("attributes");

            Assert.Equal("Ukulele Lessons", attributes.GetProperty("name").GetString());
            Assert.Equal("en-AU", attributes.GetProperty("locale").GetString());
            Assert.Equal("The Best Ukulele Lessons", attributes.GetProperty("customAppName").GetString());

            var relationships = data.GetProperty("relationships");

            Assert.Single(relationships.EnumerateObject());
            Assert.False(relationships.TryGetProperty("subscriptionGroup", out _));

            var linkedVersion = relationships.GetProperty("version").GetProperty("data");

            Assert.Equal("subscriptionGroupVersions", linkedVersion.GetProperty("type").GetString());
            Assert.Equal(VersionId, linkedVersion.GetProperty("id").GetString());
        }

        Assert.Equal(LocalizationId, localization.Data.Id);
        Assert.Equal(
            VersionId,
            localization.Data.Relationships!["version"].ToOne()!.Id);
    }

    [Fact]
    public async Task LeavesOptionalCustomAppNameOutOfLocalizationCreate()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.subscriptiongroupversionLocalizationResponse.json",
            HttpStatusCode.Created);

        await client.CreateSubscriptionGroupLocalizationV2Async(
            new SubscriptionGroupLocalizationV2CreateRequest
            {
                Data = new SubscriptionGroupLocalizationV2CreateRequestData
                {
                    Attributes = new SubscriptionGroupLocalizationV2CreateRequestDataAttributes
                    {
                        Name = "Ukulele Lessons",
                        Locale = "en-AU"
                    },
                    Relationships = new SubscriptionGroupLocalizationV2CreateRequestDataRelationships
                    {
                        Version = RelationshipDeclaration.To("subscriptionGroupVersions", VersionId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");

        Assert.Equal(2, attributes.EnumerateObject().Count());
        Assert.False(attributes.TryGetProperty("customAppName", out _));
    }

    [Fact]
    public async Task ReadsSubscriptionGroupVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionResponse.json");

        var response = await client.GetSubscriptionGroupVersionAsync(
            "3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d",
            new AppStoreConnectQuery().Include("localizations").Limit("localizations", 10),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions/3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d?include=localizations&limit[localizations]=10",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);

        var version = response.Data;

        Assert.Equal("subscriptionGroupVersions", version.Type);
        Assert.Equal("3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d", version.Id);
        Assert.Equal(1, version.Attributes!.Version);
        Assert.Equal(SubscriptionGroupVersionState.InReview, version.Attributes.State);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions/3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d",
            version.Links!.Self);

        var group = version.Relationships!["subscriptionGroup"].ToOne()!;

        Assert.Equal("subscriptionGroups", group.Type);
        Assert.Equal("2000036297", group.Id);

        var localizations = version.Relationships["localizations"];
        var linked = localizations.ToMany();

        Assert.Equal(2, linked.Length);
        Assert.Equal("subscriptionGroupLocalizations", linked[0].Type);
        Assert.Equal("5e4d3c2b-1a09-4f8e-8d7c-6b5a49382716", linked[0].Id);
        Assert.Equal(2, localizations.Meta!.Paging.Total);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions/3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d/localizations",
            localizations.Links!.Related);
    }

    [Fact]
    public async Task ListsDraftVersionsWithIncludedLocalizations()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionDraftsResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("state", "PREPARE_FOR_SUBMISSION")
            .Include("localizations");

        var response = await client.ListVersionsForSubscriptionGroupAsync(
            "2000036297",
            query,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroups/2000036297/versions?filter[state]=PREPARE_FOR_SUBMISSION&include=localizations",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var draft = Assert.Single(response.Data);

        Assert.Equal(VersionId, draft.Id);
        Assert.Equal(2, draft.Attributes!.Version);
        Assert.Equal(SubscriptionGroupVersionState.PrepareForSubmission, draft.Attributes.State);

        var linkedLocalization = Assert.Single(draft.Relationships!["localizations"].ToMany());

        Assert.True(response.TryGetIncluded<SubscriptionGroupLocalizationV2>(
            linkedLocalization.Type,
            linkedLocalization.Id,
            out var localization));
        Assert.Equal("subscriptionGroupLocalizations", localization.Type);
        Assert.Equal(LocalizationId, localization.Id);
        Assert.Equal("Ukulele Lessons", localization.Attributes!.Name);
        Assert.Equal("The Best Ukulele Lessons", localization.Attributes.CustomAppName);
        Assert.Equal("en-AU", localization.Attributes.Locale);
        Assert.Equal(VersionId, localization.Relationships!["version"].ToOne()!.Id);

        Assert.Equal(1, response.Meta!.Paging.Total);
    }

    [Fact]
    public async Task DecodesEverySubscriptionGroupVersionStateAndUnknownAsUnmapped()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionStatesResponse.json");

        var response = await client.ListVersionsForSubscriptionGroupAsync(
            "2000036297",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroups/2000036297/versions",
            handler.CapturedRequest!.RequestUri!.OriginalString);

        Assert.Equal(
            new SubscriptionGroupVersionState?[]
            {
                SubscriptionGroupVersionState.PrepareForSubmission,
                SubscriptionGroupVersionState.ReadyForReview,
                SubscriptionGroupVersionState.WaitingForReview,
                SubscriptionGroupVersionState.InReview,
                SubscriptionGroupVersionState.Accepted,
                SubscriptionGroupVersionState.Approved,
                SubscriptionGroupVersionState.ReplacedWithNewVersion,
                SubscriptionGroupVersionState.Rejected,
                SubscriptionGroupVersionState.DeveloperRejected,
                SubscriptionGroupVersionState._Unmapped
            },
            response.Data.Select(version => version.Attributes!.State).ToArray());

        Assert.Equal(
            Enumerable.Range(1, 10).Reverse().Select(number => (int?)number).ToArray(),
            response.Data.Select(version => version.Attributes!.Version).ToArray());
    }

    [Fact]
    public async Task ListsLocalizationsOfSubscriptionGroupVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionLocalizationsResponse.json");

        var response = await client.ListLocalizationsForSubscriptionGroupVersionAsync(
            "3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d",
            new AppStoreConnectQuery().Limit(2),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions/3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d/localizations?limit=2",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var english = response.Data[0];

        Assert.Equal("subscriptionGroupLocalizations", english.Type);
        Assert.Equal("5e4d3c2b-1a09-4f8e-8d7c-6b5a49382716", english.Id);
        Assert.Equal("Ukulele Lessons", english.Attributes!.Name);
        Assert.Equal("The Best Ukulele Lessons", english.Attributes.CustomAppName);
        Assert.Equal("en-AU", english.Attributes.Locale);

        var owner = english.Relationships!["version"].ToOne()!;

        Assert.Equal("subscriptionGroupVersions", owner.Type);
        Assert.Equal("3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d", owner.Id);

        var spanish = response.Data[1].Attributes!;

        Assert.Equal("Clases de ukelele", spanish.Name);
        Assert.Null(spanish.CustomAppName);
        Assert.Equal("es-MX", spanish.Locale);

        Assert.Equal(3, response.Meta!.Paging.Total);
        Assert.Equal(2, response.Meta.Paging.Limit);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionGroupVersions/3b9a8c7d-6e5f-4a3b-9c2d-1e0f9a8b7c6d/localizations?cursor=Ag.AQ&limit=2",
            response.Links.Next);
    }

    [Fact]
    public async Task ReadsSubscriptionGroupLocalizationV2()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionLocalizationResponse.json");

        var response = await client.GetSubscriptionGroupLocalizationV2Async(
            LocalizationId,
            new AppStoreConnectQuery().Include("version"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionGroupLocalizations/{LocalizationId}?include=version",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var localization = response.Data;

        Assert.Equal("subscriptionGroupLocalizations", localization.Type);
        Assert.Equal(LocalizationId, localization.Id);
        Assert.Equal("Ukulele Lessons", localization.Attributes!.Name);
        Assert.Equal("The Best Ukulele Lessons", localization.Attributes.CustomAppName);
        Assert.Equal("en-AU", localization.Attributes.Locale);
        Assert.Equal(VersionId, localization.Relationships!["version"].ToOne()!.Id);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionGroupLocalizations/{LocalizationId}",
            localization.Links!.Self);
    }

    [Fact]
    public async Task SendsOnlyAssignedAttributesInLocalizationV2Update()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptiongroupversionLocalizationResponse.json");

        var request = new SubscriptionGroupLocalizationV2UpdateRequest
        {
            Data = new SubscriptionGroupLocalizationV2UpdateRequestData
            {
                Id = LocalizationId,
                Attributes = new SubscriptionGroupLocalizationV2UpdateRequestDataAttributes
                {
                    Name = "Ukulele Lessons Plus"
                }
            }
        };

        await client.UpdateSubscriptionGroupLocalizationV2Async(
            LocalizationId,
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionGroupLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("subscriptionGroupLocalizations", data.GetProperty("type").GetString());
        Assert.Equal(LocalizationId, data.GetProperty("id").GetString());
        Assert.False(data.TryGetProperty("relationships", out _));

        var attributes = data.GetProperty("attributes");

        Assert.Equal("Ukulele Lessons Plus", attributes.GetProperty("name").GetString());
        Assert.Single(attributes.EnumerateObject());
        Assert.False(attributes.TryGetProperty("customAppName", out _));
    }

    [Fact]
    public async Task SendsExplicitNullToClearCustomAppNameAndOmitsItWhenUnassigned()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(TestUtilities.ReadResourceAsString("models.subscriptiongroupversionLocalizationResponse.json"))
            .EnqueueJson(TestUtilities.ReadResourceAsString("models.subscriptiongroupversionLocalizationResponse.json"));
        var client = TestUtilities.CreateClient(handler);

        var cleared = new SubscriptionGroupLocalizationV2UpdateRequestDataAttributes
        {
            CustomAppName = null
        };

        await client.UpdateSubscriptionGroupLocalizationV2Async(
            LocalizationId,
            new SubscriptionGroupLocalizationV2UpdateRequest
            {
                Data = new SubscriptionGroupLocalizationV2UpdateRequestData
                {
                    Id = LocalizationId,
                    Attributes = cleared
                }
            },
            TestContext.Current.CancellationToken);

        var untouched = new SubscriptionGroupLocalizationV2UpdateRequestDataAttributes
        {
            Name = "Ukulele Lessons"
        };

        await client.UpdateSubscriptionGroupLocalizationV2Async(
            LocalizationId,
            new SubscriptionGroupLocalizationV2UpdateRequest
            {
                Data = new SubscriptionGroupLocalizationV2UpdateRequestData
                {
                    Id = LocalizationId,
                    Attributes = untouched
                }
            },
            TestContext.Current.CancellationToken);

        handler.AssertAllResponsesConsumed();

        Assert.True(cleared.IsAssigned(nameof(cleared.CustomAppName)));
        Assert.False(untouched.IsAssigned(nameof(untouched.CustomAppName)));

        using (var body = JsonDocument.Parse(handler.CapturedRequestBodies[0]!))
        {
            var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");

            Assert.True(attributes.TryGetProperty("customAppName", out var customAppName));
            Assert.Equal(JsonValueKind.Null, customAppName.ValueKind);
            Assert.False(attributes.TryGetProperty("name", out _));
        }

        using (var body = JsonDocument.Parse(handler.CapturedRequestBodies[1]!))
        {
            var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");

            Assert.False(attributes.TryGetProperty("customAppName", out _));
            Assert.Equal("Ukulele Lessons", attributes.GetProperty("name").GetString());
        }
    }

    [Fact]
    public async Task DeletesSubscriptionGroupLocalizationV2WithEmptyNoContent()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteSubscriptionGroupLocalizationV2Async(
            LocalizationId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionGroupLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }

    [Theory]
    [InlineData("ListLocalizationsForSubscriptionGroupAsync", "ListLocalizationsForSubscriptionGroupVersionAsync")]
    [InlineData("CreateSubscriptionGroupLocalizationAsync", "CreateSubscriptionGroupLocalizationV2Async")]
    [InlineData("GetSubscriptionGroupLocalizationAsync", "GetSubscriptionGroupLocalizationV2Async")]
    [InlineData("UpdateSubscriptionGroupLocalizationAsync", "UpdateSubscriptionGroupLocalizationV2Async")]
    [InlineData("DeleteSubscriptionGroupLocalizationAsync", "DeleteSubscriptionGroupLocalizationV2Async")]
    [InlineData("CreateSubscriptionGroupSubmissionAsync", "CreateReviewSubmissionItemAsync")]
    public void MarksDeprecatedEndpointObsoleteAndNamesItsReplacement(string deprecated, string replacement)
    {
        var method = typeof(AppStoreConnectAPIClient).GetMethod(deprecated, BindingFlags.Public | BindingFlags.Instance);
        var obsolete = method!.GetCustomAttribute<ObsoleteAttribute>();

        Assert.NotNull(obsolete);
        Assert.StartsWith("Apple deprecated this endpoint in App Store Connect API 4.4.1.", obsolete.Message);
        Assert.Contains(replacement, obsolete.Message);
    }

    [Fact]
    public void LeavesReplacementEndpointsAndTypesUnmarked()
    {
        var methods = new[]
        {
            nameof(AppStoreConnectAPIClient.CreateSubscriptionGroupVersionAsync),
            nameof(AppStoreConnectAPIClient.GetSubscriptionGroupVersionAsync),
            nameof(AppStoreConnectAPIClient.ListVersionsForSubscriptionGroupAsync),
            nameof(AppStoreConnectAPIClient.ListLocalizationsForSubscriptionGroupVersionAsync),
            nameof(AppStoreConnectAPIClient.CreateSubscriptionGroupLocalizationV2Async),
            nameof(AppStoreConnectAPIClient.GetSubscriptionGroupLocalizationV2Async),
            nameof(AppStoreConnectAPIClient.UpdateSubscriptionGroupLocalizationV2Async),
            nameof(AppStoreConnectAPIClient.DeleteSubscriptionGroupLocalizationV2Async),
            nameof(AppStoreConnectAPIClient.CreateSubscriptionGroupAsync),
            nameof(AppStoreConnectAPIClient.GetSubscriptionGroupAsync),
            nameof(AppStoreConnectAPIClient.UpdateSubscriptionGroupAsync),
            nameof(AppStoreConnectAPIClient.DeleteSubscriptionGroupAsync),
            nameof(AppStoreConnectAPIClient.ListSubscriptionsForSubscriptionGroupAsync)
        };

        foreach (var name in methods)
        {
            var method = typeof(AppStoreConnectAPIClient).GetMethod(name, BindingFlags.Public | BindingFlags.Instance);

            Assert.Null(method!.GetCustomAttribute<ObsoleteAttribute>());
        }

        var types = new[]
        {
            typeof(SubscriptionGroupVersion),
            typeof(SubscriptionGroupVersionAttributes),
            typeof(SubscriptionGroupVersionCreateRequest),
            typeof(SubscriptionGroupVersionState),
            typeof(SubscriptionGroupLocalizationV2),
            typeof(SubscriptionGroupLocalizationV2Attributes),
            typeof(SubscriptionGroupLocalizationV2CreateRequest),
            typeof(SubscriptionGroupLocalizationV2UpdateRequest),
            typeof(SubscriptionGroup)
        };

        foreach (var type in types)
        {
            Assert.Null(type.GetCustomAttribute<ObsoleteAttribute>());
        }
    }

    [Theory]
    [InlineData("SubscriptionGroupLocalization")]
    [InlineData("SubscriptionGroupLocalizationAttributes")]
    [InlineData("SubscriptionGroupLocalizationCreateRequest")]
    [InlineData("SubscriptionGroupLocalizationCreateRequestData")]
    [InlineData("SubscriptionGroupLocalizationCreateRequestDataAttributes")]
    [InlineData("SubscriptionGroupLocalizationCreateRequestDataRelationships")]
    [InlineData("SubscriptionGroupLocalizationUpdateRequest")]
    [InlineData("SubscriptionGroupLocalizationUpdateRequestData")]
    [InlineData("SubscriptionGroupLocalizationUpdateRequestDataAttributes")]
    [InlineData("SubscriptionGroupSubmission")]
    [InlineData("SubscriptionGroupSubmissionAttributes")]
    [InlineData("SubscriptionGroupSubmissionCreateRequest")]
    [InlineData("SubscriptionGroupSubmissionCreateRequestData")]
    [InlineData("SubscriptionGroupSubmissionCreateRequestDataRelationships")]
    [InlineData("Enums.SubscriptionGroupLocalizationState")]
    public void MarksDeprecatedModelTypeObsolete(string typeName)
    {
        // Looked up by name so the test itself doesn't reference an obsolete type.
        var type = typeof(SubscriptionGroupVersion).Assembly.GetType($"Enjna.AppStoreConnectApi.Models.{typeName}");
        var obsolete = type!.GetCustomAttribute<ObsoleteAttribute>();

        Assert.NotNull(obsolete);
        Assert.Contains("in App Store Connect API 4.4.1.", obsolete.Message);
    }
}
