using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class TestFlightTests
{
    private const string BuildId = "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f";
    private const string InternalGroupId = "0d5e9ee1-4c02-4b7a-9f3b-2b0c9a1d7e11";
    private const string PublicGroupId = "7c4b3a29-8d51-4e60-b2f7-1a9c8d7e6f50";
    private const string TesterId = "5f6a7b8c-9d0e-4f1a-8b2c-3d4e5f6a7b8c";

    [Fact]
    public async Task DecodesBuildAttributes()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.testflightBuildsResponse.json");

        var response = await client.ListBuildsAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(2, response.Data.Length);

        var build = response.Data[0];
        Assert.Equal("builds", build.Type);
        Assert.Equal(BuildId, build.Id);

        var attributes = build.Attributes;
        Assert.NotNull(attributes);
        Assert.Equal("1042", attributes.Version);
        Assert.Equal(
            new DateTimeOffset(2024, 5, 14, 10, 32, 41, TimeSpan.FromHours(-7)),
            attributes.UploadedDate);
        Assert.Equal(
            new DateTimeOffset(2024, 8, 12, 10, 32, 41, TimeSpan.FromHours(-7)),
            attributes.ExpirationDate);
        Assert.False(attributes.Expired);
        Assert.Equal("16.0", attributes.MinOsVersion);
        Assert.Equal("13.0", attributes.LsMinimumSystemVersion);
        Assert.Equal("13.0", attributes.ComputedMinMacOsVersion);
        Assert.Equal("1.1", attributes.ComputedMinVisionOsVersion);
        Assert.Equal(BuildProcessingState.Valid, attributes.ProcessingState);
        Assert.Equal(BuildAudienceType.AppStoreEligible, attributes.BuildAudienceType);
        Assert.False(attributes.UsesNonExemptEncryption);

        Assert.Equal(
            "https://is1-ssl.mzstatic.com/image/thumb/abc123/{w}x{h}{c}.{f}",
            attributes.IconAssetToken?.TemplateUrl);
        Assert.Equal(1024, attributes.IconAssetToken?.Width);
        Assert.Equal(1024, attributes.IconAssetToken?.Height);

        var processing = response.Data[1];
        Assert.Equal(BuildProcessingState.Processing, processing.Attributes?.ProcessingState);
        Assert.Equal(BuildAudienceType.InternalOnly, processing.Attributes?.BuildAudienceType);
        Assert.True(processing.Attributes?.Expired);
        Assert.Null(processing.Attributes?.IconAssetToken);
    }

    [Fact]
    public async Task ReadsBuildRelationshipsAndIncludedApp()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.testflightBuildsResponse.json");

        var response = await client.ListBuildsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var relationships = response.Data[0].Relationships!;
        var app = relationships["app"].ToOne();
        Assert.Equal("apps", app?.Type);
        Assert.Equal("6446939457", app?.Id);

        var betaGroups = relationships["betaGroups"].ToMany();
        Assert.Equal(2, betaGroups.Length);
        Assert.Equal(InternalGroupId, betaGroups[0].Id);
        Assert.Equal(PublicGroupId, betaGroups[1].Id);

        Assert.True(response.TryGetIncluded<App>("apps", "6446939457", out var includedApp));
        Assert.Equal("com.example.notes", includedApp.Attributes?.BundleId);
    }

    [Fact]
    public async Task DecodesBetaGroupAttributes()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.testflightBetaGroupsResponse.json");

        var response = await client.ListBetaGroupsAsync(cancellationToken: TestContext.Current.CancellationToken);

        var internalGroup = response.Data[0];
        Assert.Equal("betaGroups", internalGroup.Type);
        Assert.Equal("Internal QA", internalGroup.Attributes?.Name);
        Assert.True(internalGroup.Attributes?.IsInternalGroup);
        Assert.True(internalGroup.Attributes?.HasAccessToAllBuilds);
        Assert.False(internalGroup.Attributes?.PublicLinkEnabled);
        Assert.Null(internalGroup.Attributes?.PublicLink);
        Assert.True(internalGroup.Attributes?.IosBuildsAvailableForAppleVision);
        Assert.Equal(
            new DateTimeOffset(2024, 1, 9, 17, 4, 22, TimeSpan.FromHours(-8)),
            internalGroup.Attributes?.CreatedDate);

        var publicGroup = response.Data[1];
        Assert.False(publicGroup.Attributes?.IsInternalGroup);
        Assert.True(publicGroup.Attributes?.PublicLinkEnabled);
        Assert.True(publicGroup.Attributes?.PublicLinkLimitEnabled);
        Assert.Equal(5000, publicGroup.Attributes?.PublicLinkLimit);
        Assert.Equal("2a5f9c31", publicGroup.Attributes?.PublicLinkId);
        Assert.Equal("https://testflight.apple.com/join/2a5f9c31", publicGroup.Attributes?.PublicLink);
    }

    [Fact]
    public async Task DecodesBetaTesterDevices()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.testflightBetaTestersResponse.json");

        var response = await client.ListBetaTestersForBetaGroupAsync(
            InternalGroupId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            $"/v1/betaGroups/{InternalGroupId}/betaTesters",
            handler.CapturedRequest!.RequestUri!.AbsolutePath);

        var tester = response.Data[0];
        Assert.Equal("Dana", tester.Attributes?.FirstName);
        Assert.Equal("Okafor", tester.Attributes?.LastName);
        Assert.Equal("dana.okafor@example.com", tester.Attributes?.Email);
        Assert.Equal(BetaInviteType.Email, tester.Attributes?.InviteType);
        Assert.Equal(BetaTesterState.Installed, tester.Attributes?.State);

        var devices = tester.Attributes!.AppDevices!;
        Assert.Equal(2, devices.Length);
        Assert.Equal("iPhone 15 Pro", devices[0].Model);
        Assert.Equal(BetaTesterAppDevicePlatform.Ios, devices[0].Platform);
        Assert.Equal("17.5.1", devices[0].OsVersion);
        Assert.Equal("1042", devices[0].AppBuildVersion);
        Assert.Equal(BetaTesterAppDevicePlatform.VisionOs, devices[1].Platform);

        var invited = response.Data[1];
        Assert.Equal(BetaInviteType.PublicLink, invited.Attributes?.InviteType);
        Assert.Equal(BetaTesterState.Invited, invited.Attributes?.State);
        Assert.Empty(invited.Attributes!.AppDevices!);
    }

    [Fact]
    public async Task DecodesPrereleaseVersions()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.testflightPrereleaseVersionsResponse.json");

        var response = await client.ListPrereleaseVersionsAsync(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(2, response.Data.Length);
        Assert.Equal("preReleaseVersions", response.Data[0].Type);
        Assert.Equal("1.4.0", response.Data[0].Attributes?.Version);
        Assert.Equal(Platform.Ios, response.Data[0].Attributes?.Platform);
        Assert.Equal(Platform.VisionOs, response.Data[1].Attributes?.Platform);

        var builds = response.Data[0].Relationships!["builds"].ToMany();
        Assert.Equal(BuildId, Assert.Single(builds).Id);
    }

    [Fact]
    public async Task AddsBetaGroupsToBuildWithLinkagePayload()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.AddBetaGroupsToBuildAsync(
            BuildId,
            [InternalGroupId, PublicGroupId],
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal($"/v1/builds/{BuildId}/relationships/betaGroups", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaGroups", InternalGroupId, PublicGroupId);
    }

    [Fact]
    public async Task RemovesBetaGroupsFromBuildWithDeleteRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.RemoveBetaGroupsFromBuildAsync(
            BuildId,
            [PublicGroupId],
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal($"/v1/builds/{BuildId}/relationships/betaGroups", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaGroups", PublicGroupId);
    }

    [Fact]
    public async Task AddsIndividualTestersToBuildWithLinkagePayload()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.AddIndividualTestersToBuildAsync(BuildId, [TesterId], TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal($"/v1/builds/{BuildId}/relationships/individualTesters", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaTesters", TesterId);
    }

    [Fact]
    public async Task AddsBetaTestersToBetaGroupWithLinkagePayload()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.AddBetaTestersToBetaGroupAsync(
            InternalGroupId,
            [TesterId],
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal($"/v1/betaGroups/{InternalGroupId}/relationships/betaTesters", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaTesters", TesterId);
    }

    [Fact]
    public async Task RemovesBetaTestersFromBetaGroupWithDeleteRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.RemoveBetaTestersFromBetaGroupAsync(
            InternalGroupId,
            [TesterId],
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal($"/v1/betaGroups/{InternalGroupId}/relationships/betaTesters", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaTesters", TesterId);
    }

    [Fact]
    public async Task AddsBuildsToBetaGroupWithLinkagePayload()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.AddBuildsToBetaGroupAsync(PublicGroupId, [BuildId], TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal($"/v1/betaGroups/{PublicGroupId}/relationships/builds", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "builds", BuildId);
    }

    [Fact]
    public async Task RemovesBuildsFromBetaGroupWithDeleteRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.RemoveBuildsFromBetaGroupAsync(PublicGroupId, [BuildId], TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal($"/v1/betaGroups/{PublicGroupId}/relationships/builds", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "builds", BuildId);
    }

    [Fact]
    public async Task RemovesBetaTestersFromAppWithDeleteRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.RemoveBetaTestersFromAppAsync("6446939457", [TesterId], TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal("/v1/apps/6446939457/relationships/betaTesters", request.RequestUri!.AbsolutePath);

        AssertLinkagePayload(handler.CapturedRequestBody!, "betaTesters", TesterId);
    }

    [Fact]
    public async Task SendsRequiredBuildFilterForBetaAppReviewSubmissions()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson("{\"data\":[],\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/betaAppReviewSubmissions\"}}");
        var client = TestUtilities.CreateClient(handler);

        await client.ListBetaAppReviewSubmissionsAsync(
            [BuildId, "d2e3f4a5-b6c7-4d8e-9f0a-1b2c3d4e5f60"],
            cancellationToken: TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal("/v1/betaAppReviewSubmissions", request.RequestUri!.AbsolutePath);
        Assert.Contains(
            $"filter[build]={BuildId},d2e3f4a5-b6c7-4d8e-9f0a-1b2c3d4e5f60",
            Uri.UnescapeDataString(request.RequestUri.Query));
    }

    [Fact]
    public async Task SendsBetaGroupCreateBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.testflightBetaGroupCreated.json",
            HttpStatusCode.Created);

        var request = new BetaGroupCreateRequest
        {
            Data = new BetaGroupCreateRequestData
            {
                Attributes = new BetaGroupCreateRequestDataAttributes
                {
                    Name = "Release Candidates",
                    PublicLinkEnabled = true,
                    PublicLinkLimitEnabled = true,
                    PublicLinkLimit = 250,
                    FeedbackEnabled = true
                },
                Relationships = new BetaGroupCreateRequestDataRelationships
                {
                    App = RelationshipDeclaration.To("apps", "6446939457"),
                    Builds = RelationshipDeclarationList.To("builds", BuildId)
                }
            }
        };

        var response = await client.CreateBetaGroupAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/v1/betaGroups", handler.CapturedRequest.RequestUri!.AbsolutePath);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("betaGroups", data.GetProperty("type").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.Equal("Release Candidates", attributes.GetProperty("name").GetString());
        Assert.Equal(250, attributes.GetProperty("publicLinkLimit").GetInt32());
        Assert.False(attributes.TryGetProperty("isInternalGroup", out _));

        var relationships = data.GetProperty("relationships");
        Assert.Equal("6446939457", relationships.GetProperty("app").GetProperty("data").GetProperty("id").GetString());
        var builds = relationships.GetProperty("builds").GetProperty("data");
        Assert.Equal(1, builds.GetArrayLength());
        Assert.Equal(BuildId, builds[0].GetProperty("id").GetString());

        Assert.Equal("b0c1d2e3-f4a5-4b6c-8d7e-9f0a1b2c3d4e", response.Data.Id);
        Assert.Equal(250, response.Data.Attributes?.PublicLinkLimit);
    }

    private static void AssertLinkagePayload(string body, string expectedType, params string[] expectedIds)
    {
        using var document = JsonDocument.Parse(body);

        Assert.Single(document.RootElement.EnumerateObject());

        var data = document.RootElement.GetProperty("data");
        Assert.Equal(JsonValueKind.Array, data.ValueKind);
        Assert.Equal(expectedIds.Length, data.GetArrayLength());

        for (var index = 0; index < expectedIds.Length; index++)
        {
            var entry = data[index];
            Assert.Equal(expectedType, entry.GetProperty("type").GetString());
            Assert.Equal(expectedIds[index], entry.GetProperty("id").GetString());
            Assert.Equal(2, entry.EnumerateObject().Count());
        }
    }
}
