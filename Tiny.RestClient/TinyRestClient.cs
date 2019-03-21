using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpStreamContent = System.Net.Http.StreamContent;
using HttpStringContent = System.Net.Http.StringContent;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class <see cref="TinyRestClient"/>.
    /// </summary>
    public class TinyRestClient
    {
        #region Fields
        private const int BufferSize = 81920;
        private static readonly HttpMethod _PatchMethod = new HttpMethod("PATCH");
        private readonly HttpClient _httpClient;
        private readonly string _serverAddress;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyRestClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyRestClient(HttpClient httpClient, string serverAddress)
        {
            _serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            // We manage it in by our own settings
            if (httpClient.Timeout != Timeout.InfiniteTimeSpan)
            {
                httpClient.Timeout = Timeout.InfiniteTimeSpan;
            }

            if (!_serverAddress.EndsWith("/"))
            {
                _serverAddress += "/";
            }

            Settings = new RestClientSettings();
        }
        #endregion

        /// <summary>
        /// Settings of <see cref="TinyRestClient"/>
        /// </summary>
        public RestClientSettings Settings { get; }

        #region Requests

        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="httpMethod">The httpMethod.</param>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest NewRequest(HttpMethod httpMethod, string route = null)
        {
            return new Request(httpMethod, route, this);
        }

        /// <summary>
        /// Create a new GET request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest GetRequest(string route = null)
        {
            return new Request(HttpMethod.Get, route, this);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PostRequest(string route = null)
        {
            return new Request(HttpMethod.Post, route, this);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <param name="compression">Add compresion system use to compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(TContent content, IFormatter formatter = null, ICompression compression = null)
        {
            return new Request(HttpMethod.Post, null, this).
                AddContent<TContent>(content, formatter, compression);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <param name="compression">Add compresion system use to compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(string route, TContent content, IFormatter formatter = null, ICompression compression = null)
        {
            return new Request(HttpMethod.Post, route, this).
                AddContent<TContent>(content, formatter, compression);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PutRequest(string route = null)
        {
            return new Request(HttpMethod.Put, route, this);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <param name="compression">Add compresion system use to compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(TContent content, IFormatter formatter = null, ICompression compression = null)
        {
            return new Request(HttpMethod.Put, null, this).
                AddContent<TContent>(content, formatter, compression);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <param name="compression">Add compresion system use to compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(string route, TContent content, IFormatter formatter = null, ICompression compression = null)
        {
            return new Request(HttpMethod.Put, route, this).
                AddContent<TContent>(content, formatter, compression);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PatchRequest(string route = null)
        {
            return new Request(_PatchMethod, route, this);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <param name="compression">Add compresion system use to compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(TContent content, IFormatter serializer = null, ICompression compression = null)
        {
            return new Request(_PatchMethod, null, this).
                AddContent<TContent>(content, serializer, compression);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <param name="compression">Add compresion system use ton compress content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(string route, TContent content, IFormatter serializer = null, ICompression compression = null)
        {
            return new Request(_PatchMethod, route, this).
                AddContent<TContent>(content, serializer, compression);
        }

        /// <summary>
        /// Create a new DELETE request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest DeleteRequest(string route = null)
        {
            return new Request(HttpMethod.Delete, route, this);
        }

        #endregion

        internal async Task<TResult> ExecuteAsync<TResult>(
            Request tinyRequest,
            IFormatter formatter,
            CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                var eTagContainer = GetETagContainer(tinyRequest);

                cancellationToken.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, eTagContainer, formatter, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, eTagContainer, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || stream.CanRead == false)
                        {
                            return default;
                        }

                        if (formatter == null)
                        {
                            if (response.Content.Headers?.ContentType?.MediaType != null)
                            {
                                // TODO : optimize the seach of formatter ?
                                // Try to find best formatter
                                var formatterFinded = Settings.Formatters.FirstOrDefault(f => f.SupportedMediaTypes.Any(m => m == response.Content.Headers.ContentType.MediaType.ToLower()));
                                formatter = formatterFinded;
                            }

                            if (formatter == null)
                            {
                                formatter = Settings.Formatters.Default;
                            }
                        }

                        try
                        {
                            stream.Position = 0;
                            return formatter.Deserialize<TResult>(stream, Settings.Encoding);
                        }
                        catch (Exception ex)
                        {
                            string data = null;
                            try
                            {
                                if (stream.CanRead)
                                {
                                    stream.Position = 0;
                                    using (var reader = new StreamReader(stream, Settings.Encoding))
                                    {
                                        data = await reader.ReadToEndAsync().ConfigureAwait(false);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }

                            throw new DeserializeException("Error during deserialization", ex, data);
                        }
                    }
                }
            }
        }

        internal async Task ExecuteAsync(
            Request tinyRequest,
            CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    await HandleResponseAsync(response, tinyRequest.ResponseHeaders, null, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        internal async Task<byte[]> ExecuteAsByteArrayResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                var eTagContainer = GetETagContainer(tinyRequest);

                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, eTagContainer, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, eTagContainer, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || !stream.CanRead)
                        {
                            return null;
                        }

                        using (var ms = new MemoryStream())
                        {
                            stream.Position = 0;
                            await stream.CopyToAsync(ms, BufferSize, cancellationToken).ConfigureAwait(false);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        internal async Task<Stream> ExecuteAsStreamResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                var eTagContainer = GetETagContainer(tinyRequest);
                var response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, eTagContainer, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false);
                var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, eTagContainer, cancellationToken).ConfigureAwait(false);
                if (stream == null || !stream.CanRead)
                {
                    return null;
                }

                return stream;
            }
        }

        internal async Task<string> ExecuteAsStringResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                var eTagContainer = GetETagContainer(tinyRequest);
                using (var response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, eTagContainer, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, eTagContainer, cancellationToken).ConfigureAwait(false);
                    if (stream == null || !stream.CanRead)
                    {
                        return null;
                    }

                    using (var reader = new StreamReader(stream, Settings.Encoding))
                    {
                        stream.Position = 0;
                        cancellationToken.ThrowIfCancellationRequested();
                        var toReturn = await reader.ReadToEndAsync().ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();
                        return toReturn;
                    }
                }
            }
        }

        internal async Task<HttpResponseMessage> ExecuteAsHttpResponseMessageResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                return await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<HttpContent> CreateContentAsync(IContent content, CancellationToken cancellationToken)
        {
            if (content == null)
            {
                return null;
            }

            if (content is StringContent stringContent)
            {
                var contentString = new HttpStringContent(stringContent.Data);
                SetContentType(stringContent.ContentType, contentString);
                return contentString;
            }

            if (content is StreamContent currentContent)
            {
                var contentStream = new HttpStreamContent(currentContent.Data);
                SetContentType(currentContent.ContentType, contentStream);
                return contentStream;
            }

            if (content is BytesContent bytesContent)
            {
                var contentArray = new ByteArrayContent(bytesContent.Data);
                SetContentType(bytesContent.ContentType, contentArray);
                return contentArray;
            }

            if (content is FormParametersContent formContent)
            {
                return new FormUrlEncodedContent(formContent.Data);
            }

            if (content is IToSerializeContent toSerializeContent)
            {
                return await GetSerializedContentAsync(toSerializeContent, cancellationToken).ConfigureAwait(false);
            }

            #if !FILEINFO_NOT_SUPPORTED
            if (content is FileContent fileContent)
            {
                var currentFileContent = new HttpStreamContent(fileContent.Data.OpenRead());
                SetContentType(fileContent.ContentType, currentFileContent);
                return currentFileContent;
            }
            #endif

            if (content is MultipartContent multiParts)
            {
                var multiPartContent = new MultipartFormDataContent();

                // get boundary automaticaly generated
                var boundary = multiPartContent.Headers.ContentType.Parameters.FirstOrDefault(n => n.Name == "boundary").Value;

                if (multiParts.ContentType != null)
                {
                    SetContentType(multiParts.ContentType, multiPartContent);
                    multiPartContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));
                }

                foreach (var currentPart in multiParts)
                {
                    if (currentPart is BytesMultipartData currentBytesPart)
                    {
                        var bytesMultiContent = new ByteArrayContent(currentBytesPart.Data);
                        SetContentType(currentBytesPart.ContentType, bytesMultiContent);
                        AddMultiPartContent(currentPart, bytesMultiContent, multiPartContent);
                    }
                    else if (currentPart is StreamMultipartData currentStreamPart)
                    {
                        var streamContent = new HttpStreamContent(currentStreamPart.Data);
                        SetContentType(currentStreamPart.ContentType, streamContent);
                        AddMultiPartContent(currentPart, streamContent, multiPartContent);
                    }
                    else if (currentPart is StringMultipartData currentStringPart)
                    {
                        var stringMultiContent = new HttpStringContent(currentStringPart.Data);
                        SetContentType(currentStringPart.ContentType, stringMultiContent);
                        AddMultiPartContent(currentPart, stringMultiContent, multiPartContent);
                    }
                    else if (currentPart is IToSerializeContent toSerializeMultiContent)
                    {
                        var serializedContent = await GetSerializedContentAsync(toSerializeMultiContent, cancellationToken).ConfigureAwait(false);
                        AddMultiPartContent(currentPart, serializedContent, multiPartContent);
                    }

                    #if !FILEINFO_NOT_SUPPORTED
                    else if (currentPart is FileMultipartData currentFileMultipartData)
                    {
                        var currentStreamContent = new HttpStreamContent(currentFileMultipartData.Data.OpenRead());
                        SetContentType(currentFileMultipartData.ContentType, currentStreamContent);
                        AddMultiPartContent(currentPart, currentStreamContent, multiPartContent);
                    }
                    #endif
                    else
                    {
                        throw new NotImplementedException($"GetContent multipart for '{currentPart.GetType().Name}' not implemented");
                    }
                }

                return multiPartContent;
            }

            throw new NotImplementedException($"GetContent for '{content.GetType().Name}' not implemented");
        }

        private async Task<HttpContent> GetSerializedContentAsync(IToSerializeContent content, CancellationToken cancellationToken)
        {
            IFormatter serializer = Settings.Formatters.Default;

            if (content.Serializer != null)
            {
                serializer = content.Serializer;
            }

            string serializedString = null;
            try
            {
                serializedString = content.GetSerializedString(serializer, Settings.Encoding);
            }
            catch (Exception ex)
            {
                throw new SerializeException(content.TypeToSerialize, ex);
            }

            if (serializedString == null)
            {
                return null;
            }

            var compression = content.Compression;
            if (compression != null)
            {
                using (var stream = new MemoryStream(Settings.Encoding.GetBytes(serializedString)))
                {
                    var compressedStream = await compression.CompressAsync(stream, BufferSize, cancellationToken).ConfigureAwait(false);
                    compressedStream.Position = 0;

                    var compressedContent = new HttpStreamContent(compressedStream);
                    compressedContent.Headers.ContentType = new MediaTypeHeaderValue(serializer.DefaultMediaType);
                    compressedContent.Headers.ContentEncoding.Add(compression.ContentEncoding);
                    return compressedContent;
                }
            }

            var stringContent = new HttpStringContent(serializedString, Settings.Encoding);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(serializer.DefaultMediaType);
            return stringContent;
        }

        private void AddMultiPartContent(MultipartData currentContent, HttpContent content, MultipartFormDataContent multipartFormDataContent)
        {
            if (string.IsNullOrWhiteSpace(currentContent.Name) && string.IsNullOrWhiteSpace(currentContent.FileName))
            {
                multipartFormDataContent.Add(content);
            }
            else if (!string.IsNullOrWhiteSpace(currentContent.Name) && !string.IsNullOrWhiteSpace(currentContent.FileName))
            {
                multipartFormDataContent.Add(content, currentContent.Name, currentContent.FileName);
            }
            else if (!string.IsNullOrWhiteSpace(currentContent.Name))
            {
                multipartFormDataContent.Add(content, currentContent.Name);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SetContentType(string contentType, HttpContent content)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return;
            }

            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        }

        private Uri BuildRequestUri(string route, Dictionary<string, string> queryParameters)
        {
            var stringBuilder = new StringBuilder(string.Concat(_serverAddress, route));

            if (queryParameters != null && queryParameters.Any())
            {
                var last = queryParameters.Last();
                stringBuilder.Append("?");
                for (int i = 0; i < queryParameters.Count; i++)
                {
                    var item = queryParameters.ElementAt(i);
                    var separator = i == queryParameters.Count - 1 ? string.Empty : "&";
                    stringBuilder.Append($"{item.Key}={WebUtility.UrlEncode(item.Value)}{separator}");
                }
            }

            return new Uri(stringBuilder.ToString());
        }

        private async Task<HttpResponseMessage> SendRequestAsync(
            HttpMethod httpMethod,
            Uri uri,
            Headers requestHeader,
            HttpContent content,
            IETagContainer eTagContainer,
            IFormatter deserializer,
            TimeSpan? localTimeout,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Stopwatch stopwatch = null;

            if (Settings.Listeners.MeasureTime)
            {
                stopwatch = new Stopwatch();
            }

            using (var request = new HttpRequestMessage(httpMethod, uri))
            {
                if (deserializer == null)
                {
                    deserializer = Settings.Formatters.Default;
                }

                if (!string.IsNullOrEmpty(deserializer.DefaultMediaType))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(deserializer.DefaultMediaType));
                }

                if (Settings.AddAcceptLanguageBasedOnCurrentCulture)
                {
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
                }

                foreach (var item in Settings.DefaultHeaders)
                {
                    request.Headers.Add(item.Key, item.Value);
                }

                foreach (var acceptEncoding in Settings.Compressions.Where(c => c.Value.AddAcceptEncodingHeader).Select(c => c.Key))
                {
                    request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(acceptEncoding));
                }

                if (requestHeader != null)
                {
                    foreach (var item in requestHeader)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                if (eTagContainer != null)
                {
                    if (!request.Headers.IfNoneMatch.Any())
                    {
                        var eTag = await eTagContainer.GetExistingETagAsync(uri, cancellationToken).ConfigureAwait(false);

                        if (eTag != null)
                        {
                            request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(eTag));
                        }
                    }
                }

                if (content != null)
                {
                    request.Content = content;
                }

                try
                {
                    HttpResponseMessage response = null;

                    await Settings.Listeners.OnSendingRequestAsync(uri, httpMethod, request, cancellationToken).ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                    stopwatch?.Start();
                    using (var cts = GetCancellationTokenSourceForTimeout(localTimeout ?? Settings.DefaultTimeout, cancellationToken))
                    {
                        try
                        {
                            var token = cancellationToken;

                            if (cts != null)
                            {
                                token = cts.Token;
                            }

                            response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false);
                            cts?.Token.ThrowIfCancellationRequested();
                        }
                        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                        {
                            throw new TimeoutException();
                        }
                    }

                    stopwatch?.Stop();
                    cancellationToken.ThrowIfCancellationRequested();
                    await Settings.Listeners.OnReceivedResponseAsync(uri, httpMethod, response, stopwatch?.Elapsed, cancellationToken).ConfigureAwait(false);
                    return response;
                }
                catch (OperationCanceledException e)
                {
                    throw e;
                }
                catch (TimeoutException e)
                {
                    stopwatch?.Stop();
                    await Settings.Listeners.OnFailedToReceiveResponseAsync(uri, httpMethod, e, stopwatch?.Elapsed, cancellationToken).ConfigureAwait(false);
                    throw e;
                }
                catch (Exception ex)
                {
                    stopwatch?.Stop();
                    await Settings.Listeners.OnFailedToReceiveResponseAsync(uri, httpMethod, ex, stopwatch?.Elapsed, cancellationToken).ConfigureAwait(false);
                    throw new ConnectionException(
                       "Failed to get a response from server",
                       uri.AbsoluteUri,
                       httpMethod.Method,
                       ex);
                }
            }
        }

        // Inspired by this blog post https://www.thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/
        private CancellationTokenSource GetCancellationTokenSourceForTimeout(
            TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                // No need to create a CTS if there's no timeout
                return null;
            }
            else
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeout);
                return cts;
            }
        }

        private IETagContainer GetETagContainer(Request request)
        {
            return request.ETagContainer ?? Settings.ETagContainer;
        }

        #region Read response
        private async Task<Stream> ReadResponseAsync(
            HttpResponseMessage response,
            Headers responseHeader,
            IETagContainer eTagContainer,
            CancellationToken cancellationToken)
        {
            await HandleResponseAsync(response, responseHeader, eTagContainer, cancellationToken).ConfigureAwait(false);

            Stream stream = null;
            if (eTagContainer != null && response.StatusCode == HttpStatusCode.NotModified)
            {
                stream = await eTagContainer.GetDataAsync(response.RequestMessage.RequestUri, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                if (eTagContainer != null)
                {
                    var tag = response.Headers.ETag.Tag;
                    if (tag != null)
                    {
                        await eTagContainer.SaveDataAsync(response.RequestMessage.RequestUri, tag, stream, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await DecompressAsync(response, stream, cancellationToken).ConfigureAwait(false);
        }

        private async Task<Stream> DecompressAsync(HttpResponseMessage response, Stream stream, CancellationToken cancellationToken)
        {
            var encoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
            if (encoding != null && Settings.Compressions.Contains(encoding))
            {
                var compression = Settings.Compressions[encoding];
                try
                {
                    return await compression.DecompressAsync(stream, BufferSize, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    stream.Dispose();
                }
            }

            return stream;
        }

        private async Task HandleResponseAsync(
            HttpResponseMessage response,
            Headers responseHeaders,
            IETagContainer eTagContainer,
            CancellationToken cancellationToken)
        {
            string content = null;
            if (responseHeaders != null)
            {
                responseHeaders.AddRange(response.Headers);

                if (response.Content != null && response.Content.Headers != null)
                {
                    responseHeaders.AddRange(response.Content.Headers);
                }
            }

            try
            {
                if (eTagContainer != null && response.StatusCode == HttpStatusCode.NotModified)
                {
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (!Settings.Ranges.CheckIfHttpStatusIsAllowed((int)response.StatusCode))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var newEx = new HttpException(
                    response.RequestMessage.RequestUri,
                    response.RequestMessage.Method.ToString(),
                    response.ReasonPhrase,
                    response.RequestMessage.Headers,
                    content,
                    response.StatusCode,
                    ex);

                throw newEx;
            }
        }
        #endregion
    }
}