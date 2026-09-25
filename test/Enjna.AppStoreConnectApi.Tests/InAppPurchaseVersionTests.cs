using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class InAppPurchaseVersionTests
{
    private const string InAppPurchaseId = "6446452615";

    private const string VersionId = "c5f0e2a1-7b3d-4e9a-8f16-2d4b6a8c0e13";

    private const string LocalizationId = "8a2d4f60-1c3e-4b5a-9d7f-6e8a0c2b4d19";

    private const string ImageId = "f1b3d5e7-9a2c-4e6f-8b0d-2c4e6a8b0d35";

    /// <summary>
    /// The <c>POST /v1/inAppPurchaseVersions</c> body from Apple's "Migrating in-app purchase
    /// metadata to v2" guide.
    /// </summary>
    private const string ApplesVersionCreatePayload = """
        {
          "data": {
            "type": "inAppPurchaseVersions",
            "relationships": {
              "inAppPurchase": {
                "data": { "type": "inAppPurchases", "id": "6446452615" }
              }
            }
          }
        }
        """;

    /// <summary>
    /// The <c>POST /v2/inAppPurchaseLocalizations</c> body from Apple's "Migrating in-app purchase
    /// metadata to v2" guide, with the placeholder for the version ID left in.
    /// </summary>
    private const string ApplesLocalizationCreatePayload = """
        {
          "data": {
            "type": "inAppPurchaseLocalizations",
            "attributes": {
              "locale": "en-US",
              "name": "Seattle Neighborhood Coffee Map",
              "description": "Find awesome coffee shops."
            },
            "relationships": {
              "version": {
                "data": { "type": "inAppPurchaseVersions", "id": "${inAppPurchaseVersionId}" }
              }
            }
          }
        }
        """;

    [Fact]
    public async Task CreatesVersionThenLocalizationWithApplesMigrationPayloads()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.iapversionCreated.json",
            HttpStatusCode.Created);
        handler.EnqueueJson(
            TestUtilities.ReadResourceAsString("models.iapversionLocalization.json"),
            HttpStatusCode.Created);

        var version = await client.CreateInAppPurchaseVersionAsync(
            new InAppPurchaseVersionCreateRequest
            {
                Data = new InAppPurchaseVersionCreateRequestData
                {
                    Relationships = new InAppPurchaseVersionCreateRequestDataRelationships
                    {
                        InAppPurchase = RelationshipDeclaration.To("inAppPurchases", InAppPurchaseId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(VersionId, version.Data.Id);
        Assert.Equal(1, version.Data.Attributes!.Version);
        Assert.Equal(InAppPurchaseVersionState.PrepareForSubmission, version.Data.Attributes.State);

        var localization = await client.CreateInAppPurchaseLocalizationV2Async(
            new InAppPurchaseLocalizationV2CreateRequest
            {
                Data = new InAppPurchaseLocalizationV2CreateRequestData
                {
                    Attributes = new InAppPurchaseLocalizationV2CreateRequestDataAttributes
                    {
                        Locale = "en-US",
                        Name = "Seattle Neighborhood Coffee Map",
                        Description = "Find awesome coffee shops."
                    },
                    Relationships = new InAppPurchaseLocalizationV2CreateRequestDataRelationships
                    {
                        Version = RelationshipDeclaration.To("inAppPurchaseVersions", version.Data.Id)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        handler.AssertAllResponsesConsumed();
        Assert.Equal(2, handler.CapturedRequests.Count);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequests[0].Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions",
            handler.CapturedRequests[0].RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(ApplesVersionCreatePayload, handler.CapturedRequestBodies[0]);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequests[1].Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchaseLocalizations",
            handler.CapturedRequests[1].RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(
            ApplesLocalizationCreatePayload.Replace("${inAppPurchaseVersionId}", VersionId),
            handler.CapturedRequestBodies[1]);

        using var body = JsonDocument.Parse(handler.CapturedRequestBodies[1]!);
        var relationships = body.RootElement.GetProperty("data").GetProperty("relationships");

        Assert.True(relationships.TryGetProperty("version", out _));
        Assert.False(relationships.TryGetProperty("inAppPurchaseV2", out _));

        Assert.Equal(LocalizationId, localization.Data.Id);
        Assert.Equal(VersionId, localization.Data.Relationships!["version"].ToOne()!.Id);
    }

    [Fact]
    public async Task ReadsVersionWithItsRelationships()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionResponse.json");

        var query = new AppStoreConnectQuery()
            .Include("localizations", "images")
            .Limit("localizations", 10);

        var response = await client.GetInAppPurchaseVersionAsync(
            VersionId,
            query,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}?include=localizations,images&limit[localizations]=10",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var version = response.Data;

        Assert.Equal("inAppPurchaseVersions", version.Type);
        Assert.Equal(VersionId, version.Id);
        Assert.Equal(1, version.Attributes!.Version);
        Assert.Equal(InAppPurchaseVersionState.WaitingForReview, version.Attributes.State);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}",
            version.Links!.Self);

        var relationships = version.Relationships!;
        var owner = relationships["inAppPurchase"].ToOne()!;

        Assert.Equal("inAppPurchases", owner.Type);
        Assert.Equal(InAppPurchaseId, owner.Id);

        var image = relationships["image"].ToOne()!;

        Assert.Equal("inAppPurchaseImages", image.Type);
        Assert.Equal(ImageId, image.Id);
        Assert.Equal(ImageId, Assert.Single(relationships["images"].ToMany()).Id);

        var localizations = relationships["localizations"];
        var linkedLocalizations = localizations.ToMany();

        Assert.Equal(2, linkedLocalizations.Length);
        Assert.Equal("inAppPurchaseLocalizations", linkedLocalizations[0].Type);
        Assert.Equal(LocalizationId, linkedLocalizations[0].Id);
        Assert.Equal(2, localizations.Meta!.Paging.Total);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}/localizations",
            localizations.Links!.Related);
    }

    [Theory]
    [InlineData("PREPARE_FOR_SUBMISSION", InAppPurchaseVersionState.PrepareForSubmission)]
    [InlineData("READY_FOR_REVIEW", InAppPurchaseVersionState.ReadyForReview)]
    [InlineData("WAITING_FOR_REVIEW", InAppPurchaseVersionState.WaitingForReview)]
    [InlineData("IN_REVIEW", InAppPurchaseVersionState.InReview)]
    [InlineData("ACCEPTED", InAppPurchaseVersionState.Accepted)]
    [InlineData("APPROVED", InAppPurchaseVersionState.Approved)]
    [InlineData("REPLACED_WITH_NEW_VERSION", InAppPurchaseVersionState.ReplacedWithNewVersion)]
    [InlineData("REJECTED", InAppPurchaseVersionState.Rejected)]
    [InlineData("DEVELOPER_REJECTED", InAppPurchaseVersionState.DeveloperRejected)]
    public async Task DecodesEveryVersionState(string wireValue, InAppPurchaseVersionState expected)
    {
        var (client, _) = TestUtilities.GetClientWithBody(VersionBody(wireValue));

        var response = await client.GetInAppPurchaseVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(expected, response.Data.Attributes!.State);
    }

    [Fact]
    public async Task DecodesUnknownVersionStateAsUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithBody(VersionBody("WITHDRAWN_FOR_AUDIT"));

        var response = await client.GetInAppPurchaseVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(InAppPurchaseVersionState._Unmapped, response.Data.Attributes!.State);
        Assert.Equal(3, response.Data.Attributes.Version);
    }

    [Fact]
    public async Task ListsDraftVersionsWithTheirIncludedLocalizations()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionListResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("state", "PREPARE_FOR_SUBMISSION")
            .Include("localizations");

        var response = await client.ListVersionsForInAppPurchaseAsync(
            InAppPurchaseId,
            query,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchases/{InAppPurchaseId}/versions?filter[state]=PREPARE_FOR_SUBMISSION&include=localizations",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var draft = Assert.Single(response.Data);

        Assert.Equal("inAppPurchaseVersions", draft.Type);
        Assert.Equal(2, draft.Attributes!.Version);
        Assert.Equal(InAppPurchaseVersionState.PrepareForSubmission, draft.Attributes.State);

        var linked = Assert.Single(draft.Relationships!["localizations"].ToMany());

        Assert.True(response.TryGetIncluded<InAppPurchaseLocalizationV2>(
            "inAppPurchaseLocalizations",
            linked.Id,
            out var localization));
        Assert.Equal("Seattle Neighborhood Coffee Map", localization.Attributes!.Name);
        Assert.Equal("en-US", localization.Attributes.Locale);
        Assert.Equal("Find awesome coffee shops.", localization.Attributes.Description);

        var owner = localization.Relationships!["version"].ToOne()!;

        Assert.Equal("inAppPurchaseVersions", owner.Type);
        Assert.Equal(draft.Id, owner.Id);

        Assert.Equal(1, response.Meta!.Paging.Total);
    }

    [Fact]
    public async Task ListsLocalizationsOfVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionLocalizations.json");

        var response = await client.ListLocalizationsForInAppPurchaseVersionAsync(
            VersionId,
            new AppStoreConnectQuery().Limit(2),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}/localizations?limit=2",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(2, response.Data.Length);

        var english = response.Data[0];

        Assert.Equal("inAppPurchaseLocalizations", english.Type);
        Assert.Equal(LocalizationId, english.Id);
        Assert.Equal("en-US", english.Attributes!.Locale);
        Assert.Equal(VersionId, english.Relationships!["version"].ToOne()!.Id);

        Assert.Equal("fr-FR", response.Data[1].Attributes!.Locale);
        Assert.Equal("Trouvez des cafés formidables.", response.Data[1].Attributes!.Description);
        Assert.Equal(2, response.Meta!.Paging.Total);
    }

    [Fact]
    public async Task DecodesV2LocalizationWithVersionRelationship()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionLocalization.json");

        var response = await client.GetInAppPurchaseLocalizationV2Async(
            LocalizationId,
            new AppStoreConnectQuery().Include("version"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseLocalizations/{LocalizationId}?include=version",
            handler.CapturedRequest.RequestUri!.OriginalString);

        InAppPurchaseLocalizationV2 localization = response.Data;

        Assert.Equal("inAppPurchaseLocalizations", localization.Type);
        Assert.Equal(LocalizationId, localization.Id);
        Assert.Equal("Seattle Neighborhood Coffee Map", localization.Attributes!.Name);
        Assert.Equal("en-US", localization.Attributes.Locale);
        Assert.Equal("Find awesome coffee shops.", localization.Attributes.Description);

        var version = localization.Relationships!["version"].ToOne()!;

        Assert.Equal("inAppPurchaseVersions", version.Type);
        Assert.Equal(VersionId, version.Id);
        Assert.False(localization.Relationships.ContainsKey("inAppPurchaseV2"));
    }

    [Fact]
    public async Task SendsOnlyAssignedAttributesWhenUpdatingV2Localization()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionLocalization.json");

        var request = new InAppPurchaseLocalizationV2UpdateRequest
        {
            Data = new InAppPurchaseLocalizationV2UpdateRequestData
            {
                Id = LocalizationId,
                Attributes = new InAppPurchaseLocalizationV2UpdateRequestDataAttributes
                {
                    Name = "Seattle Coffee Map"
                }
            }
        };

        await client.UpdateInAppPurchaseLocalizationV2Async(
            LocalizationId,
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(
            $$"""
            {
              "data": {
                "type": "inAppPurchaseLocalizations",
                "id": "{{LocalizationId}}",
                "attributes": { "name": "Seattle Coffee Map" }
              }
            }
            """,
            handler.CapturedRequestBody);
    }

    [Fact]
    public async Task DeletesV2LocalizationWithNoContent()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteInAppPurchaseLocalizationV2Async(
            LocalizationId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseLocalizations/{LocalizationId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }

    [Fact]
    public async Task ListsImagesOfVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionImages.json");

        var response = await client.ListImagesForInAppPurchaseVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}/images",
            handler.CapturedRequest.RequestUri!.OriginalString);

        var image = Assert.Single(response.Data);

        Assert.Equal("inAppPurchaseImages", image.Type);
        Assert.Equal(ImageId, image.Id);
        Assert.Equal("coffee-map-promo.png", image.Attributes!.FileName);
        Assert.Equal(245670L, image.Attributes.FileSize);
        Assert.Equal("PurpleSource221/v4/f1/b3/d5/coffee-map-promo.png", image.Attributes.AssetToken);
        Assert.Equal(1024, image.Attributes.ImageAsset!.Width);
        Assert.Equal(1024, image.Attributes.ImageAsset.Height);
        Assert.Equal(AppMediaAssetStateState.Complete, image.Attributes.AssetDeliveryState!.State);
        Assert.Empty(image.Attributes.AssetDeliveryState.Errors!);
        Assert.Null(image.Attributes.UploadOperations);
    }

    [Fact]
    public async Task ReadsImageOfVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionImage.json");

        var response = await client.GetImageForInAppPurchaseVersionAsync(
            VersionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{VersionId}/image",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal(ImageId, response.Data.Id);
        Assert.Equal(AppMediaAssetStateState.UploadComplete, response.Data.Attributes!.AssetDeliveryState!.State);
    }

    [Fact]
    public async Task ReservesV2ImageOnVersion()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.iapversionImageReserved.json",
            HttpStatusCode.Created);

        var request = new InAppPurchaseImageV2CreateRequest
        {
            Data = new InAppPurchaseImageV2CreateRequestData
            {
                Attributes = new InAppPurchaseImageV2CreateRequestDataAttributes
                {
                    FileName = "coffee-map-promo.png",
                    FileSize = 245670
                },
                Relationships = new InAppPurchaseImageV2CreateRequestDataRelationships
                {
                    Version = RelationshipDeclaration.To("inAppPurchaseVersions", VersionId)
                }
            }
        };

        var response = await client.CreateInAppPurchaseImageV2Async(
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v2/inAppPurchaseImages",
            handler.CapturedRequest.RequestUri!.OriginalString);

        // The body of Apple's "Working with in-app purchase versions" guide.
        TestUtilities.AssertJsonEquivalent(
            $$"""
            {
              "data": {
                "type": "inAppPurchaseImages",
                "attributes": {
                  "fileName": "coffee-map-promo.png",
                  "fileSize": 245670
                },
                "relationships": {
                  "version": {
                    "data": {
                      "type": "inAppPurchaseVersions",
                      "id": "{{VersionId}}"
                    }
                  }
                }
              }
            }
            """,
            handler.CapturedRequestBody);

        var attributes = response.Data.Attributes!;
        var operation = Assert.Single(attributes.UploadOperations!);

        Assert.Equal(ImageId, response.Data.Id);
        Assert.Equal("PUT", operation.Method);
        Assert.Equal(245670, operation.Length);
        Assert.Equal(0, operation.Offset);
        Assert.Equal("image/png", Assert.Single(operation.RequestHeaders!).Value);
        Assert.Equal(AppMediaAssetStateState.AwaitingUpload, attributes.AssetDeliveryState!.State);
    }

    [Fact]
    public async Task CommitsV2ImageWithOnlyTheUploadedFlag()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionImage.json");

        var request = new InAppPurchaseImageV2UpdateRequest
        {
            Data = new InAppPurchaseImageV2UpdateRequestData
            {
                Id = ImageId,
                Attributes = new InAppPurchaseImageV2UpdateRequestDataAttributes
                {
                    Uploaded = true
                }
            }
        };

        var response = await client.UpdateInAppPurchaseImageV2Async(
            ImageId,
            request,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseImages/{ImageId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(
            $$"""
            {
              "data": {
                "type": "inAppPurchaseImages",
                "id": "{{ImageId}}",
                "attributes": { "uploaded": true }
              }
            }
            """,
            handler.CapturedRequestBody);

        Assert.Equal(AppMediaAssetStateState.UploadComplete, response.Data.Attributes!.AssetDeliveryState!.State);
    }

    [Fact]
    public async Task ReadsV2Image()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.iapversionImage.json");

        var response = await client.GetInAppPurchaseImageV2Async(
            ImageId,
            new AppStoreConnectQuery().Fields("inAppPurchaseImages", "fileName", "assetDeliveryState"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Get, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseImages/{ImageId}?fields[inAppPurchaseImages]=fileName,assetDeliveryState",
            handler.CapturedRequest.RequestUri!.OriginalString);

        Assert.Equal("inAppPurchaseImages", response.Data.Type);
        Assert.Equal("coffee-map-promo.png", response.Data.Attributes!.FileName);
        Assert.Equal(
            "https://is1-ssl.mzstatic.com/image/thumb/PurpleSource221/v4/f1/b3/d5/coffee-map-promo.png/{w}x{h}bb.{f}",
            response.Data.Attributes.ImageAsset!.TemplateUrl);
    }

    [Fact]
    public async Task DeletesV2ImageWithNoContent()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteInAppPurchaseImageV2Async(
            ImageId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v2/inAppPurchaseImages/{ImageId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }

    private static string VersionBody(string state) => $$"""
        {
          "data": {
            "type": "inAppPurchaseVersions",
            "id": "{{VersionId}}",
            "attributes": {
              "version": 3,
              "state": "{{state}}"
            },
            "links": {
              "self": "https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{{VersionId}}"
            }
          },
          "links": {
            "self": "https://api.appstoreconnect.apple.com/v1/inAppPurchaseVersions/{{VersionId}}"
          }
        }
        """;
}
