using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enjna.AppStoreConnectApi.Tests;

/// <summary>
/// An <see cref="HttpMessageHandler"/> that replays queued responses and records the requests it received.
/// </summary>
internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    private HttpResponseMessage? _lastEnqueuedResponse;

    private int _enqueuedCount;

    private int _consumedCount;

    public List<HttpRequestMessage> CapturedRequests { get; } = [];

    public List<string?> CapturedRequestBodies { get; } = [];

    public HttpRequestMessage? CapturedRequest => CapturedRequests.Count > 0 ? CapturedRequests[^1] : null;

    public string? CapturedRequestBody => CapturedRequestBodies.Count > 0 ? CapturedRequestBodies[^1] : null;

    /// <summary>
    /// The number of queued responses that no request has been handed yet. Stays accurate after disposal.
    /// </summary>
    public int RemainingResponseCount => _enqueuedCount - _consumedCount;

    public TestHttpMessageHandler EnqueueJson(string body, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };

        return Enqueue(response);
    }

    public TestHttpMessageHandler EnqueueBytes(byte[] body, string contentType, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var content = new ByteArrayContent(body);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        return Enqueue(new HttpResponseMessage(statusCode) { Content = content });
    }

    public TestHttpMessageHandler EnqueueEmpty(HttpStatusCode statusCode = HttpStatusCode.NoContent)
    {
        return Enqueue(new HttpResponseMessage(statusCode));
    }

    public TestHttpMessageHandler Enqueue(HttpResponseMessage response)
    {
        _responses.Enqueue(response);
        _lastEnqueuedResponse = response;
        _enqueuedCount++;
        return this;
    }

    /// <summary>
    /// Adds a rate limit header to the response enqueued most recently.
    /// </summary>
    public TestHttpMessageHandler WithRateLimit(string headerValue)
    {
        if (_lastEnqueuedResponse is null)
        {
            throw new InvalidOperationException("Enqueue a response before adding a rate limit header to it.");
        }

        _lastEnqueuedResponse.Headers.TryAddWithoutValidation("X-Rate-Limit", headerValue);
        return this;
    }

    /// <summary>
    /// Throws when a queued response was never handed to a request, which means a call the test
    /// queued a response for was never made. Sending fewer requests than expected is otherwise
    /// silent, and every <see cref="CapturedRequest"/> assertion after the missing call then reads
    /// the request before it and still passes.
    /// </summary>
    /// <exception cref="InvalidOperationException">A queued response was never requested.</exception>
    public void AssertAllResponsesConsumed()
    {
        if (RemainingResponseCount == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"{RemainingResponseCount} of {_enqueuedCount} queued responses were never requested. "
            + $"Requests received ({CapturedRequests.Count}): {DescribeCapturedRequests()}.");
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            while (_responses.Count > 0)
            {
                _responses.Dequeue().Dispose();
            }

            _lastEnqueuedResponse = null;
        }

        base.Dispose(disposing);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        CapturedRequests.Add(request);
        CapturedRequestBodies.Add(request.Content is not null
            ? await request.Content.ReadAsStringAsync(cancellationToken)
            : null);

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException($"No queued response for {request.Method} {request.RequestUri}.");
        }

        _consumedCount++;

        return _responses.Dequeue();
    }

    private string DescribeCapturedRequests()
    {
        return CapturedRequests.Count == 0
            ? "none"
            : string.Join(", ", CapturedRequests.Select(request => $"{request.Method} {request.RequestUri}"));
    }
}
