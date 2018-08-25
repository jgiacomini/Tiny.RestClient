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
        private readonly ISerializer _defaultSerializer;
        private readonly IDeserializer _defaultDeserializer;
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
            : this(new HttpClient(), serverAddress, new TinyJsonSerializer(), new TinyJsonDeserializer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="defaultSerializer">The default serializer.</param>
        /// <param name="defaultDeserializer">The default deserializer.</param>
        public TinyHttpClient(string serverAddress, ISerializer defaultSerializer, IDeserializer defaultDeserializer)
            : this(new HttpClient(), serverAddress, defaultSerializer, defaultDeserializer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress)
            : this(httpClient, serverAddress, new TinyJsonSerializer(), new TinyJsonDeserializer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        /// /// <param name="defaultSerializer">The serializer used for serialize data</param>
        /// <param name="defaultDeserializer">The deserializer used for deszerialiaze data.</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress, ISerializer defaultSerializer, IDeserializer defaultDeserializer)
        {
            _serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _defaultSerializer = defaultSerializer ?? throw new ArgumentNullException(nameof(defaultSerializer));
            _defaultDeserializer = defaultDeserializer ?? throw new ArgumentNullException(nameof(defaultDeserializer));

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

        internal async Task<TResult> ExecuteAsync<TResult>(
            TinyRequest tinyRequest,
            CancellationToken cancellationToken)
        {
            IDeserializer deserializer = tinyRequest.Deserializer;
            ISerializer serializer = tinyRequest.Serializer;

            if (deserializer == null)
            {
                deserializer = _defaultDeserializer;
            }

            if (serializer == null)
            {
                serializer = _defaultSerializer;
            }

            using (var content = CreateContent(tinyRequest.ContentType, serializer, tinyRequest.GetContent(), tinyRequest.FormParameters, tinyRequest.MultiPartFormData))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, deserializer, cancellationToken).ConfigureAwait(false))
                {
                    using (var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false))
                    {
                        if (stream == null || stream.CanRead == false)
                        {
                            return default;
                        }

                        return deserializer.Deserialize<TResult>(stream);
                    }
                }
            }
        }

        internal async Task ExecuteAsync(
            TinyRequest tinyRequest,
            CancellationToken cancellationToken)
        {
            IDeserializer deserializer = tinyRequest.Deserializer;
            ISerializer serializer = tinyRequest.Serializer;

            if (deserializer == null)
            {
                deserializer = _defaultDeserializer;
            }

            if (serializer == null)
            {
                serializer = _defaultSerializer;
            }

            using (var content = CreateContent(tinyRequest.ContentType, serializer, tinyRequest.GetContent(), tinyRequest.FormParameters, tinyRequest.MultiPartFormData))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, deserializer, cancellationToken).ConfigureAwait(false))
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
            IDeserializer deserializer = tinyRequest.Deserializer;
            ISerializer serializer = tinyRequest.Serializer;

            if (deserializer == null)
            {
                deserializer = _defaultDeserializer;
            }

            if (serializer == null)
            {
                serializer = _defaultSerializer;
            }

            using (var content = CreateContent(tinyRequest.ContentType, serializer, tinyRequest.GetContent(), tinyRequest.FormParameters, tinyRequest.MultiPartFormData))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                using (HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, deserializer, cancellationToken).ConfigureAwait(false))
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
            IDeserializer deserializer = tinyRequest.Deserializer;
            ISerializer serializer = tinyRequest.Serializer;

            if (deserializer == null)
            {
                deserializer = _defaultDeserializer;
            }

            if (serializer == null)
            {
                serializer = _defaultSerializer;
            }

            using (var content = CreateContent(tinyRequest.ContentType, serializer, tinyRequest.GetContent(), tinyRequest.FormParameters, tinyRequest.MultiPartFormData))
            {
                var requestUri = BuildRequestUri(tinyRequest.Route, tinyRequest.QueryParameters);
                HttpResponseMessage response = await SendRequestAsync(ConvertToHttpMethod(tinyRequest.HttpVerb), requestUri, content, deserializer, cancellationToken).ConfigureAwait(false);
                var stream = await ReadResponseAsync(response, cancellationToken).ConfigureAwait(false);
                if (stream == null || !stream.CanRead)
                {
                    return null;
                }

                return stream;
            }
        }

        private HttpContent CreateContent(
            ContentType contentType,
            ISerializer serializer,
            object data,
            IEnumerable<KeyValuePair<string, string>> formsParameters,
            IEnumerable<MultiPartData> multiParts)
        {
            switch (contentType)
            {
                case ContentType.Stream:
                    var contentStream = new StreamContent(data as Stream);
                    contentStream.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return contentStream;
                case ContentType.String:

                    if (data == null)
                    {
                        return null;
                    }

                    var content = new StringContent(serializer.Serialize(data, _encoding), _encoding);
                    if (_defaultSerializer.HasMediaType)
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue(_defaultSerializer.MediaType);
                    }

                    return content;
                case ContentType.Forms:
                    return new FormUrlEncodedContent(formsParameters);
                case ContentType.ByteArray:
                    if (data == null)
                    {
                        return null;
                    }

                    var contentArray = new ByteArrayContent(data as byte[]);
                    contentArray.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return contentArray;
                case ContentType.MultipartFormData:
                    var multiPartContent = new MultipartFormDataContent();
                    foreach (var currentPart in multiParts)
                    {
                        if (currentPart is BytesMultiPartData currentBytesPart)
                        {
                            var bytesContent = new ByteArrayContent(currentBytesPart.Data);

                            if (string.IsNullOrWhiteSpace(currentBytesPart.Name) && string.IsNullOrWhiteSpace(currentBytesPart.FileName))
                            {
                                multiPartContent.Add(bytesContent);
                            }
                            else if (!string.IsNullOrWhiteSpace(currentBytesPart.Name))
                            {
                                multiPartContent.Add(bytesContent, currentBytesPart.Name);
                            }
                            else if (!string.IsNullOrWhiteSpace(currentBytesPart.Name) && !string.IsNullOrWhiteSpace(currentBytesPart.FileName))
                            {
                                multiPartContent.Add(bytesContent, currentBytesPart.Name, currentBytesPart.FileName);
                            }
                        }
                        else if (currentPart is StreamMultiPartData currentStreamPart)
                        {
                            var streamContent = new StreamContent(currentStreamPart.Data);

                            if (string.IsNullOrWhiteSpace(currentStreamPart.Name) && string.IsNullOrWhiteSpace(currentStreamPart.FileName))
                            {
                                multiPartContent.Add(streamContent);
                            }
                            else if (!string.IsNullOrWhiteSpace(currentStreamPart.Name))
                            {
                                multiPartContent.Add(streamContent, currentStreamPart.Name);
                            }
                            else if (!string.IsNullOrWhiteSpace(currentStreamPart.Name) && !string.IsNullOrWhiteSpace(currentStreamPart.FileName))
                            {
                                multiPartContent.Add(streamContent, currentStreamPart.Name, currentStreamPart.FileName);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                    return multiPartContent;
                default:
                    throw new NotImplementedException();
            }
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

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri uri, HttpContent content, IDeserializer deserializer, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();
            Stopwatch sw = new Stopwatch();
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    if (deserializer.HasMediaType)
                    {
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(deserializer.MediaType));
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