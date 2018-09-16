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

namespace Tiny.RestClient
{
    /// <summary>
    /// Class <see cref="TinyRestClient"/>.
    /// </summary>
    public class TinyRestClient
    {
        #region Fields
        private static readonly HttpMethod _PatchMethod = new HttpMethod("Patch");
        private readonly HttpClient _httpClient;
        private readonly string _serverAddress;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyRestClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyRestClient(HttpClient httpClient, string serverAddress)
        {
            _serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(TContent content, IFormatter formatter = null)
        {
            return new Request(HttpMethod.Post, null, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(string route, TContent content, IFormatter formatter = null)
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
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(TContent content, IFormatter formatter = null)
        {
            return new Request(HttpMethod.Put, null, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(string route, TContent content, IFormatter formatter = null)
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
        /// <param name="content">The content of the request</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(TContent content, IFormatter serializer = null)
        {
            return new Request(_PatchMethod, null, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="content">The content of the request</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(string route, TContent content, IFormatter serializer = null)
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
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, formatter, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ReponseHeaders, cancellationToken).ConfigureAwait(false))
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
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ReponseHeaders, cancellationToken).ConfigureAwait(false))
                    {
                    }
                }
            }
        }

        internal async Task<byte[]> ExecuteAsByteArrayResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, tinyRequest.ReponseHeaders, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || !stream.CanRead)
                        {
                            return null;
                        }

                        using (var ms = new MemoryStream())
                        {
                            await stream.CopyToAsync(ms).ConfigureAwait(false);
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
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false);
                var stream = await ReadResponseAsync(response, tinyRequest.ReponseHeaders, cancellationToken).ConfigureAwait(false);
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
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                HttpResponseMessage response = await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false);
                var stream = await ReadResponseAsync(response, tinyRequest.ReponseHeaders, cancellationToken).ConfigureAwait(false);
                if (stream == null || !stream.CanRead)
                {
                    return null;
                }

                using (StreamReader reader = new StreamReader(stream, Settings.Encoding))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        internal async Task<HttpResponseMessage> ExecuteAsHttpResponseMessageResultAsync(
           Request tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                return await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false);
            }
        }

        private HttpContent CreateContent(IContent content)
        {
            if (content == null)
            {
                return null;
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
                return GetSerializedContent(toSerializeContent);
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
                        AddMulitPartContent(currentPart, bytesMultiContent, multiPartContent);
                    }
                    else if (currentPart is StreamMultipartData currentStreamPart)
                    {
                        var streamContent = new HttpStreamContent(currentStreamPart.Data);
                        SetContentType(currentStreamPart.ContentType, streamContent);
                        AddMulitPartContent(currentPart, streamContent, multiPartContent);
                    }
                    else if (currentPart is IToSerializeContent toSerializeMultiContent)
                    {
                        var stringContent = GetSerializedContent(toSerializeMultiContent);
                        AddMulitPartContent(currentPart, stringContent, multiPartContent);
                    }
                    else if (currentPart is FileMultipartData currentFileMultipartData)
                    {
                        var currentStreamContent = new HttpStreamContent(currentFileMultipartData.Data.OpenRead());
                        SetContentType(currentFileMultipartData.ContentType, currentStreamContent);
                        AddMulitPartContent(currentPart, currentStreamContent, multiPartContent);
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

        private StringContent GetSerializedContent(IToSerializeContent content)
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

            var stringContent = new StringContent(serializedString, Settings.Encoding);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(serializer.DefaultMediaType);
            return stringContent;
        }

        private void AddMulitPartContent(MultipartData currentContent, HttpContent content, MultipartFormDataContent multipartFormDataContent)
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

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri uri, Dictionary<string, string> requestHeader, HttpContent content, IFormatter deserializer, CancellationToken cancellationToken)
        {
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

                if (requestHeader != null)
                {
                    foreach (var item in requestHeader)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                if (content != null)
                {
                    request.Content = content;
                }

                try
                {
                    using (var cts = GetCancellationTokenSourceForTimeout(request, cancellationToken))
                    {
                        await Settings.Listeners.OnSendingRequestAsync(uri, httpMethod, request).ConfigureAwait(false);
                        stopwatch?.Start();
                        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

                        stopwatch?.Stop();
                        await Settings.Listeners.OnReceivedResponseAsync(uri, httpMethod, response, stopwatch?.Elapsed).ConfigureAwait(false);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    stopwatch?.Stop();

                    await Settings.Listeners.OnFailedToReceiveResponseAsync(uri, httpMethod, ex, stopwatch?.Elapsed).ConfigureAwait(false);

                    throw new ConnectionException(
                       "Failed to get a response from server",
                       uri.AbsoluteUri,
                       httpMethod.Method,
                       ex);
                }
            }
        }

        private CancellationTokenSource GetCancellationTokenSourceForTimeout(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var timeout = Settings.DefaultTimeout;
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

        #region Read response
        private async Task<Stream> ReadResponseAsync(HttpResponseMessage response, Headers headersToFill, CancellationToken cancellationToken)
        {
            Stream stream = null;
            string content = null;
            try
            {
                stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                if (headersToFill != null)
                {
                    headersToFill.AddSource(response.Headers);
                }

                if (response.IsSuccessStatusCode)
                {
                    return stream;
                }
                else
                {
                    content = await StreamToStringAsync(stream).ConfigureAwait(false);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var newEx = new HttpException(
                    $"URL : {response.RequestMessage.RequestUri.ToString()}",
                    response.RequestMessage.Headers,
                    response.ReasonPhrase,
                    response.RequestMessage.RequestUri.ToString(),
                    response.RequestMessage.Method.ToString(),
                    content,
                    response.StatusCode,
                    ex);

                throw newEx;
            }

            return stream;
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                {
                    content = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            return content;
        }
        #endregion
    }
}