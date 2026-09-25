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

public class SubscriptionVersionTests
{
    private const string SubscriptionId = "6446671421";

    private const string VersionId = "7c2d9a41-5e8f-4b3a-9c61-2f4e8d7b1a05";

    private const string LocalizationId = "4a6c8e0f-2b4d-4f61-8a3c-5e7f9b1d3c22";

    private const string ImageId = "e3a5c7e9-1b3d-4f50-8c72-9e1a3c5e7b44";

    [Fact]
    public async Task CreatesAVersionThenAttachesALocalizationToIt()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.subscriptionversionResponse.json"),
                HttpStatusCode.Created)
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.subscriptionversionLocalizationResponse.json"),
                HttpStatusCode.Created);
        using var client = TestUtilities.CreateClient(handler);

        var version = await client.CreateSubscriptionVersionAsync(
            new SubscriptionVersionCreateRequest
            {
                Data = new SubscriptionVersionCreateRequestData
                {
                    Relationships = new SubscriptionVersionCreateRequestDataRelationships
                    {
                        Subscription = RelationshipDeclaration.To("subscriptions", SubscriptionId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(SubscriptionVersionState.PrepareForSubmission, version.Data.Attributes!.State);

        var localization = await client.CreateSubscriptionLocalizationV2Async(
            new SubscriptionLocalizationV2CreateRequest
            {
                Data = new SubscriptionLocalizationV2CreateRequestData
                {
                    Attributes = new SubscriptionLocalizationV2CreateRequestDataAttributes
                    {
                        Name = "All Access (Monthly)",
                        Locale = "en-US",
                        Description = "Unlimited lessons across every instrument."
                    },
                    Relationships = new SubscriptionLocalizationV2CreateRequestDataRelationships
                    {
                        Version = RelationshipDeclaration.To("subscriptionVersions", version.Data.Id)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        handler.AssertAllResponsesConsumed();
        Assert.Equal(2, handler.CapturedRequests.Count);

        var versionRequest = handler.CapturedRequests[0];
        Assert.Equal(HttpMethod.Post, versionRequest.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/subscriptionVersions",
            versionRequest.RequestUri!.OriginalString);

        using (var versionBody = JsonDocument.Parse(handler.CapturedRequestBodies[0]!))
        {
            var data = versionBody.RootElement.GetProperty("data");
            Assert.Equal("subscriptionVersions", data.GetProperty("type").GetString());
            Assert.False(data.TryGetProperty("id", out _));
            Assert.False(data.TryGetProperty("attributes", out _));

            var subscription = data.GetProperty("relationships").GetProperty("subscription").GetProperty("data");
            Assert.Equal("subscriptions", subscription.GetProperty("type").GetString());
            Assert.Equal(SubscriptionId, subscription.GetProperty("id").GetString());
        }

        var localizationRequest = handler.CapturedRequests[1];
        Assert.Equal(HttpMethod.Post, localizationRequest.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/subscriptionLocalizations",
            localizationRequest.RequestUri!.OriginalString);

        using (var localizationBody = JsonDocument.Parse(handler.CapturedRequestBodies[1]!))
        {
            var data = localizationBody.RootElement.GetProperty("data");
            Assert.Equal("subscriptionLocalizations", data.GetProperty("type").GetString());
            Assert.False(data.TryGetProperty("id", out _));

            var attributes = data.GetProperty("attributes");
            Assert.Equal("All Access (Monthly)", attributes.GetProperty("name").GetString());
            Assert.Equal("en-US", attributes.GetProperty("locale").GetString());
            Assert.Equal(
                "Unlimited lessons across every instrument.",
                attributes.GetProperty("description").GetString());

            var relationships = data.GetProperty("relationships");
            Assert.Equal(["version"], relationships.EnumerateObject().Select(property => property.Name));

            var versionLinkage = relationships.GetProperty("version").GetProperty("data");
            Assert.Equal("subscriptionVersions", versionLinkage.GetProperty("type").GetString());
            Assert.Equal(VersionId, versionLinkage.GetProperty("id").GetString());
        }

        Assert.Equal(LocalizationId, localization.Data.Id);
        Assert.Equal(VersionId, localization.Data.Relationships!["version"].ToOne()!.Id);
    }

    [Fact]
    public async Task ReadsAVersionAndDecodesIt()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionResponse.json");

        var response = await client.GetSubscriptionVersionAsync(
            VersionId,
            new AppStoreConnectQuery().Include("localizations", "images").Limit("localizations", 50),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        var url = Uri.UnescapeDataString(handler.CapturedRequest.RequestUri!.ToString());
        Assert.StartsWith($"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}?", url);
        Assert.Contains("include=localizations,images", url);
        Assert.Contains("limit[localizations]=50", url);

        var version = response.Data;
        Assert.Equal("subscriptionVersions", version.Type);
        Assert.Equal(VersionId, version.Id);
        Assert.Equal(2, version.Attributes!.Version);
        Assert.Equal(SubscriptionVersionState.PrepareForSubmission, version.Attributes.State);

        var subscription = version.Relationships!["subscription"].ToOne()!;
        Assert.Equal("subscriptions", subscription.Type);
        Assert.Equal(SubscriptionId, subscription.Id);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}/localizations",
            version.Relationships["localizations"].Links!.Related);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}",
            version.Links!.Self);
    }

    [Fact]
    public async Task ListsVersionsForASubscriptionWithTheirIncludedLocalizations()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionListResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("state", "PREPARE_FOR_SUBMISSION", "APPROVED")
            .Include("localizations");

        var response = await client.ListVersionsForSubscriptionAsync(
            SubscriptionId,
            query,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        var url = Uri.UnescapeDataString(handler.CapturedRequest.RequestUri!.ToString());
        Assert.StartsWith($"https://api.appstoreconnect.apple.com/v1/subscriptions/{SubscriptionId}/versions?", url);
        Assert.Contains("filter[state]=PREPARE_FOR_SUBMISSION,APPROVED", url);
        Assert.Contains("include=localizations", url);

        Assert.Equal(2, response.Data.Length);
        Assert.Equal(1, response.Data[0].Attributes!.Version);
        Assert.Equal(SubscriptionVersionState.Approved, response.Data[0].Attributes!.State);

        var draft = response.Data.Single(version => version.Attributes!.State == SubscriptionVersionState.PrepareForSubmission);
        Assert.Equal(VersionId, draft.Id);
        Assert.Equal(2, draft.Attributes!.Version);

        var linkage = Assert.Single(draft.Relationships!["localizations"].ToMany());
        Assert.Equal("subscriptionLocalizations", linkage.Type);

        Assert.True(response.TryGetIncluded<SubscriptionLocalizationV2>(linkage.Type, linkage.Id, out var localization));
        Assert.Equal(LocalizationId, localization.Id);
        Assert.Equal("en-US", localization.Attributes!.Locale);
        Assert.Equal("All Access (Monthly)", localization.Attributes.Name);
        Assert.Equal(
            "Unlimited lessons across every instrument, now with sheet music.",
            localization.Attributes.Description);
        Assert.Equal(VersionId, localization.Relationships!["version"].ToOne()!.Id);

        Assert.Equal(2, response.Meta!.Paging.Total);
    }

    [Fact]
    public async Task DecodesEveryVersionStateAndFallsBackToUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.subscriptionversionStatesResponse.json");

        var response = await client.ListVersionsForSubscriptionAsync(
            SubscriptionId,
            cancellationToken: TestContext.Current.CancellationToken);

        SubscriptionVersionState?[] expected =
        [
            SubscriptionVersionState.PrepareForSubmission,
            SubscriptionVersionState.ReadyForReview,
            SubscriptionVersionState.WaitingForReview,
            SubscriptionVersionState.InReview,
            SubscriptionVersionState.Accepted,
            SubscriptionVersionState.Approved,
            SubscriptionVersionState.ReplacedWithNewVersion,
            SubscriptionVersionState.Rejected,
            SubscriptionVersionState.DeveloperRejected,
            SubscriptionVersionState._Unmapped
        ];

        Assert.Equal(expected, response.Data.Select(version => version.Attributes!.State));
        Assert.Equal(10, response.Data[^1].Attributes!.Version);
    }

    [Fact]
    public async Task ListsLocalizationsForAVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionLocalizationsResponse.json");

        var response = await client.ListLocalizationsForSubscriptionVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}/localizations",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);
        Assert.Equal("subscriptionLocalizations", response.Data[0].Type);
        Assert.Equal("en-US", response.Data[0].Attributes!.Locale);
        Assert.Equal("fr-FR", response.Data[1].Attributes!.Locale);
        Assert.Equal("Accès illimité (mensuel)", response.Data[1].Attributes!.Name);
        Assert.Null(response.Data[1].Attributes!.Description);
        Assert.Equal(VersionId, response.Data[1].Relationships!["version"].ToOne()!.Id);
    }

    [Fact]
    public async Task ListsImagesForAVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionImagesResponse.json");

        var response = await client.ListImagesForSubscriptionVersionAsync(
            VersionId,
            new AppStoreConnectQuery().Limit(10),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}/images?limit=10",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var image = Assert.Single(response.Data);
        Assert.Equal("subscriptionImages", image.Type);
        Assert.Equal(ImageId, image.Id);
        Assert.Equal(245670L, image.Attributes!.FileSize);
        Assert.Equal("all-access-promo.png", image.Attributes.FileName);
        Assert.Equal(1024, image.Attributes.ImageAsset!.Width);
        Assert.Equal(1024, image.Attributes.ImageAsset.Height);
        Assert.Null(image.Attributes.UploadOperations);

        var delivery = image.Attributes.AssetDeliveryState!;
        Assert.Equal(AppMediaAssetStateState.Complete, delivery.State);
        Assert.Empty(delivery.Errors!);
        Assert.Equal("IMAGE_TOO_BRIGHT", Assert.Single(delivery.Warnings!).Code);
    }

    [Fact]
    public async Task ReadsTheImageOfAVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionImageResponse.json");

        var response = await client.GetImageForSubscriptionVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/subscriptionVersions/{VersionId}/image",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(ImageId, response.Data.Id);
        Assert.Equal(AppMediaAssetStateState.AwaitingUpload, response.Data.Attributes!.AssetDeliveryState!.State);
    }

    [Fact]
    public async Task ReadsALocalization()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionLocalizationResponse.json");

        var response = await client.GetSubscriptionLocalizationV2Async(
            LocalizationId,
            new AppStoreConnectQuery().Include("version"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionLocalizations/{LocalizationId}?include=version",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var localization = response.Data;
        Assert.Equal("subscriptionLocalizations", localization.Type);
        Assert.Equal("All Access (Monthly)", localization.Attributes!.Name);
        Assert.Equal("en-US", localization.Attributes.Locale);
        Assert.Equal("Unlimited lessons across every instrument.", localization.Attributes.Description);

        var version = localization.Relationships!["version"].ToOne()!;
        Assert.Equal("subscriptionVersions", version.Type);
        Assert.Equal(VersionId, version.Id);
    }

    [Fact]
    public async Task UpdatesALocalizationWithOnlyTheAttributesAssigned()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionLocalizationResponse.json");

        await client.UpdateSubscriptionLocalizationV2Async(
            LocalizationId,
            new SubscriptionLocalizationV2UpdateRequest
            {
                Data = new SubscriptionLocalizationV2UpdateRequestData
                {
                    Id = LocalizationId,
                    Attributes = new SubscriptionLocalizationV2UpdateRequestDataAttributes
                    {
                        Description = "Unlimited lessons, now with sheet music."
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptionLocalizations", data.GetProperty("type").GetString());
        Assert.Equal(LocalizationId, data.GetProperty("id").GetString());
        Assert.False(data.TryGetProperty("relationships", out _));

        var attributes = data.GetProperty("attributes");
        Assert.Equal(["description"], attributes.EnumerateObject().Select(property => property.Name));
        Assert.Equal("Unlimited lessons, now with sheet music.", attributes.GetProperty("description").GetString());
    }

    [Fact]
    public async Task DeletesALocalization()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteSubscriptionLocalizationV2Async(
            LocalizationId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }

    [Fact]
    public async Task ReservesAnImageOnAVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.subscriptionversionImageResponse.json",
            HttpStatusCode.Created);

        var response = await client.CreateSubscriptionImageV2Async(
            new SubscriptionImageV2CreateRequest
            {
                Data = new SubscriptionImageV2CreateRequestData
                {
                    Attributes = new SubscriptionImageV2CreateRequestDataAttributes
                    {
                        FileName = "all-access-promo.png",
                        FileSize = 245670
                    },
                    Relationships = new SubscriptionImageV2CreateRequestDataRelationships
                    {
                        Version = RelationshipDeclaration.To("subscriptionVersions", VersionId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/subscriptionImages",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using (var body = JsonDocument.Parse(handler.CapturedRequestBody!))
        {
            var data = body.RootElement.GetProperty("data");
            Assert.Equal("subscriptionImages", data.GetProperty("type").GetString());
            Assert.False(data.TryGetProperty("id", out _));

            var attributes = data.GetProperty("attributes");
            Assert.Equal("all-access-promo.png", attributes.GetProperty("fileName").GetString());
            Assert.Equal(245670, attributes.GetProperty("fileSize").GetInt64());

            var relationships = data.GetProperty("relationships");
            Assert.Equal(["version"], relationships.EnumerateObject().Select(property => property.Name));

            var version = relationships.GetProperty("version").GetProperty("data");
            Assert.Equal("subscriptionVersions", version.GetProperty("type").GetString());
            Assert.Equal(VersionId, version.GetProperty("id").GetString());
        }

        var image = response.Data;
        Assert.Equal("subscriptionImages", image.Type);
        Assert.Equal(ImageId, image.Id);
        Assert.Equal(245670L, image.Attributes!.FileSize);
        Assert.Equal("PurpleSource221/v4/aa/bb/cc/all-access-promo.png", image.Attributes.AssetToken);
        Assert.Null(image.Attributes.ImageAsset);
        Assert.Equal(AppMediaAssetStateState.AwaitingUpload, image.Attributes.AssetDeliveryState!.State);

        var operation = Assert.Single(image.Attributes.UploadOperations!);
        Assert.Equal("PUT", operation.Method);
        Assert.Equal(245670, operation.Length);
        Assert.Equal(0, operation.Offset);
        Assert.Equal("image/png", Assert.Single(operation.RequestHeaders!).Value);
    }

    [Fact]
    public async Task ReadsAnImage()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionImageResponse.json");

        var response = await client.GetSubscriptionImageV2Async(
            ImageId,
            new AppStoreConnectQuery().Fields("subscriptionImages", "fileName", "assetDeliveryState"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        var url = Uri.UnescapeDataString(handler.CapturedRequest.RequestUri!.ToString());
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionImages/{ImageId}?fields[subscriptionImages]=fileName,assetDeliveryState",
            url);

        Assert.Equal("all-access-promo.png", response.Data.Attributes!.FileName);
    }

    [Fact]
    public async Task CommitsAnImageWithOnlyTheUploadedFlag()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.subscriptionversionImageResponse.json");

        await client.UpdateSubscriptionImageV2Async(
            ImageId,
            new SubscriptionImageV2UpdateRequest
            {
                Data = new SubscriptionImageV2UpdateRequestData
                {
                    Id = ImageId,
                    Attributes = new SubscriptionImageV2UpdateRequestDataAttributes
                    {
                        Uploaded = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionImages/{ImageId}",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("subscriptionImages", data.GetProperty("type").GetString());
        Assert.Equal(ImageId, data.GetProperty("id").GetString());

        var attributes = data.GetProperty("attributes");
        Assert.Equal(["uploaded"], attributes.EnumerateObject().Select(property => property.Name));
        Assert.True(attributes.GetProperty("uploaded").GetBoolean());
    }

    [Fact]
    public async Task DeletesAnImage()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteSubscriptionImageV2Async(
            ImageId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/subscriptionImages/{ImageId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }
}
