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

public class ReviewSubmissionTests
{
    private const string AppId = "6446148572";

    private const string ReviewSubmissionId = "fda9bd85-170b-4a1c-8d78-c2b445527542";

    private const string ReviewSubmissionItemId = "7c2e9f41-0a3b-4d5c-8e6f-9a0b1c2d3e4f";

    private const string InAppPurchaseVersionId = "abc123-4567-89ab-cdef-000000000001";

    // The three request bodies Apple's "Migrating in-app purchase metadata to v2" guide sends to
    // submit an in-app purchase version, copied as the guide prints them.
    private const string AppleCreateReviewSubmissionBody = """
        {
          "data": {
            "type": "reviewSubmissions",
            "attributes": { "platform": "IOS" },
            "relationships": {
              "app": { "data": { "type": "apps", "id": "6446148572" } }
            }
          }
        }
        """;

    private const string AppleCreateReviewSubmissionItemBody = """
        {
          "data": {
            "type": "reviewSubmissionItems",
            "relationships": {
              "reviewSubmission": {
                "data": { "type": "reviewSubmissions", "id": "${reviewSubmissionId}" }
              },
              "inAppPurchaseVersion": {
                "data": { "type": "inAppPurchaseVersions", "id": "${inAppPurchaseVersionId}" }
              }
            }
          }
        }
        """;

    private const string AppleSubmitReviewSubmissionBody = """
        {
          "data": {
            "type": "reviewSubmissions",
            "id": "${reviewSubmissionId}",
            "attributes": { "submitted": true }
          }
        }
        """;

    [Fact]
    public async Task WalksApplesThreeStepSubmissionFlow()
    {
        var handler = new TestHttpMessageHandler()
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.reviewsubmissionCreatedResponse.json"),
                HttpStatusCode.Created)
            .EnqueueJson(
                TestUtilities.ReadResourceAsString("models.reviewsubmissionItemCreatedResponse.json"),
                HttpStatusCode.Created)
            .EnqueueJson(TestUtilities.ReadResourceAsString("models.reviewsubmissionSubmittedResponse.json"));
        var client = TestUtilities.CreateClient(handler);

