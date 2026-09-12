using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    /// <summary>
    /// Downloads one sales and trends report. The response is a gzip archive that holds a single
    /// tab-separated text file, so decompress it before reading it.
    /// </summary>
    /// <param name="vendorNumber">Your vendor number, from Payments and Financial Reports in App Store Connect.</param>
    /// <param name="reportType">The kind of report to download.</param>
    /// <param name="reportSubType">The level of detail to download, which depends on the report type.</param>
    /// <param name="frequency">The reporting period the report covers.</param>
    /// <param name="query">Optional query parameters, such as <c>filter[reportDate]</c> and <c>filter[version]</c>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The gzip-compressed bytes of the report.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a report type, sub-type, or frequency is a value this library doesn't recognize.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-salesreports"/>
    public async Task<byte[]> GetSalesReportAsync(
        string vendorNumber,
        SalesReportType reportType,
        SalesReportSubType reportSubType,
        SalesReportFrequency frequency,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var queryParameters = query?.ToQueryParameters() ?? new Dictionary<string, string[]>(StringComparer.Ordinal);
        SetRequiredFilter(queryParameters, "filter[vendorNumber]", vendorNumber, nameof(query));
        SetRequiredFilter(queryParameters, "filter[reportType]", ToQueryValue(reportType, nameof(reportType)), nameof(query));
        SetRequiredFilter(queryParameters, "filter[reportSubType]", ToQueryValue(reportSubType, nameof(reportSubType)), nameof(query));
        SetRequiredFilter(queryParameters, "filter[frequency]", ToQueryValue(frequency, nameof(frequency)), nameof(query));

        return await MakeRawRequestAsync(
                path: "/v1/salesReports",
                method: HttpMethod.Get,
                queryParameters: queryParameters,
                accept: "application/a-gzip",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads one finance report. The response is a gzip archive that holds a single
    /// tab-separated text file, so decompress it before reading it.
    /// </summary>
    /// <param name="vendorNumber">Your vendor number, from Payments and Financial Reports in App Store Connect.</param>
    /// <param name="reportType">The kind of report to download.</param>
    /// <param name="regionCode">The region the report covers, such as <c>US</c>, <c>ZZ</c> for the rest of the world, or <c>Z1</c> for every region in one report.</param>
    /// <param name="reportDate">The fiscal period the report covers, as <c>YYYY-MM</c>. Apple's fiscal months don't line up with calendar months.</param>
    /// <param name="query">Optional query parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The gzip-compressed bytes of the report.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the report type is a value this library doesn't recognize.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-financereports"/>
    public async Task<byte[]> GetFinanceReportAsync(
        string vendorNumber,
        FinanceReportType reportType,
        string regionCode,
        string reportDate,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var queryParameters = query?.ToQueryParameters() ?? new Dictionary<string, string[]>(StringComparer.Ordinal);
        SetRequiredFilter(queryParameters, "filter[vendorNumber]", vendorNumber, nameof(query));
        SetRequiredFilter(queryParameters, "filter[reportType]", ToQueryValue(reportType, nameof(reportType)), nameof(query));
        SetRequiredFilter(queryParameters, "filter[regionCode]", regionCode, nameof(query));
        SetRequiredFilter(queryParameters, "filter[reportDate]", reportDate, nameof(query));

        return await MakeRawRequestAsync(
                path: "/v1/financeReports",
                method: HttpMethod.Get,
                queryParameters: queryParameters,
                accept: "application/a-gzip",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asks Apple to start generating analytics reports for an app. The reports take a day or so to
    /// appear, and you then read them from the request's <c>reports</c> relationship.
    /// </summary>
    /// <param name="request">The request body that describes the analytics report request to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains the new Analytics Report Requests resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/post-v1-analyticsreportrequests"/>
    public async Task<ResourceResponse<AnalyticsReportRequest>> CreateAnalyticsReportRequestAsync(
        AnalyticsReportRequestCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AnalyticsReportRequest>>(
                path: "/v1/analyticsReportRequests",
                method: HttpMethod.Post,
                queryParameters: null,
                body: request,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single analytics report request, including whether an ongoing one has stopped.
    /// </summary>
    /// <param name="analyticsReportRequestId">The ID of the analytics report request to read.</param>
    /// <param name="query">Optional query parameters such as fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an Analytics Report Requests resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreportrequests-_id_"/>
    public async Task<ResourceResponse<AnalyticsReportRequest>> GetAnalyticsReportRequestAsync(
        string analyticsReportRequestId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AnalyticsReportRequest>>(
                path: $"/v1/analyticsReportRequests/{analyticsReportRequestId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an analytics report request, which stops Apple from generating any further reports
    /// for it and revokes access to the ones it already produced.
    /// </summary>
    /// <param name="analyticsReportRequestId">The ID of the analytics report request to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once the analytics report request is deleted.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/delete-v1-analyticsreportrequests-_id_"/>
    public async Task DeleteAnalyticsReportRequestAsync(
        string analyticsReportRequestId,
        CancellationToken cancellationToken = default)
    {
        await MakeRequestAsync<object>(
                path: $"/v1/analyticsReportRequests/{analyticsReportRequestId}",
                method: HttpMethod.Delete,
                queryParameters: null,
                body: null,
                parseResponse: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the analytics reports an analytics report request produced.
    /// </summary>
    /// <param name="analyticsReportRequestId">The ID of the analytics report request to read the reports of.</param>
    /// <param name="query">Optional query parameters such as <c>filter[name]</c>, <c>filter[category]</c>, fields, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Analytics Reports resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreportrequests-_id_-reports"/>
    public async Task<ResourceListResponse<AnalyticsReport>> ListReportsForAnalyticsReportRequestAsync(
        string analyticsReportRequestId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AnalyticsReport>>(
                path: $"/v1/analyticsReportRequests/{analyticsReportRequestId}/reports",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single analytics report.
    /// </summary>
    /// <param name="analyticsReportId">The ID of the analytics report to read.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an Analytics Reports resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreports-_id_"/>
    public async Task<ResourceResponse<AnalyticsReport>> GetAnalyticsReportAsync(
        string analyticsReportId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AnalyticsReport>>(
                path: $"/v1/analyticsReports/{analyticsReportId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the instances of an analytics report, one per reporting period.
    /// </summary>
    /// <param name="analyticsReportId">The ID of the analytics report to read the instances of.</param>
    /// <param name="query">Optional query parameters such as <c>filter[granularity]</c>, <c>filter[processingDate]</c>, fields, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Analytics Report Instances resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreports-_id_-instances"/>
    public async Task<ResourceListResponse<AnalyticsReportInstance>> ListInstancesForAnalyticsReportAsync(
        string analyticsReportId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AnalyticsReportInstance>>(
                path: $"/v1/analyticsReports/{analyticsReportId}/instances",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single instance of an analytics report.
    /// </summary>
    /// <param name="analyticsReportInstanceId">The ID of the analytics report instance to read.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an Analytics Report Instances resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreportinstances-_id_"/>
    public async Task<ResourceResponse<AnalyticsReportInstance>> GetAnalyticsReportInstanceAsync(
        string analyticsReportInstanceId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AnalyticsReportInstance>>(
                path: $"/v1/analyticsReportInstances/{analyticsReportInstanceId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the segments an analytics report instance is split into. Each segment carries the URL
    /// to download that part of the instance's data from.
    /// </summary>
    /// <param name="analyticsReportInstanceId">The ID of the analytics report instance to read the segments of.</param>
    /// <param name="query">Optional query parameters such as fields and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Analytics Report Segments resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreportinstances-_id_-segments"/>
    public async Task<ResourceListResponse<AnalyticsReportSegment>> ListSegmentsForAnalyticsReportInstanceAsync(
        string analyticsReportInstanceId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AnalyticsReportSegment>>(
                path: $"/v1/analyticsReportInstances/{analyticsReportInstanceId}/segments",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a single segment of an analytics report instance.
    /// </summary>
    /// <param name="analyticsReportSegmentId">The ID of the analytics report segment to read.</param>
    /// <param name="query">Optional query parameters such as fields.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains an Analytics Report Segments resource.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-analyticsreportsegments-_id_"/>
    public async Task<ResourceResponse<AnalyticsReportSegment>> GetAnalyticsReportSegmentAsync(
        string analyticsReportSegmentId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceResponse<AnalyticsReportSegment>>(
                path: $"/v1/analyticsReportSegments/{analyticsReportSegmentId}",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Lists the analytics report requests that exist for an app.
    /// </summary>
    /// <param name="appId">The ID of the app to read the analytics report requests of.</param>
    /// <param name="query">Optional query parameters such as <c>filter[accessType]</c>, fields, includes, and paging limits.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response that contains a list of Analytics Report Requests resources.</returns>
    /// <exception cref="APIException">Thrown if a response was returned indicating the request could not be processed.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/get-v1-apps-_id_-analyticsreportrequests"/>
    public async Task<ResourceListResponse<AnalyticsReportRequest>> ListAnalyticsReportRequestsForAppAsync(
        string appId,
        AppStoreConnectQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        return await MakeRequestAsync<ResourceListResponse<AnalyticsReportRequest>>(
                path: $"/v1/apps/{appId}/analyticsReportRequests",
                method: HttpMethod.Get,
                queryParameters: query?.ToQueryParameters(),
                body: null,
                parseResponse: true,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Sets a filter that the endpoint requires and that a dedicated parameter already supplies.
    /// A value the caller put in the query for the same filter would otherwise be overwritten
    /// without a word, and the caller would get a valid report for the wrong thing.
    /// </summary>
    /// <param name="queryParameters">The query parameters being built.</param>
    /// <param name="name">The full filter name.</param>
    /// <param name="value">The value the dedicated parameter supplies.</param>
    /// <param name="queryParameterName">The name of the query parameter, for the exception.</param>
    /// <exception cref="ArgumentException">Thrown when the query already sets this filter to something else.</exception>
    private static void SetRequiredFilter(
        Dictionary<string, string[]> queryParameters,
        string name,
        string value,
        string queryParameterName)
    {
        if (queryParameters.TryGetValue(name, out var existing)
            && !(existing.Length == 1 && string.Equals(existing[0], value, StringComparison.Ordinal)))
        {
            throw new ArgumentException(
                $"The query sets \"{name}\", which this method supplies from its own parameter. "
                    + $"Remove it from the query, or pass the value you want through the dedicated parameter.",
                queryParameterName);
        }

        queryParameters[name] = new[] { value };
    }

    private static string ToQueryValue<TEnum>(TEnum value, string parameterName) where TEnum : struct, Enum
    {
        var attribute = typeof(TEnum)
            .GetField(value.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>();

        if (attribute?.Value is null)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "The value isn't one this library recognizes.");
        }

        return attribute.Value;
    }
}
