using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
#if SUPPORTS_ASYNC_ENUMERABLE
using System.Runtime.CompilerServices;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpStreamContent = System.Net.Http.StreamContent;
using HttpStringContent = System.Net.Http.StringContent;

namespace Tiny.RestClient
{
    /// <summary>
    /// A tiny, fluent, asynchronous HTTP client for consuming REST APIs.
    /// <para>
    /// Create an instance from an <see cref="HttpClient"/> and a base address, build a request with one of the
    /// verb methods (<see cref="GetRequest"/>, <see cref="PostRequest(string)"/>, ...), chain modifiers, then call
    /// one of the <c>ExecuteAs...Async</c> methods to send it.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// using Tiny.RestClient;
    ///
    /// var client = new TinyRestClient(new HttpClient(), "http://MyAPI.com/api");
    ///
    /// // GET http://MyAPI.com/api/City/All and deserialize the JSON response.
    /// List&lt;City&gt; cities = await client.GetRequest("City/All").ExecuteAsync&lt;List&lt;City&gt;&gt;();
    /// </code>
    /// </example>
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
        /// <param name="httpClient">The <see cref="HttpClient"/> used to send requests. Its timeout is managed by <see cref="RestClientSettings"/>.</param>
        /// <param name="serverAddress">The base address of the API. A trailing slash is appended if missing.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="serverAddress"/> is <c>null</c>.</exception>
        /// <example>
        /// <code>
        /// var client = new TinyRestClient(new HttpClient(), "http://MyAPI.com/api");
        /// </code>
        /// </example>
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
        /// Settings of <see cref="TinyRestClient"/>.
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
        /// <param name="route">The route appended to the base address.</param>
        /// <returns>The new request, ready to be chained and executed.</returns>
        /// <example>
        /// <code>
        /// // GET http://MyAPI.com/api/City?id=2&amp;country=France
        /// City city = await client
        ///     .GetRequest("City")
        ///     .AddQueryParameter("id", 2)
        ///     .AddQueryParameter("country", "France")
        ///     .ExecuteAsync&lt;City&gt;();
        /// </code>
        /// </example>
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
        /// <param name="content">The content of the request.</param>
        /// <param name="formatter">The formatter use to serialize the content.</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(TContent content, IFormatter formatter = null)
            where TContent : class
        {
            return new Request(HttpMethod.Post, null, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new POST request with a body that will be serialized (JSON by default).
        /// </summary>
        /// <param name="route">The route appended to the base address.</param>
        /// <param name="content">The content of the request, serialized with the resolved formatter.</param>
        /// <param name="formatter">Optional formatter used to serialize the content. When <c>null</c>, the default formatter is used.</param>
        /// <returns>The new request, ready to be chained and executed.</returns>
        /// <example>
        /// <code>
        /// var city = new City { Name = "Paris", Country = "France" };
        ///
        /// // POST http://MyAPI.com/api/City with the serialized city as body.
        /// bool created = await client.PostRequest("City", city).ExecuteAsync&lt;bool&gt;();
        /// </code>
        /// </example>
        public IParameterRequest PostRequest<TContent>(string route, TContent content, IFormatter formatter = null)
            where TContent : class
        {
            return new Request(HttpMethod.Post, route, this).
                AddContent<TContent>(content, formatter);
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
        /// <param name="content">The content of the request.</param>
        /// <param name="formatter">The formatter use to serialize the content.</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(TContent content, IFormatter formatter = null)
            where TContent : class
        {
            return new Request(HttpMethod.Put, null, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request.</param>
        /// <param name="formatter">The formatter use to serialize the content.</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(string route, TContent content, IFormatter formatter = null)
            where TContent : class
        {
            return new Request(HttpMethod.Put, route, this).
                AddContent<TContent>(content, formatter);
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
        /// <param name="content">The content of the request.</param>
        /// <param name="serializer">The serializer use to serialize it.</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(TContent content, IFormatter serializer = null)
            where TContent : class
        {
            return new Request(_PatchMethod, null, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request.</param>
        /// <param name="serializer">The serializer use to serialize it.</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(string route, TContent content, IFormatter serializer = null)
            where TContent : class
        {
            return new Request(_PatchMethod, route, this).
                AddContent<TContent>(content, serializer);
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

                using (var response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, eTagContainer, formatter, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, eTagContainer, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || stream.CanRead == false)
                        {
                            // TODO : throw an exception ?
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
                            return await formatter.DeserializeAsync<TResult>(stream, Settings.Encoding, cancellationToken);
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
                var etagContainer = GetETagContainer(tinyRequest);
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, etagContainer, null, tinyRequest.Timeout, cancellationToken).ConfigureAwait(false))
                {
                    await HandleResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, etagContainer, cancellationToken).ConfigureAwait(false);
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
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, eTagContainer, cancellationToken).ConfigureAwait(false))
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
                var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, eTagContainer, cancellationToken).ConfigureAwait(false);
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
                    var stream = await ReadResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, GetETagContainer(tinyRequest), cancellationToken).ConfigureAwait(false);
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

#if SUPPORTS_ASYNC_ENUMERABLE
        internal async IAsyncEnumerable<ServerSentEvent> ExecuteAsSSEAsync(
           Request tinyRequest,
           [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var content = await CreateContentAsync(tinyRequest.Content, cancellationToken).ConfigureAwait(false);
            try
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                var eTagContainer = GetETagContainer(tinyRequest);

                // ResponseHeadersRead is mandatory : it returns as soon as headers are received
                // so the body can be consumed incrementally instead of being fully buffered.
                var response = await SendRequestAsync(
                    tinyRequest.HttpMethod,
                    requestUri,
                    tinyRequest.Headers,
                    content,
                    eTagContainer,
                    null,
                    tinyRequest.Timeout,
                    cancellationToken,
                    HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                try
                {
                    await HandleResponseAsync(response, tinyRequest.ResponseHeaders, tinyRequest.HttpStatusCodeAllowed, eTagContainer, cancellationToken).ConfigureAwait(false);

#if NET8_0_OR_GREATER
                    using (var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
#else
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
#endif
                    using (var reader = new StreamReader(stream, Settings.Encoding))
                    {
                        await foreach (var sse in ParseServerSentEventsAsync(reader, cancellationToken).ConfigureAwait(false))
                        {
                            yield return sse;
                        }
                    }
                }
                finally
                {
                    response.Dispose();
                }
            }
            finally
            {
                content?.Dispose();
            }
        }

        private static async IAsyncEnumerable<ServerSentEvent> ParseServerSentEventsAsync(
            StreamReader reader,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string id = null;
            string eventType = null;
            int? retry = null;
            StringBuilder dataBuilder = null;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var line = await reader.ReadLineAsync().ConfigureAwait(false);

                if (line == null)
                {
                    // End of stream : dispatch a pending event if any.
                    if (dataBuilder != null || eventType != null || id != null || retry != null)
                    {
                        yield return new ServerSentEvent(id, eventType ?? "message", dataBuilder?.ToString(), retry);
                    }

                    yield break;
                }

                if (line.Length == 0)
                {
                    // Blank line : dispatch the accumulated event.
                    if (dataBuilder != null || eventType != null || id != null || retry != null)
                    {
                        yield return new ServerSentEvent(id, eventType ?? "message", dataBuilder?.ToString(), retry);
                    }

                    id = null;
                    eventType = null;
                    retry = null;
                    dataBuilder = null;
                    continue;
                }

                if (line[0] == ':')
                {
                    // Comment line : ignored.
                    continue;
                }

                string field;
                string value;
                var colonIndex = line.IndexOf(':');
                if (colonIndex == -1)
                {
                    field = line;
                    value = string.Empty;
                }
                else
                {
                    field = line.Substring(0, colonIndex);
                    value = line.Substring(colonIndex + 1);

                    // A single leading space after the colon is removed.
                    if (value.Length > 0 && value[0] == ' ')
                    {
                        value = value.Substring(1);
                    }
                }

                switch (field)
                {
                    case "event":
                        eventType = value;
                        break;
                    case "data":
                        if (dataBuilder == null)
                        {
                            dataBuilder = new StringBuilder();
                        }
                        else
                        {
                            dataBuilder.Append('\n');
                        }

                        dataBuilder.Append(value);
                        break;
                    case "id":
                        id = value;
                        break;
                    case "retry":
                        if (int.TryParse(value, out var retryValue))
                        {
                            retry = retryValue;
                        }

                        break;
                    default:
                        // Unknown fields are ignored per the SSE specification.
                        break;
                }
            }
        }
#endif

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

            if (content is FileContent fileContent)
            {
                var currentFileContent = new HttpStreamContent(fileContent.Data.OpenRead());
                SetContentType(fileContent.ContentType, currentFileContent);
                return currentFileContent;
            }

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
                    else if (currentPart is FileMultipartData currentFileMultipartData)
                    {
                        var currentStreamContent = new HttpStreamContent(currentFileMultipartData.Data.OpenRead());
                        SetContentType(currentFileMultipartData.ContentType, currentStreamContent);
                        AddMultiPartContent(currentPart, currentStreamContent, multiPartContent);
                    }
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

            string serializedString;
            try
            {
                serializedString = await content.GetSerializedStringAsync(serializer, Settings.Encoding, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new SerializeException(content.TypeToSerialize, ex);
            }

            if (serializedString == null)
            {
                return null;
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
            CancellationToken cancellationToken,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Stopwatch stopwatch = null;

            if (Settings.Listeners.MeasureTime)
            {
                stopwatch = new Stopwatch();
            }

            Headers calculatedHeaders = null;
            var calculateHeadersHandler = Settings.CalculateHeadersHandler;
            if (calculateHeadersHandler != null)
            {
                calculatedHeaders = await calculateHeadersHandler.Invoke();
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

                if (requestHeader != null)
                {
                    foreach (var item in requestHeader)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                if (calculatedHeaders != null)
                {
                    foreach (var item in calculatedHeaders)
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

                            response = await _httpClient.SendAsync(request, completionOption, token).ConfigureAwait(false);
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
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (TimeoutException e)
                {
                    stopwatch?.Stop();
                    await Settings.Listeners.OnFailedToReceiveResponseAsync(uri, httpMethod, e, stopwatch?.Elapsed, cancellationToken).ConfigureAwait(false);
                    throw;
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
            HttpStatusRanges httpStatusRanges,
            IETagContainer eTagContainer,
            CancellationToken cancellationToken)
        {
            await HandleResponseAsync(response, responseHeader, httpStatusRanges, eTagContainer, cancellationToken).ConfigureAwait(false);
            Stream stream;
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
            return stream;
        }

        private async Task HandleResponseAsync(
            HttpResponseMessage response,
            Headers responseHeaders,
            HttpStatusRanges requestAllowedStatusRange,
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

                bool haveToEnsureSuccessStatusCode = false;
                if (!response.IsSuccessStatusCode)
                {
                    // if status code is not allowed we read the content
                    if (!CheckIfStatusCodeIsAllowed((int)response.StatusCode, requestAllowedStatusRange))
                    {
                        haveToEnsureSuccessStatusCode = true;
                        content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }

                if (haveToEnsureSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Fix for Blazor WASM : requestMessage.RequestUri is always null in Blazor WASM
                Uri requestUri = null;
                string method = null;
                HttpRequestHeaders requestHeaders = null;
                var requestMessage = response.RequestMessage;
                if (requestMessage != null)
                {
                    requestUri = requestMessage.RequestUri;
                    method = requestMessage.Method.ToString();
                    requestHeaders = requestMessage.Headers;
                }

                var newEx = new HttpException(
                  requestUri,
                  method,
                  response.ReasonPhrase,
                  requestHeaders,
                  content,
                  response.StatusCode,
                  response.Headers,
                  ex);

                var handler = Settings.EncapsulateHttpExceptionHandler;
                if (handler != null)
                {
                    throw handler(newEx);
                }

                throw newEx;
            }
        }

        private bool CheckIfStatusCodeIsAllowed(int statusCode, HttpStatusRanges requestAllowedStatusRange)
        {
            if (requestAllowedStatusRange != null)
            {
                if (!requestAllowedStatusRange.CheckIfHttpStatusIsAllowed(statusCode))
                {
                    if (!Settings.HttpStatusCodeAllowed.CheckIfHttpStatusIsAllowed(statusCode))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                if (!Settings.HttpStatusCodeAllowed.CheckIfHttpStatusIsAllowed(statusCode))
                {
                    return false;
                }

                return true;
            }
        }
        #endregion
    }
}