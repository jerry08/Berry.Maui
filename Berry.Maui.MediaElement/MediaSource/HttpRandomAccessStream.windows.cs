using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpCompletionOption = Windows.Web.Http.HttpCompletionOption;
using HttpMethod = Windows.Web.Http.HttpMethod;
using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;

namespace Berry.Maui.Views;

// Taken from https://github.com/kiewic/MediaPlayerElementWithHttpClient/blob/master/MediaPlayerElementWithHttpClient/HttpRandomAccessStream.cs
internal sealed class HttpRandomAccessStream : IRandomAccessStreamWithContentType
{
    private readonly HttpClient _client;
    private IInputStream? inputStream;
    private ulong size;
    private ulong requestedPosition;
    //private string? etagHeader;
    //private string? lastModifiedHeader;
    private readonly Uri _requestedUri;

    // No public constructor, factory methods instead to handle async tasks.
    private HttpRandomAccessStream(HttpClient client, Uri uri)
    {
        _client = client;
        _requestedUri = uri;
        requestedPosition = 0;
    }

    public static IAsyncOperation<HttpRandomAccessStream> CreateAsync(HttpClient client, Uri uri)
    {
        var randomStream = new HttpRandomAccessStream(client, uri);

        return AsyncInfo.Run<HttpRandomAccessStream>(async (cancellationToken) =>
        {
            await randomStream.SendRequesAsync().ConfigureAwait(false);
            return randomStream;
        });
    }

    async Task SendRequesAsync()
    {
        Debug.Assert(inputStream is null);

        var request = new HttpRequestMessage(HttpMethod.Get, _requestedUri);

        //request.Headers.Add("Range", string.Format("bytes={0}-", requestedPosition));
        //
        //if (!string.IsNullOrEmpty(etagHeader))
        //{
        //    request.Headers.Add("If-Match", etagHeader);
        //}
        //
        //if (!string.IsNullOrEmpty(lastModifiedHeader))
        //{
        //    request.Headers.Add("If-Unmodified-Since", lastModifiedHeader);
        //}

        var response = await _client.SendRequestAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead).AsTask().ConfigureAwait(false);

        //if (response.Content.Headers.ContentType != null)
        //{
        //    ContentType = response.Content.Headers.ContentType.MediaType;
        //}

        size = response.Content.Headers.ContentLength ?? 0;

        if (response.StatusCode != HttpStatusCode.PartialContent && requestedPosition != 0)
        {
            throw new Exception("HTTP server did not reply with a '206 Partial Content' status.");
        }

        //if (!response.Headers.ContainsKey("Accept-Ranges"))
        //{
        //    throw new Exception(string.Format(
        //        "HTTP server does not support range requests: {0}",
        //        "http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.5"));
        //}

        //if (string.IsNullOrEmpty(etagHeader) && response.Headers.ContainsKey("ETag"))
        //{
        //    etagHeader = response.Headers["ETag"];
        //}
        //
        //if (string.IsNullOrEmpty(lastModifiedHeader) && response.Content.Headers.ContainsKey("Last-Modified"))
        //{
        //    lastModifiedHeader = response.Content.Headers["Last-Modified"];
        //}
        //
        //if (response.Content.Headers.ContainsKey("Content-Type"))
        //{
        //    ContentType = response.Content.Headers["Content-Type"];
        //}

        inputStream = await response.Content.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false);
    }

    public string ContentType { get; } = default!;

    public bool CanRead => true;

    public bool CanWrite => false;

    public IRandomAccessStream CloneStream()
    {
        // If there is only one MediaPlayerElement using the stream, it is safe to return itself.
        return this;
    }

    public IInputStream GetInputStreamAt(ulong position)
    {
        throw new NotImplementedException();
    }

    public IOutputStream GetOutputStreamAt(ulong position)
    {
        throw new NotImplementedException();
    }

    public ulong Position => requestedPosition;

    public void Seek(ulong position)
    {
        if (requestedPosition != position)
        {
            if (inputStream != null)
            {
                inputStream.Dispose();
                inputStream = null;
            }
            Debug.WriteLine("Seek: {0:N0} -> {1:N0}", requestedPosition, position);
            requestedPosition = position;
        }
    }

    public ulong Size
    {
        get
        {
            return size;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public void Dispose()
    {
        if (inputStream != null)
        {
            inputStream.Dispose();
            inputStream = null;
        }
    }

    public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
    {
        return AsyncInfo.Run<IBuffer, uint>(async (cancellationToken, progress) =>
        {
            progress.Report(0);

            try
            {
                if (inputStream is null)
                {
                    await SendRequesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }

            var result = await inputStream!.ReadAsync(buffer, count, options).AsTask(cancellationToken, progress).ConfigureAwait(false);

            // Move position forward.
            requestedPosition += result.Length;
            Debug.WriteLine("requestedPosition = {0:N0}", requestedPosition);

            return result;
        });
    }

    public IAsyncOperation<bool> FlushAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
    {
        throw new NotImplementedException();
    }
}