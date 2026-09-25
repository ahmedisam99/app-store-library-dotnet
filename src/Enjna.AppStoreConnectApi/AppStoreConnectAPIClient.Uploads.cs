using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;

namespace Enjna.AppStoreConnectApi;

public partial class AppStoreConnectAPIClient
{
    private static readonly TimeSpan UploadTimeout = TimeSpan.FromMinutes(30);

    private HttpClient? _uploadHttpClient;

    /// <summary>
    /// Uploads an asset's bytes to the upload operations that App Store Connect returned when you
    /// reserved the asset. Commit the upload afterwards by patching the reserved resource with
    /// <c>uploaded</c> set to <c>true</c>. Where the resource's update request also takes a
    /// <c>sourceFileChecksum</c>, such as an App Review screenshot, send the checksum from
    /// <see cref="ComputeSourceFileChecksum"/> with it. The v2 in-app purchase and subscription
    /// images take no checksum.
    /// </summary>
    /// <param name="uploadOperations">The upload operations from the reservation response.</param>
    /// <param name="fileData">The bytes of the whole, unsplit asset file.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that completes once every part uploaded successfully.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the upload operations or the file data are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when an upload operation has no URL, has a URL or HTTP method this client can't use, has a request header carrying a control character, or its byte range falls outside the file.</exception>
    /// <exception cref="APIException">Thrown if a response was returned indicating a part could not be uploaded.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/uploading-assets-to-app-store-connect"/>
    public async Task UploadAssetAsync(
        IEnumerable<UploadOperation> uploadOperations,
        byte[] fileData,
        CancellationToken cancellationToken = default)
    {
        if (uploadOperations is null)
        {
            throw new ArgumentNullException(nameof(uploadOperations));
        }

        if (fileData is null)
        {
            throw new ArgumentNullException(nameof(fileData));
        }

        var operations = uploadOperations as IReadOnlyList<UploadOperation> ?? uploadOperations.ToList();
        var parts = new (Uri Url, HttpMethod Method, UploadOperationHeader[] Headers, int Offset, int Length)[operations.Count];

        for (var index = 0; index < operations.Count; index++)
        {
            var operation = operations[index];

            if (operation.Url is null)
            {
                throw new ArgumentException("An upload operation is missing its URL.", nameof(uploadOperations));
            }

            if (!Uri.TryCreate(operation.Url, UriKind.Absolute, out var url)
                || (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException(
                    $"An upload operation has a URL that isn't an absolute HTTP URL: \"{operation.Url}\".",
                    nameof(uploadOperations));
            }

            HttpMethod method;

            if (string.IsNullOrEmpty(operation.Method))
            {
                method = HttpMethod.Put;
            }
            else
            {
                try
                {
                    method = new HttpMethod(operation.Method);
                }
                catch (FormatException exception)
                {
                    throw new ArgumentException(
                        $"An upload operation has an invalid HTTP method: \"{operation.Method}\".",
                        nameof(uploadOperations),
                        exception);
                }
            }

            var headers = operation.RequestHeaders ?? Array.Empty<UploadOperationHeader>();

            foreach (var header in headers)
            {
                ValidateUploadHeader(header, nameof(uploadOperations));
            }

            var offset = operation.Offset ?? 0;
            var length = operation.Length ?? fileData.Length - offset;

            if (offset < 0 || length < 0 || (long)offset + length > fileData.Length)
            {
                throw new ArgumentException(
                    $"The upload operation covers bytes {offset} to {(long)offset + length}, which falls outside the {fileData.Length} byte file.",
                    nameof(uploadOperations));
            }

            parts[index] = (url, method, headers, offset, length);
        }

        var uploadHttpClient = GetUploadHttpClient();

        foreach (var (url, method, headers, offset, length) in parts)
        {
            using var request = new HttpRequestMessage(method, url);
            var content = new ByteArrayContent(fileData, offset, length);
            request.Content = content;

            foreach (var header in headers)
            {
                if (header.Name is null)
                {
                    continue;
                }

                if (!request.Headers.TryAddWithoutValidation(header.Name, header.Value))
                {
                    content.Headers.TryAddWithoutValidation(header.Name, header.Value);
                }
            }

            using var response = await uploadHttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await CreateApiExceptionAsync(response, null, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Rejects an upload header whose name or value carries a control character. The headers come
    /// from the reservation response rather than from the caller, and they are written to the wire
    /// without validation, so a carriage return or line feed in a value would end the header and
    /// start another one — letting a tampered or proxied reservation response inject arbitrary
    /// headers into the upload request.
    /// </summary>
    /// <param name="header">The header to validate.</param>
    /// <param name="parameterName">The name of the public parameter the header arrived through.</param>
    /// <exception cref="ArgumentException">Thrown when the name or the value carries a control character.</exception>
    private static void ValidateUploadHeader(UploadOperationHeader? header, string parameterName)
    {
        if (header is null)
        {
            return;
        }

        if (ContainsControlCharacter(header.Name))
        {
            throw new ArgumentException(
                "An upload operation has a request header whose name carries a control character.",
                parameterName);
        }

        if (ContainsControlCharacter(header.Value))
        {
            throw new ArgumentException(
                $"The upload operation request header \"{header.Name}\" has a value that carries a control character.",
                parameterName);
        }
    }

    private static bool ContainsControlCharacter(string? value)
    {
        if (value is null)
        {
            return false;
        }

        foreach (var character in value)
        {
            if (char.IsControl(character) && character != '\t')
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Computes the checksum that commits an asset upload: the MD5 of the whole, unsplit asset file,
    /// as a lowercase hexadecimal string.
    /// </summary>
    /// <param name="fileData">The bytes of the whole, unsplit asset file.</param>
    /// <returns>The lowercase hexadecimal MD5 of the file.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the file data is <c>null</c>.</exception>
    /// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/uploading-assets-to-app-store-connect"/>
    public static string ComputeSourceFileChecksum(byte[] fileData)
    {
        if (fileData is null)
        {
            throw new ArgumentNullException(nameof(fileData));
        }

        return Convert.ToHexString(MD5.HashData(fileData)).ToLowerInvariant();
    }

    private HttpClient GetUploadHttpClient()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        return _uploadHttpClient ??= new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        })
        {
            Timeout = UploadTimeout
        };
    }
}