        var submission = await client.CreateReviewSubmissionAsync(
            new ReviewSubmissionCreateRequest
            {
                Data = new ReviewSubmissionCreateRequestData
                {
                    Attributes = new ReviewSubmissionCreateRequestDataAttributes
                    {
                        Platform = Platform.Ios
                    },
                    Relationships = new ReviewSubmissionCreateRequestDataRelationships
                    {
                        App = RelationshipDeclaration.To("apps", AppId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        var reviewSubmissionId = submission.Data.Id;

        var item = await client.CreateReviewSubmissionItemAsync(
            new ReviewSubmissionItemCreateRequest
            {
                Data = new ReviewSubmissionItemCreateRequestData
                {
                    Relationships = new ReviewSubmissionItemCreateRequestDataRelationships
                    {
                        ReviewSubmission = RelationshipDeclaration.To("reviewSubmissions", reviewSubmissionId),
                        InAppPurchaseVersion = RelationshipDeclaration.To("inAppPurchaseVersions", InAppPurchaseVersionId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        var submitted = await client.UpdateReviewSubmissionAsync(
            reviewSubmissionId,
            new ReviewSubmissionUpdateRequest
            {
                Data = new ReviewSubmissionUpdateRequestData
                {
                    Id = reviewSubmissionId,
                    Attributes = new ReviewSubmissionUpdateRequestDataAttributes
                    {
                        Submitted = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        handler.AssertAllResponsesConsumed();
        Assert.Equal(3, handler.CapturedRequests.Count);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequests[0].Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/reviewSubmissions",
            handler.CapturedRequests[0].RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(AppleCreateReviewSubmissionBody, handler.CapturedRequestBodies[0]);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequests[1].Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/reviewSubmissionItems",
            handler.CapturedRequests[1].RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(
            AppleCreateReviewSubmissionItemBody
                .Replace("${reviewSubmissionId}", reviewSubmissionId)
                .Replace("${inAppPurchaseVersionId}", InAppPurchaseVersionId),
            handler.CapturedRequestBodies[1]);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequests[2].Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissions/{reviewSubmissionId}",
            handler.CapturedRequests[2].RequestUri!.OriginalString);
        TestUtilities.AssertJsonEquivalent(
            AppleSubmitReviewSubmissionBody.Replace("${reviewSubmissionId}", reviewSubmissionId),
            handler.CapturedRequestBodies[2]);

        Assert.Equal(ReviewSubmissionState.ReadyForReview, submission.Data.Attributes!.State);
        Assert.Null(submission.Data.Attributes.SubmittedDate);
        Assert.Equal(ReviewSubmissionItemState.ReadyForReview, item.Data.Attributes!.State);
        Assert.Equal(ReviewSubmissionState.WaitingForReview, submitted.Data.Attributes!.State);
        Assert.Equal(
            new DateTimeOffset(2024, 6, 18, 11, 42, 7, TimeSpan.FromHours(-7)),
            submitted.Data.Attributes.SubmittedDate);
    }

    [Fact]
    public async Task SendsTheAppAsTheRequiredFilterWhenListingReviewSubmissions()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionListResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("state", "WAITING_FOR_REVIEW", "IN_REVIEW")
            .Include("items")
            .Limit(50);

        await client.ListReviewSubmissionsAsync(AppId, query, TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1/reviewSubmissions", request.RequestUri!.AbsolutePath);

        var queryString = DecodedQuery(handler);
        Assert.Contains($"filter[app]={AppId}", queryString);
        Assert.Contains("filter[state]=WAITING_FOR_REVIEW,IN_REVIEW", queryString);
        Assert.Contains("include=items", queryString);
        Assert.Contains("limit=50", queryString);
    }

    [Fact]
    public async Task SendsOnlyTheAppFilterWhenListingReviewSubmissionsWithoutAQuery()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionListResponse.json");

        await client.ListReviewSubmissionsAsync(AppId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissions?filter[app]={AppId}",
            Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.AbsoluteUri));
    }

    [Fact]
    public async Task RejectsAQueryThatSetsADifferentAppFilter()
    {
        var handler = new TestHttpMessageHandler();
        var client = TestUtilities.CreateClient(handler);

        var query = new AppStoreConnectQuery().Filter("app", "6446999999");

        await Assert.ThrowsAsync<ArgumentException>(() => client.ListReviewSubmissionsAsync(
            AppId,
            query,
            TestContext.Current.CancellationToken));

        Assert.Empty(handler.CapturedRequests);
    }

    [Fact]
    public async Task AcceptsAQueryThatRepeatsTheAppFilterWithTheSameValue()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionListResponse.json");

        var query = new AppStoreConnectQuery().Filter("app", AppId);

        await client.ListReviewSubmissionsAsync(AppId, query, TestContext.Current.CancellationToken);

        Assert.Contains($"filter[app]={AppId}", DecodedQuery(handler));
    }

    [Fact]
    public async Task DecodesEveryReviewSubmissionState()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.reviewsubmissionListResponse.json");

        var response = await client.ListReviewSubmissionsAsync(
            AppId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            [
                ReviewSubmissionState.ReadyForReview,
                ReviewSubmissionState.WaitingForReview,
                ReviewSubmissionState.InReview,
                ReviewSubmissionState.UnresolvedIssues,
                ReviewSubmissionState.Canceling,
                ReviewSubmissionState.Completing,
                ReviewSubmissionState.Complete
            ],
            response.Data.Select(submission => submission.Attributes!.State!.Value));

        Assert.Equal(
            [
                Platform.Ios,
                Platform.Ios,
                Platform.MacOs,
                Platform.Ios,
                Platform.TvOs,
                Platform.VisionOs,
                Platform.Ios
            ],
            response.Data.Select(submission => submission.Attributes!.Platform!.Value));

        var ready = response.Data[0];

        Assert.Equal("reviewSubmissions", ready.Type);
        Assert.Equal(ReviewSubmissionId, ready.Id);
        Assert.Null(ready.Attributes!.SubmittedDate);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissions/{ReviewSubmissionId}/items",
            ready.Relationships!["items"].Links!.Related);

        Assert.Equal(
            new DateTimeOffset(2024, 6, 17, 9, 15, 33, TimeSpan.FromHours(-7)),
            response.Data[2].Attributes!.SubmittedDate);
        Assert.Equal(7, response.Meta!.Paging.Total);
    }

    [Fact]
    public async Task DecodesApplesExampleOfTheReviewSubmissionsOfAnApp()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionForAppResponse.json");

        var response = await client.ListReviewSubmissionsForAppAsync(
            "6446998023",
            new AppStoreConnectQuery().Filter("platform", "IOS"),
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1/apps/6446998023/reviewSubmissions", request.RequestUri!.AbsolutePath);
        Assert.Equal("filter[platform]=IOS", DecodedQuery(handler).TrimStart('?'));

        var submission = Assert.Single(response.Data);

        Assert.Equal("reviewSubmissions", submission.Type);
        Assert.Equal(ReviewSubmissionId, submission.Id);
        Assert.Equal(Platform.Ios, submission.Attributes!.Platform);
        Assert.Null(submission.Attributes.SubmittedDate);
        Assert.Equal(ReviewSubmissionState.ReadyForReview, submission.Attributes.State);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissions/{ReviewSubmissionId}/relationships/items",
            submission.Relationships!["items"].Links!.Self);
        Assert.False(submission.Relationships["items"].IncludesLinkage);
        Assert.Equal(1, response.Meta!.Paging.Total);
        Assert.Equal(50, response.Meta.Paging.Limit);
    }

    [Fact]
    public async Task LeavesAttributesOutOfTheCreateBodyWhenNoPlatformIsGiven()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.reviewsubmissionCreatedResponse.json",
            HttpStatusCode.Created);

        await client.CreateReviewSubmissionAsync(
            new ReviewSubmissionCreateRequest
            {
                Data = new ReviewSubmissionCreateRequestData
                {
                    Relationships = new ReviewSubmissionCreateRequestDataRelationships
                    {
                        App = RelationshipDeclaration.To("apps", AppId)
                    }
                }
            },
            TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("reviewSubmissions", data.GetProperty("type").GetString());
        Assert.False(data.TryGetProperty("attributes", out _));
        Assert.False(data.TryGetProperty("id", out _));

        var app = data.GetProperty("relationships").GetProperty("app").GetProperty("data");

        Assert.Equal("apps", app.GetProperty("type").GetString());
        Assert.Equal(AppId, app.GetProperty("id").GetString());
    }

    [Fact]
    public async Task ReadsAReviewSubmissionWithItsRelationshipsAndIncludedItems()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionResponse.json");

        var response = await client.GetReviewSubmissionAsync(
            ReviewSubmissionId,
            new AppStoreConnectQuery().Include("items").Limit("items", 10),
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal($"/v1/reviewSubmissions/{ReviewSubmissionId}", request.RequestUri!.AbsolutePath);

        var queryString = DecodedQuery(handler);
        Assert.Contains("include=items", queryString);
        Assert.Contains("limit[items]=10", queryString);

        var submission = response.Data;

        Assert.Equal(ReviewSubmissionState.InReview, submission.Attributes!.State);
        Assert.Equal(Platform.Ios, submission.Attributes.Platform);
        Assert.Equal(
            new DateTimeOffset(2024, 6, 18, 11, 42, 7, TimeSpan.FromHours(-7)),
            submission.Attributes.SubmittedDate);

        var relationships = submission.Relationships!;

        Assert.Equal(AppId, relationships["app"].ToOne()!.Id);
        Assert.Equal("appStoreVersions", relationships["appStoreVersionForReview"].ToOne()!.Type);
        Assert.Equal("actors", relationships["submittedByActor"].ToOne()!.Type);
        Assert.Equal("API_KEY-2X9R4HXF34", relationships["lastUpdatedByActor"].ToOne()!.Id);

        var linkedItem = Assert.Single(relationships["items"].ToMany());
        Assert.Equal("reviewSubmissionItems", linkedItem.Type);
        Assert.Equal(ReviewSubmissionItemId, linkedItem.Id);
        Assert.Equal(1, relationships["items"].Meta!.Paging.Total);

        Assert.True(response.TryGetIncluded<ReviewSubmissionItem>(
            "reviewSubmissionItems",
            ReviewSubmissionItemId,
            out var includedItem));
        Assert.Equal(ReviewSubmissionItemState.Accepted, includedItem.Attributes!.State);
        Assert.Equal(
            InAppPurchaseVersionId,
            includedItem.Relationships!["inAppPurchaseVersion"].ToOne()!.Id);
    }

    [Fact]
    public async Task SendsOnlyTheCanceledAttributeWhenCancelingAReviewSubmission()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionSubmittedResponse.json");

        await client.UpdateReviewSubmissionAsync(
            ReviewSubmissionId,
            new ReviewSubmissionUpdateRequest
            {
                Data = new ReviewSubmissionUpdateRequestData
                {
                    Id = ReviewSubmissionId,
                    Attributes = new ReviewSubmissionUpdateRequestDataAttributes
                    {
                        Canceled = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissions/{ReviewSubmissionId}",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("reviewSubmissions", data.GetProperty("type").GetString());
        Assert.Equal(ReviewSubmissionId, data.GetProperty("id").GetString());
        Assert.Equal("{\"canceled\":true}", data.GetProperty("attributes").GetRawText());
    }

    [Fact]
    public async Task SendsThePlatformWhenItIsAddedToAReviewSubmission()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionSubmittedResponse.json");

        await client.UpdateReviewSubmissionAsync(
            ReviewSubmissionId,
            new ReviewSubmissionUpdateRequest
            {
                Data = new ReviewSubmissionUpdateRequestData
                {
                    Id = ReviewSubmissionId,
                    Attributes = new ReviewSubmissionUpdateRequestDataAttributes
                    {
                        Platform = Platform.VisionOs,
                        Submitted = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);

        Assert.Equal(
            "{\"platform\":\"VISION_OS\",\"submitted\":true}",
            body.RootElement.GetProperty("data").GetProperty("attributes").GetRawText());
    }

    [Fact]
    public async Task DecodesEveryReviewSubmissionItemState()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionItemsResponse.json");

        var response = await client.ListItemsForReviewSubmissionAsync(
            ReviewSubmissionId,
            new AppStoreConnectQuery().Include(
                "inAppPurchaseVersion",
                "subscriptionVersion",
                "subscriptionGroupVersion",
                "appStoreVersion",
                "appEvent"),
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal($"/v1/reviewSubmissions/{ReviewSubmissionId}/items", request.RequestUri!.AbsolutePath);
        Assert.Contains(
            "include=inAppPurchaseVersion,subscriptionVersion,subscriptionGroupVersion,appStoreVersion,appEvent",
            DecodedQuery(handler));

        Assert.Equal(
            [
                ReviewSubmissionItemState.ReadyForReview,
                ReviewSubmissionItemState.Accepted,
                ReviewSubmissionItemState.Approved,
                ReviewSubmissionItemState.Rejected,
                ReviewSubmissionItemState.Removed
            ],
            response.Data.Select(item => item.Attributes!.State!.Value));

        Assert.All(response.Data, item => Assert.Equal("reviewSubmissionItems", item.Type));

        Assert.Equal(
            ["inAppPurchaseVersions", "subscriptionVersions", "subscriptionGroupVersions", "appStoreVersions", "appEvents"],
            response.Data.Select(item => item.Relationships!.Values.Single(relationship => relationship.IncludesLinkage).ToOne()!.Type));
        Assert.Equal(
            InAppPurchaseVersionId,
            response.Data[0].Relationships!["inAppPurchaseVersion"].ToOne()!.Id);
        Assert.Equal(5, response.Meta!.Paging.Total);
    }

    [Theory]
    [InlineData("appStoreVersion", "appStoreVersions")]
    [InlineData("appCustomProductPageVersion", "appCustomProductPageVersions")]
    [InlineData("appStoreVersionExperiment", "appStoreVersionExperiments")]
    [InlineData("appStoreVersionExperimentV2", "appStoreVersionExperiments")]
    [InlineData("appEvent", "appEvents")]
    [InlineData("backgroundAssetVersion", "backgroundAssetVersions")]
    [InlineData("gameCenterAchievementVersion", "gameCenterAchievementVersions")]
    [InlineData("gameCenterActivityVersion", "gameCenterActivityVersions")]
    [InlineData("gameCenterChallengeVersion", "gameCenterChallengeVersions")]
    [InlineData("gameCenterLeaderboardSetVersion", "gameCenterLeaderboardSetVersions")]
    [InlineData("gameCenterLeaderboardVersion", "gameCenterLeaderboardVersions")]
    [InlineData("inAppPurchaseVersion", "inAppPurchaseVersions")]
    [InlineData("subscriptionVersion", "subscriptionVersions")]
    [InlineData("subscriptionGroupVersion", "subscriptionGroupVersions")]
    public async Task SendsTheSubmissionAndTheOneThingAnItemNames(string relationshipName, string resourceType)
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.reviewsubmissionItemCreatedResponse.json",
            HttpStatusCode.Created);

        var relationships = new ReviewSubmissionItemCreateRequestDataRelationships
        {
            ReviewSubmission = RelationshipDeclaration.To("reviewSubmissions", ReviewSubmissionId)
        };
        SetItemRelationship(relationships, relationshipName, RelationshipDeclaration.To(resourceType, "target-id"));

        await client.CreateReviewSubmissionItemAsync(
            new ReviewSubmissionItemCreateRequest
            {
                Data = new ReviewSubmissionItemCreateRequestData
                {
                    Relationships = relationships
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal(
            "https://api.appstoreconnect.apple.com/v1/reviewSubmissionItems",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("reviewSubmissionItems", data.GetProperty("type").GetString());
        Assert.False(data.TryGetProperty("id", out _));
        Assert.False(data.TryGetProperty("attributes", out _));

        var sent = data.GetProperty("relationships");

        Assert.Equal(
            ["reviewSubmission", relationshipName],
            sent.EnumerateObject().Select(property => property.Name));

        var submission = sent.GetProperty("reviewSubmission").GetProperty("data");
        Assert.Equal("reviewSubmissions", submission.GetProperty("type").GetString());
        Assert.Equal(ReviewSubmissionId, submission.GetProperty("id").GetString());

        var target = sent.GetProperty(relationshipName).GetProperty("data");
        Assert.Equal(resourceType, target.GetProperty("type").GetString());
        Assert.Equal("target-id", target.GetProperty("id").GetString());
    }

    [Fact]
    public async Task SendsOnlyTheResolvedAttributeWhenUpdatingAReviewSubmissionItem()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionItemCreatedResponse.json");

        var response = await client.UpdateReviewSubmissionItemAsync(
            ReviewSubmissionItemId,
            new ReviewSubmissionItemUpdateRequest
            {
                Data = new ReviewSubmissionItemUpdateRequestData
                {
                    Id = ReviewSubmissionItemId,
                    Attributes = new ReviewSubmissionItemUpdateRequestDataAttributes
                    {
                        Resolved = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissionItems/{ReviewSubmissionItemId}",
            handler.CapturedRequest.RequestUri!.OriginalString);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal("reviewSubmissionItems", data.GetProperty("type").GetString());
        Assert.Equal(ReviewSubmissionItemId, data.GetProperty("id").GetString());
        Assert.Equal("{\"resolved\":true}", data.GetProperty("attributes").GetRawText());

        Assert.Equal(ReviewSubmissionItemId, response.Data.Id);
    }

    [Fact]
    public async Task SendsTheRemovedAttributeWhenUpdatingAReviewSubmissionItem()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewsubmissionItemCreatedResponse.json");

        await client.UpdateReviewSubmissionItemAsync(
            ReviewSubmissionItemId,
            new ReviewSubmissionItemUpdateRequest
            {
                Data = new ReviewSubmissionItemUpdateRequestData
                {
                    Id = ReviewSubmissionItemId,
                    Attributes = new ReviewSubmissionItemUpdateRequestDataAttributes
                    {
                        Removed = true
                    }
                }
            },
            TestContext.Current.CancellationToken);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);

        Assert.Equal(
            "{\"removed\":true}",
            body.RootElement.GetProperty("data").GetProperty("attributes").GetRawText());
    }

    [Fact]
    public async Task DeletesAReviewSubmissionItemWithAnEmptyResponse()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteReviewSubmissionItemAsync(
            ReviewSubmissionItemId,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Delete, handler.CapturedRequest!.Method);
        Assert.Equal(
            $"https://api.appstoreconnect.apple.com/v1/reviewSubmissionItems/{ReviewSubmissionItemId}",
            handler.CapturedRequest.RequestUri!.OriginalString);
        Assert.Null(handler.CapturedRequestBody);
    }

    [Fact]
    public async Task DecodesAnUnknownReviewSubmissionStateAsUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.reviewsubmissionUnknownStateResponse.json");

        var response = await client.GetReviewSubmissionAsync(
            ReviewSubmissionId,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(ReviewSubmissionState._Unmapped, response.Data.Attributes!.State);
        Assert.Equal(Platform.Ios, response.Data.Attributes.Platform);
    }

    [Fact]
    public async Task DecodesAnUnknownReviewSubmissionItemStateAsUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.reviewsubmissionItemUnknownStateResponse.json");

        var response = await client.UpdateReviewSubmissionItemAsync(
            ReviewSubmissionItemId,
            new ReviewSubmissionItemUpdateRequest
            {
                Data = new ReviewSubmissionItemUpdateRequestData
                {
                    Id = ReviewSubmissionItemId
                }
            },
            TestContext.Current.CancellationToken);

        Assert.Equal(ReviewSubmissionItemState._Unmapped, response.Data.Attributes!.State);
    }

    private static void SetItemRelationship(
        ReviewSubmissionItemCreateRequestDataRelationships relationships,
        string relationshipName,
        RelationshipDeclaration target)
    {
        switch (relationshipName)
        {
            case "appStoreVersion":
                relationships.AppStoreVersion = target;
                break;
            case "appCustomProductPageVersion":
                relationships.AppCustomProductPageVersion = target;
                break;
            case "appStoreVersionExperiment":
                relationships.AppStoreVersionExperiment = target;
                break;
            case "appStoreVersionExperimentV2":
                relationships.AppStoreVersionExperimentV2 = target;
                break;
            case "appEvent":
                relationships.AppEvent = target;
                break;
            case "backgroundAssetVersion":
                relationships.BackgroundAssetVersion = target;
                break;
            case "gameCenterAchievementVersion":
                relationships.GameCenterAchievementVersion = target;
                break;
            case "gameCenterActivityVersion":
                relationships.GameCenterActivityVersion = target;
                break;
            case "gameCenterChallengeVersion":
                relationships.GameCenterChallengeVersion = target;
                break;
            case "gameCenterLeaderboardSetVersion":
                relationships.GameCenterLeaderboardSetVersion = target;
                break;
            case "gameCenterLeaderboardVersion":
                relationships.GameCenterLeaderboardVersion = target;
                break;
            case "inAppPurchaseVersion":
                relationships.InAppPurchaseVersion = target;
                break;
            case "subscriptionVersion":
                relationships.SubscriptionVersion = target;
                break;
            case "subscriptionGroupVersion":
                relationships.SubscriptionGroupVersion = target;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(relationshipName), relationshipName, null);
        }
    }

    private static string DecodedQuery(TestHttpMessageHandler handler)
    {
        return Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.Query);
    }
}
