using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class CustomerReviewTests
{
    private const string ReviewId = "00000001-b1c2-4a5d-9e3f-0a1b2c3d4e5f";

    [Fact]
    public async Task DecodesCustomerReviewAttributes()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewCustomerReviewResponse.json");

        var response = await client.GetCustomerReviewAsync(
            ReviewId,
            new AppStoreConnectQuery().Include("response"),
            TestContext.Current.CancellationToken);

        Assert.Equal($"/v1/customerReviews/{ReviewId}", handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Contains("include=response", Uri.UnescapeDataString(handler.CapturedRequest.RequestUri.Query));

        var review = response.Data;
        Assert.NotNull(review);
        Assert.Equal("customerReviews", review.Type);
        Assert.Equal(ReviewId, review.Id);

        var attributes = review.Attributes;
        Assert.NotNull(attributes);
        Assert.Equal(4, attributes.Rating);
        Assert.Equal("Almost perfect", attributes.Title);
        Assert.StartsWith("Sync is fast", attributes.Body);
        Assert.Equal("notetaker_88", attributes.ReviewerNickname);
        Assert.Equal(
            new DateTimeOffset(2024, 6, 18, 11, 42, 7, TimeSpan.FromHours(-7)),
            attributes.CreatedDate);
        Assert.Equal(TerritoryCode.Usa, attributes.Territory);
    }

    [Fact]
    public async Task DecodesIncludedResponseForCustomerReview()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.reviewCustomerReviewResponse.json");

        var response = await client.GetCustomerReviewAsync(
            ReviewId,
            new AppStoreConnectQuery().Include("response"),
            TestContext.Current.CancellationToken);

        Assert.True(response.TryGetIncluded<CustomerReviewResponse>(
            "customerReviewResponses",
            ReviewId,
            out var reviewResponse));

        Assert.Equal(
            "Thanks for the report. The export dialog now remembers the last folder in version 1.4.1.",
            reviewResponse.Attributes?.ResponseBody);
        Assert.Equal(CustomerReviewResponseState.Published, reviewResponse.Attributes?.State);
        Assert.Equal(
            new DateTimeOffset(2024, 6, 20, 9, 15, 33, TimeSpan.FromHours(-7)),
            reviewResponse.Attributes?.LastModifiedDate);

        var linkage = response.Data.Relationships!["response"].ToOne();
        Assert.Equal("customerReviewResponses", linkage?.Type);
        Assert.Equal(ReviewId, linkage?.Id);
    }

    [Fact]
    public async Task DecodesCustomerReviewsForApp()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewCustomerReviewsResponse.json");

        var response = await client.ListCustomerReviewsForAppAsync(
            "6446939457",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("/v1/apps/6446939457/customerReviews", handler.CapturedRequest!.RequestUri!.AbsolutePath);

        Assert.Equal(2, response.Data.Length);
        Assert.Equal(5, response.Data[0].Attributes?.Rating);
        Assert.Equal(TerritoryCode.Can, response.Data[0].Attributes?.Territory);
        Assert.Equal(TerritoryCode.Gbr, response.Data[1].Attributes?.Territory);
        Assert.Equal("padpro_user", response.Data[1].Attributes?.ReviewerNickname);
    }

    [Fact]
    public async Task SendsCommaJoinedFilterValuesForCustomerReviews()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reviewCustomerReviewsResponse.json");

        var query = new AppStoreConnectQuery()
            .Filter("rating", "4", "5")
            .Filter("territory", "USA", "CAN", "GBR")
            .Exists("publishedResponse", "false")
            .Sort("-createdDate")
            .Limit(50);

        await client.ListCustomerReviewsForAppAsync("6446939457", query, TestContext.Current.CancellationToken);

        var queryString = Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.Query);
        Assert.Contains("filter[rating]=4,5", queryString);
        Assert.Contains("filter[territory]=USA,CAN,GBR", queryString);
        Assert.Contains("exists[publishedResponse]=false", queryString);
        Assert.Contains("sort=-createdDate", queryString);
        Assert.Contains("limit=50", queryString);
    }

    [Fact]
    public async Task SendsCustomerReviewResponseCreateBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.reviewCreatedResponse.json",
            HttpStatusCode.Created);

        var request = new CustomerReviewResponseV1CreateRequest
        {
            Data = new CustomerReviewResponseV1CreateRequestData
            {
                Attributes = new CustomerReviewResponseV1CreateRequestDataAttributes
                {
                    ResponseBody = "Thanks for writing in. iPad split view gets a proper layout in the next update."
                },
                Relationships = new CustomerReviewResponseV1CreateRequestDataRelationships
                {
                    Review = RelationshipDeclaration.To("customerReviews", "00000002-b1c2-4a5d-9e3f-0a1b2c3d4e5f")
                }
            }
        };

        var response = await client.CreateCustomerReviewResponseAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/v1/customerReviewResponses", handler.CapturedRequest.RequestUri!.AbsolutePath);
        Assert.Equal("application/json", handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("customerReviewResponses", data.GetProperty("type").GetString());
        Assert.Equal(
            "Thanks for writing in. iPad split view gets a proper layout in the next update.",
            data.GetProperty("attributes").GetProperty("responseBody").GetString());

        var review = data.GetProperty("relationships").GetProperty("review").GetProperty("data");
        Assert.Equal("customerReviews", review.GetProperty("type").GetString());
        Assert.Equal("00000002-b1c2-4a5d-9e3f-0a1b2c3d4e5f", review.GetProperty("id").GetString());
        Assert.False(data.TryGetProperty("id", out _));

        Assert.Equal("00000003-b1c2-4a5d-9e3f-0a1b2c3d4e5f", response.Data.Id);
        Assert.Equal(CustomerReviewResponseState.PendingPublish, response.Data.Attributes?.State);
    }

    [Fact]
    public async Task ReadsResponseForCustomerReviewFromRelatedPath()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(TestUtilities.ReadResourceAsString("models.reviewCreatedResponse.json"));
        var client = TestUtilities.CreateClient(handler);

        var response = await client.GetResponseForCustomerReviewAsync(
            "00000002-b1c2-4a5d-9e3f-0a1b2c3d4e5f",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "/v1/customerReviews/00000002-b1c2-4a5d-9e3f-0a1b2c3d4e5f/response",
            handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Equal("customerReviewResponses", response.Data.Type);
    }

    [Fact]
    public async Task DeletesCustomerReviewResponse()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteCustomerReviewResponseAsync(
            "00000003-b1c2-4a5d-9e3f-0a1b2c3d4e5f",
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal(
            "/v1/customerReviewResponses/00000003-b1c2-4a5d-9e3f-0a1b2c3d4e5f",
            request.RequestUri!.AbsolutePath);
        Assert.Null(handler.CapturedRequestBody);
    }
}
