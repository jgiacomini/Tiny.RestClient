using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HttpStreamContent = System.Net.Http.StreamContent;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyHttpClient.
    /// </summary>
    public class TinyHttpClient
    {
        #region Fields
        private static readonly HttpMethod _PatchMethod = new HttpMethod("Patch");
        private readonly HttpClient _httpClient;
        private readonly string _serverAddress;
        private IFormatter _defaultFormatter;
        private Encoding _encoding;
        #endregion

        #region Logging events

        /// <summary>
        /// Raised whenever a request is sending.
        /// </summary>
        public event EventHandler<HttpSendingRequestEventArgs> SendingRequest;

        /// <summary>
        /// Raised whenever a response of resquest is received.
        /// </summary>
        public event EventHandler<HttpReceivedResponseEventArgs> ReceivedResponse;

        /// <summary>
        /// Raised whenever it failed to get of resquest.
        /// </summary>
        public event EventHandler<FailedToGetResponseEventArgs> FailedToGetResponse;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient" /> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(string serverAddress)
            : this(new HttpClient(), serverAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress)
        {
            _serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            DefaultHeaders = new Dictionary<string, string>();

            if (!_serverAddress.EndsWith("/"))
            {
                _serverAddress += "/";
            }

            _defaultFormatter = new JsonFormatter();
            var formatters = new List<IFormatter>
            {
                _defaultFormatter,
                new XmlFormatter()
            };

            Formatters = formatters.ToArray();
            _encoding = Encoding.UTF8;
        }
        #endregion

        /// <summary>
        /// Add to all request the AcceptLanguage based on CurrentCulture of the Thread
        /// </summary>
        public bool AddAcceptLanguageBasedOnCurrentCulture { get; set; }

        /// <summary>
        /// Gets the default headers.
        /// </summary>
        /// <value>
        /// The default headers.
        /// </value>
        public Dictionary<string, string> DefaultHeaders
        {
            get; private set;
        }

        /// <summary>
        /// Gets or set the encoding use by the client
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value ?? throw new ArgumentNullException(nameof(Encoding));
            }
        }

        /// <summary>
        /// Gets the list of formatter used to serialize and deserialize data
        /// </summary>
        public IFormatter DefaultFormatter
        {
            get
            {
                return _defaultFormatter;
            }
        }

        /// <summary>
        /// Gets the list of formatter used to serialize and deserialize data
        /// </summary>
        public IEnumerable<IFormatter> Formatters { get; private set; }

        /// <summary>
        /// Add a formatter in the list of supported formatters
        /// </summary>
        /// <param name="formatter">Add the formatter to the list of supported formatter. The value can't be null</param>
        /// <param name="isDefault">Define this formatter as default formatter</param>
        /// <exception cref="ArgumentNullException">throw <see cref="ArgumentNullException"/> if formatter is null</exception>
        public void AddFormatter(IFormatter formatter, bool isDefault)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (isDefault)
            {
                _defaultFormatter = formatter;
            }

            var newFormatters = Formatters.ToList();
            newFormatters.Add(formatter);
            Formatters = newFormatters.ToArray();
        }

        /// <summary>
        /// Removes a formatter in the list of supported formatters
        /// </summary>
        /// <param name="formatter">The formatter to remove on the supported formatter list</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found.</returns>
        /// <exception cref="ArgumentNullException">throw <see cref="ArgumentNullException"/> if formatter is null</exception>
        /// <exception cref="ArgumentException">throw <see cref="ArgumentException"/> if the current formatter removed is the default one </exception>
        public bool RemoveFormatter(IFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (_defaultFormatter == formatter)
            {
                throw new ArgumentException("Add a new default formatter before remove the current one");
            }

            var newList = Formatters.ToList();
            bool result = newList.Remove(formatter);
            Formatters = newList.ToArray();

            return result;
        }

        #region Requests

        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="httpMethod">The httpMethod.</param>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest NewRequest(HttpMethod httpMethod, string route = null)
        {
            return new TinyRequest(httpMethod, route, this);
        }

        /// <summary>
        /// Create a new GET request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest GetRequest(string route = null)
        {
            return new TinyRequest(HttpMethod.Get, route, this);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PostRequest(string route = null)
        {
            return new TinyRequest(HttpMethod.Post, route, this);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PostRequest<TContent>(TContent content, IFormatter formatter = null)
        {
            return new TinyRequest(HttpMethod.Post, null, this).
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
            return new TinyRequest(HttpMethod.Post, route, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PutRequest(string route = null)
        {
            return new TinyRequest(HttpMethod.Put, route, this);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="formatter">The formatter use to serialize the content</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PutRequest<TContent>(TContent content, IFormatter formatter = null)
        {
            return new TinyRequest(HttpMethod.Put, null, this).
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
            return new TinyRequest(HttpMethod.Put, route, this).
                AddContent<TContent>(content, formatter);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PatchRequest(string route = null)
        {
            return new TinyRequest(_PatchMethod, route, this);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <returns>The new request.</returns>
        public IParameterRequest PatchRequest<TContent>(TContent content, IFormatter serializer = null)
        {
            return new TinyRequest(_PatchMethod, null, this).
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
            return new TinyRequest(_PatchMethod, route, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new DELETE request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest DeleteRequest(string route = null)
        {
            return new TinyRequest(HttpMethod.Delete, route, this);
        }

        #endregion

        internal async Task<TResult> ExecuteAsync<TResult>(
            TinyRequest tinyRequest,
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
                            if (response.Content.Headers?.ContentType.MediaType != null)
                            {
                                // TODO : optimize the seach of formatter ?
                                // Try to find best formatter
                                var formatterFinded = Formatters.FirstOrDefault(f => f.SupportedMediaTypes.Any(m => m == response.Content.Headers.ContentType.MediaType.ToLower()));
                                formatter = formatterFinded;
                            }

                            if (formatter == null)
                            {
                                formatter = _defaultFormatter;
                            }
                        }

                        try
                        {
                            return formatter.Deserialize<TResult>(stream, _encoding);
                        }
                        catch (Exception ex)
                        {
                            string data = null;
                            try
                            {
                                if (stream.CanRead)
                                {
                                    stream.Position = 0;
                                    using (var reader = new StreamReader(stream, _encoding))
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
            TinyRequest tinyRequest,
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
           TinyRequest tinyRequest,
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
           TinyRequest tinyRequest,
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
           TinyRequest tinyRequest,
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

                using (StreamReader reader = new StreamReader(stream, Encoding))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        internal async Task<HttpResponseMessage> ExecuteAsHttpResponseMessageResultAsync(
           TinyRequest tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                return await SendRequestAsync(tinyRequest.HttpMethod, requestUri, tinyRequest.Headers, content, null, cancellationToken).ConfigureAwait(false);
            }
        }

        private HttpContent CreateContent(ITinyContent content)
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
            IFormatter serializer = _defaultFormatter;

            if (content.Serializer != null)
            {
                serializer = content.Serializer;
            }

            var serializedString = content.GetSerializedStream(serializer, _encoding);
            if (serializedString == null)
            {
                return null;
            }

            var stringContent = new StringContent(serializedString, _encoding);
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
            if (contentType == null)
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
                    stringBuilder.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}{separator}");
                }
            }

            return new Uri(stringBuilder.ToString());
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri uri, Dictionary<string, string> requestHeader, HttpContent content, IFormatter deserializer, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();
            Stopwatch sw = new Stopwatch();
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    if (deserializer == null)
                    {
                        deserializer = _defaultFormatter;
                    }

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(deserializer.DefaultMediaType));

                    if (AddAcceptLanguageBasedOnCurrentCulture)
                    {
                        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
                    }

                    foreach (var item in DefaultHeaders)
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

                    OnSendingRequest(requestId, uri, httpMethod);
                    sw.Start();
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

                    sw.Stop();
                    OnReceivedResponse(requestId, uri, httpMethod, response, sw.Elapsed);
                    return response;
                }
            }
            catch (Exception ex)
            {
                sw.Stop();

                OnFailedToReceiveResponse(requestId, uri, httpMethod, ex, sw.Elapsed);

                throw new ConnectionException(
                   "Failed to get a response from server",
                   uri.AbsoluteUri,
                   httpMethod.Method,
                   ex);
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

        #region Events invoker
        private void OnSendingRequest(string requestId, Uri url, HttpMethod httpMethod)
        {
            try
            {
                SendingRequest?.Invoke(this, new HttpSendingRequestEventArgs(requestId, url.ToString(), httpMethod.Method));
            }
            catch
            {
                // ignored
            }
        }

        private void OnReceivedResponse(string requestId, Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan elapsedTime)
        {
            try
            {
                ReceivedResponse?.Invoke(this, new HttpReceivedResponseEventArgs(requestId, uri.AbsoluteUri, httpMethod.Method, response.StatusCode, response.ReasonPhrase, elapsedTime));
            }
            catch
            {
                // ignored
            }
        }

        private void OnFailedToReceiveResponse(string requestId, Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan elapsedTime)
        {
            try
            {
                FailedToGetResponse?.Invoke(this, new FailedToGetResponseEventArgs(requestId, uri.AbsoluteUri, httpMethod.Method, exception, elapsedTime));
            }
            catch
            {
                // ignored
            }
        }
        #endregion
    }
}