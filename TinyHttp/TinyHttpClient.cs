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
        private readonly HttpClient _httpClient;
        private readonly string _serverAddress;
        private readonly IFormatter _defaultFormatter;
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

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient" /> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(string serverAddress)
            : this(new HttpClient(), serverAddress, new JsonFormatter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="defaultFormatter">The default formatter.</param>
        public TinyHttpClient(string serverAddress, IFormatter defaultFormatter)
            : this(new HttpClient(), serverAddress, defaultFormatter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress)
            : this(httpClient, serverAddress, new JsonFormatter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        /// /// <param name="defaultFormatter">The serializer used for serialize data</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress, IFormatter defaultFormatter)
        {
            _serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _defaultFormatter = defaultFormatter ?? throw new ArgumentNullException(nameof(defaultFormatter));

            DefaultHeaders = new Dictionary<string, string>();

            if (!_serverAddress.EndsWith("/"))
            {
                _serverAddress += "/";
            }

            _encoding = Encoding.UTF8;
        }
        #endregion

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
        /// Create a new request.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest NewRequest(HttpVerb verb, string route = null)
        {
            return new TinyRequest(verb, route, this);
        }

        /// <summary>
        /// Create a new GET request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest GetRequest(string route = null)
        {
            return new TinyRequest(HttpVerb.Get, route, this);
        }

        /// <summary>
        /// Create a new POST request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PostRequest(string route = null)
        {
            return new TinyRequest(HttpVerb.Post, route, this);
        }

        /// <summary>
         /// Create a new POST request.
         /// </summary>
         /// <param name="content">The content of the request</param>
         /// <param name="route">The route.</param>
         /// <param name="serializer">The serializer use to serialize it</param>
         /// <returns>The new request.</returns>
        public IContentRequest PostRequest<TContent>(TContent content, string route = null, IFormatter serializer = null)
        {
            return new TinyRequest(HttpVerb.Post, route, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="route">The route.</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <returns>The new request.</returns>
        public IContentRequest PutRequest<TContent>(TContent content, string route = null, IFormatter serializer = null)
        {
            return new TinyRequest(HttpVerb.Put, route, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new PUT request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PutRequest(string route = null)
        {
            return new TinyRequest(HttpVerb.Put, route, this);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="content">The content of the request</param>
        /// <param name="route">The route.</param>
        /// <param name="serializer">The serializer use to serialize it</param>
        /// <returns>The new request.</returns>
        public IContentRequest PatchRequest<TContent>(TContent content, string route = null, IFormatter serializer = null)
        {
            return new TinyRequest(HttpVerb.Patch, route, this).
                AddContent<TContent>(content, serializer);
        }

        /// <summary>
        /// Create a new PATCH request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest PatchRequest(string route = null)
        {
            return new TinyRequest(HttpVerb.Patch, route, this);
        }

        /// <summary>
        /// Create a new DELETE request.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new request.</returns>
        public IRequest DeleteRequest(string route = null)
        {
            return new TinyRequest(HttpVerb.Delete, route, this);
        }

        internal async Task<TResult> ExecuteAsync<TResult>(
            TinyRequest tinyRequest,
            IFormatter formatter,
            CancellationToken cancellationToken)
        {
            if (formatter == null)
            {
                formatter = _defaultFormatter;
            }

            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, formatter, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || stream.CanRead == false)
                        {
                            return default;
                        }

                        return formatter.Deserialize<TResult>(stream);
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
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, null, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false))
                    {
                    }
                }
            }
        }

        internal async Task<byte[]> ExecuteByteArrayResultAsync(
           TinyRequest tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, null, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || !stream.CanRead)
                        {
                            return null;
                        }

                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        internal async Task<Stream> ExecuteWithStreamResultAsync(
           TinyRequest tinyRequest,
           CancellationToken cancellationToken)
        {
            using (var content = CreateContent(tinyRequest.Content))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, null, cancellationToken).ConfigureAwait(false);
                var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false);
                if (stream == null || !stream.CanRead)
                {
                    return null;
                }

                return stream;
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

            if (content is MultiPartContent multiParts)
            {
                var multiPartContent = new MultipartFormDataContent();
                var boundary = multiPartContent.Headers.ContentType.Parameters.FirstOrDefault(n => n.Name == "boundary").Value;

                if (multiParts.ContentType != null)
                {
                    SetContentType(multiParts.ContentType, multiPartContent);
                    multiPartContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));
                }

                foreach (var currentPart in multiParts)
                {
                    if (currentPart is BytesMultiPartData currentBytesPart)
                    {
                        var bytesMultiContent = new ByteArrayContent(currentBytesPart.Data);
                        SetContentType(currentBytesPart.ContentType, bytesMultiContent);
                        AddMulitPartContent(currentPart, bytesMultiContent, multiPartContent);
                    }
                    else if (currentPart is StreamMultiPartData currentStreamPart)
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

            var serializedString = content.GetSerializedStream(_defaultFormatter, _encoding);
            if (serializedString == null)
            {
                return null;
            }

            var stringContent = new StringContent(serializedString, _encoding);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(serializer.DefaultMediaType);
            return stringContent;
        }

        private void AddMulitPartContent(MultiPartData currentContent, HttpContent content, MultipartFormDataContent multipartFormDataContent)
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

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri uri, HttpContent content, IFormatter deserializer, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();
            Stopwatch sw = new Stopwatch();
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    if (deserializer != null)
                    {
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(deserializer.DefaultMediaType));
                    }

                    // TODO : add something to customize that stuff
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

                    // TODO : remove that ?
                    request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                    foreach (var item in DefaultHeaders)
                    {
                        request.Headers.Add(item.Key, item.Value);
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

        private HttpMethod ConvertToHttpMethod(HttpVerb httpVerb)
        {
            switch (httpVerb)
            {
                case HttpVerb.Get:
                    return HttpMethod.Get;
                case HttpVerb.Post:
                    return HttpMethod.Post;
                case HttpVerb.Put:
                    return HttpMethod.Put;
                case HttpVerb.Delete:
                    return HttpMethod.Delete;
                case HttpVerb.Head:
                    return HttpMethod.Head;
                case HttpVerb.Patch:
                    return new HttpMethod("PATCH");
                default:
                    throw new NotImplementedException();
            }
        }

        #region Read response
        private async Task<Stream> ReadResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            Stream stream = null;
            string content = null;
            try
            {
                stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

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