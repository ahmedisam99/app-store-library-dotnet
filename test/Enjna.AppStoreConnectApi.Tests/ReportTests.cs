using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class ReportTests
{
    private const string VendorNumber = "85442345";

    [Fact]
    public async Task RejectsAQueryThatFightsARequiredSalesReportFilter()
    {
        var handler = new TestHttpMessageHandler();
        var client = TestUtilities.CreateClient(handler);

        var query = new AppStoreConnectQuery()
            .Filter("reportType", "SUBSCRIBER")
            .Filter("vendorNumber", "87654321");

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Sales,
            SalesReportSubType.Summary,
            SalesReportFrequency.Daily,
            query,
            TestContext.Current.CancellationToken));

        Assert.Empty(handler.CapturedRequests);
    }

    [Fact]
    public async Task AcceptsAQueryThatRepeatsARequiredFilterWithTheSameValue()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Gzip("Provider\tSKU\n"), "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        var query = new AppStoreConnectQuery().Filter("vendorNumber", VendorNumber);

        await client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Sales,
            SalesReportSubType.Summary,
            SalesReportFrequency.Daily,
            query,
            TestContext.Current.CancellationToken);

        Assert.Contains("filter[vendorNumber]=" + VendorNumber, DecodedQuery(handler));
    }

    [Fact]
    public async Task SendsEveryRequiredSalesReportFilter()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Gzip("Provider\tSKU\n"), "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        await client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Subscription,
            SalesReportSubType.Summary,
            SalesReportFrequency.Monthly,
            cancellationToken: TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1/salesReports", request.RequestUri!.AbsolutePath);

        var query = DecodedQuery(handler);
        Assert.Contains("filter[vendorNumber]=85442345", query);
        Assert.Contains("filter[reportType]=SUBSCRIPTION", query);
        Assert.Contains("filter[reportSubType]=SUMMARY", query);
        Assert.Contains("filter[frequency]=MONTHLY", query);
    }

    [Fact]
    public async Task KeepsOptionalSalesReportFiltersAlongsideRequiredOnes()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Gzip("Provider\tSKU\n"), "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        var query = new AppStoreConnectQuery()
            .Filter("reportDate", "2024-06-01")
            .Filter("version", "1_0");

        await client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.SubscriptionOfferCodeRedemption,
            SalesReportSubType.Detailed,
            SalesReportFrequency.Daily,
            query,
            TestContext.Current.CancellationToken);

        var queryString = DecodedQuery(handler);
        Assert.Contains("filter[reportDate]=2024-06-01", queryString);
        Assert.Contains("filter[version]=1_0", queryString);
        Assert.Contains("filter[reportType]=SUBSCRIPTION_OFFER_CODE_REDEMPTION", queryString);
        Assert.Contains("filter[reportSubType]=DETAILED", queryString);
        Assert.Contains("filter[frequency]=DAILY", queryString);
        Assert.Contains("filter[vendorNumber]=85442345", queryString);
    }

    [Fact]
    public async Task SendsGzipAcceptHeaderForSalesReport()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Gzip("Provider\tSKU\n"), "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        await client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Sales,
            SalesReportSubType.Summary,
            SalesReportFrequency.Weekly,
            cancellationToken: TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal("application/a-gzip", request.Headers.Accept.ToString());
        Assert.DoesNotContain("application/json", request.Headers.Accept.ToString());
    }

    [Fact]
    public async Task ReturnsSalesReportBytesUntouched()
    {
        const string report = "Provider\tSKU\tUnits\nAPPLE\tEXAMPLENOTES\t312\n";
        var compressed = Gzip(report);

        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(compressed, "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        var downloaded = await client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Sales,
            SalesReportSubType.Summary,
            SalesReportFrequency.Weekly,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(compressed, downloaded);
        Assert.Equal(0x1f, downloaded[0]);
        Assert.Equal(0x8b, downloaded[1]);
        Assert.Equal(report, Gunzip(downloaded));
    }

    [Fact]
    public async Task SendsEveryRequiredFinanceReportFilter()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(Gzip("Start Date\tEnd Date\n"), "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        await client.GetFinanceReportAsync(
            VendorNumber,
            FinanceReportType.FinanceDetail,
            "ZZ",
            "2024-06",
            cancellationToken: TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/v1/financeReports", request.RequestUri!.AbsolutePath);
        Assert.Equal("application/a-gzip", request.Headers.Accept.ToString());

        var query = DecodedQuery(handler);
        Assert.Contains("filter[vendorNumber]=85442345", query);
        Assert.Contains("filter[reportType]=FINANCE_DETAIL", query);
        Assert.Contains("filter[regionCode]=ZZ", query);
        Assert.Contains("filter[reportDate]=2024-06", query);
    }

    [Fact]
    public async Task ReturnsFinanceReportBytesUntouched()
    {
        var compressed = Gzip("Start Date\tEnd Date\tVendor Identifier\n2024-06-01\t2024-06-30\tEXAMPLENOTES\n");

        var handler = new TestHttpMessageHandler();
        handler.EnqueueBytes(compressed, "application/a-gzip");
        var client = TestUtilities.CreateClient(handler);

        var downloaded = await client.GetFinanceReportAsync(
            VendorNumber,
            FinanceReportType.Financial,
            "US",
            "2024-06",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(compressed, downloaded);
    }

    [Fact]
    public async Task ThrowsApiExceptionWhenReportDownloadFails()
    {
        var handler = new TestHttpMessageHandler();
        handler.EnqueueJson(
            TestUtilities.ReadResourceAsString("models.appErrorResponse.json"),
            HttpStatusCode.NotFound);
        var client = TestUtilities.CreateClient(handler);

        var exception = await Assert.ThrowsAsync<APIException>(() => client.GetSalesReportAsync(
            VendorNumber,
            SalesReportType.Sales,
            SalesReportSubType.Summary,
            SalesReportFrequency.Weekly,
            cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(404, exception.HttpStatusCode);
        Assert.Equal("ENTITY_ERROR.ATTRIBUTE.INVALID", exception.Errors![0].Code);
    }

    [Fact]
    public async Task SendsAnalyticsReportRequestCreateBody()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(
            "models.reportAnalyticsReportRequestResponse.json",
            HttpStatusCode.Created);

        var request = new AnalyticsReportRequestCreateRequest
        {
            Data = new AnalyticsReportRequestCreateRequestData
            {
                Attributes = new AnalyticsReportRequestCreateRequestDataAttributes
                {
                    AccessType = AnalyticsReportRequestAccessType.Ongoing
                },
                Relationships = new AnalyticsReportRequestCreateRequestDataRelationships
                {
                    App = RelationshipDeclaration.To("apps", "6446939457")
                }
            }
        };

        var response = await client.CreateAnalyticsReportRequestAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
        Assert.Equal("/v1/analyticsReportRequests", handler.CapturedRequest.RequestUri!.AbsolutePath);

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var data = body.RootElement.GetProperty("data");
        Assert.Equal("analyticsReportRequests", data.GetProperty("type").GetString());
        Assert.Equal("ONGOING", data.GetProperty("attributes").GetProperty("accessType").GetString());

        var app = data.GetProperty("relationships").GetProperty("app").GetProperty("data");
        Assert.Equal("apps", app.GetProperty("type").GetString());
        Assert.Equal("6446939457", app.GetProperty("id").GetString());

        Assert.Equal("0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d", response.Data.Id);
        Assert.Equal(AnalyticsReportRequestAccessType.Ongoing, response.Data.Attributes?.AccessType);
        Assert.False(response.Data.Attributes?.StoppedDueToInactivity);
    }

    [Fact]
    public async Task DecodesReportsForAnalyticsReportRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reportAnalyticsReportsResponse.json");

        var query = new AppStoreConnectQuery().Filter("category", "APP_USAGE", "APP_STORE_ENGAGEMENT");

        var response = await client.ListReportsForAnalyticsReportRequestAsync(
            "0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
            query,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            "/v1/analyticsReportRequests/0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d/reports",
            handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Contains("filter[category]=APP_USAGE,APP_STORE_ENGAGEMENT", DecodedQuery(handler));

        Assert.Equal(3, response.Data.Length);
        Assert.Equal("App Store Installation and Deletion Standard", response.Data[0].Attributes?.Name);
        Assert.Equal(AnalyticsReportCategory.AppStoreEngagement, response.Data[0].Attributes?.Category);
        Assert.Equal(AnalyticsReportCategory.AppUsage, response.Data[1].Attributes?.Category);
    }

    [Fact]
    public async Task MapsUnknownAnalyticsReportCategoryToUnmapped()
    {
        var (client, _) = TestUtilities.GetClientWithJson("models.reportAnalyticsReportsResponse.json");

        var response = await client.ListReportsForAnalyticsReportRequestAsync(
            "0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
            cancellationToken: TestContext.Current.CancellationToken);

        var unknown = response.Data[2];
        Assert.Equal("Report From A Newer API Version", unknown.Attributes?.Name);
        Assert.Equal(AnalyticsReportCategory._Unmapped, unknown.Attributes?.Category);
    }

    [Fact]
    public async Task DecodesAnalyticsReportInstanceProcessingDate()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reportAnalyticsReportInstancesResponse.json");

        var response = await client.ListInstancesForAnalyticsReportAsync(
            "r2-app-sessions",
            new AppStoreConnectQuery().Filter("granularity", "DAILY"),
            TestContext.Current.CancellationToken);

        Assert.Equal("/v1/analyticsReports/r2-app-sessions/instances", handler.CapturedRequest!.RequestUri!.AbsolutePath);
        Assert.Contains("filter[granularity]=DAILY", DecodedQuery(handler));

        Assert.Equal(2, response.Data.Length);
        Assert.Equal(AnalyticsReportInstanceGranularity.Daily, response.Data[0].Attributes?.Granularity);
        Assert.Equal(new DateOnly(2024, 6, 27), response.Data[0].Attributes?.ProcessingDate);
        Assert.Equal(AnalyticsReportInstanceGranularity.Monthly, response.Data[1].Attributes?.Granularity);
        Assert.Equal(new DateOnly(2024, 5, 1), response.Data[1].Attributes?.ProcessingDate);
    }

    [Fact]
    public async Task DecodesAnalyticsReportSegmentDownloadDetails()
    {
        var (client, handler) = TestUtilities.GetClientWithJson("models.reportAnalyticsReportSegmentsResponse.json");

        var response = await client.ListSegmentsForAnalyticsReportInstanceAsync(
            "6e3a1f70-6d22-4f0a-9c8c-5c6a3d9b1e42",
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(
            "/v1/analyticsReportInstances/6e3a1f70-6d22-4f0a-9c8c-5c6a3d9b1e42/segments",
            handler.CapturedRequest!.RequestUri!.AbsolutePath);

        var segment = Assert.Single(response.Data);
        Assert.Equal(
            "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08",
            segment.Attributes?.Checksum);
        Assert.Equal(4823194L, segment.Attributes?.SizeInBytes);
        Assert.Equal(
            "https://analytics-reports.itunes.apple.com/segments/3f9d2a11?token=abc123",
            segment.Attributes?.Url);
    }

    [Fact]
    public async Task DeletesAnalyticsReportRequest()
    {
        var (client, handler) = TestUtilities.GetClientWithJson(null, HttpStatusCode.NoContent);

        await client.DeleteAnalyticsReportRequestAsync(
            "0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
            TestContext.Current.CancellationToken);

        var request = handler.CapturedRequest!;
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal(
            "/v1/analyticsReportRequests/0a1b2c3d-4e5f-6a7b-8c9d-0e1f2a3b4c5d",
            request.RequestUri!.AbsolutePath);
        Assert.Null(handler.CapturedRequestBody);
    }

    private static byte[] Gzip(string content)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            gzip.Write(bytes, 0, bytes.Length);
        }

        return output.ToArray();
    }

    private static string Gunzip(byte[] content)
    {
        using var input = new MemoryStream(content);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip, Encoding.UTF8);

        return reader.ReadToEnd();
    }

    private static string DecodedQuery(TestHttpMessageHandler handler)
    {
        return Uri.UnescapeDataString(handler.CapturedRequest!.RequestUri!.Query);
    }
}
