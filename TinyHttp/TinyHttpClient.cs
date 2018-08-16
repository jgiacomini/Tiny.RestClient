using Newtonsoft.Json;
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

namespace TinyHttp
{
    public class TinyHttpClient
    {
        #region Fields
        private readonly HttpClient _httpClient;

        private readonly string _serverAddress;

        private ISerializer _defaultSerializer;
        #endregion

        #region Logging events
        public event EventHandler<HttpSendingRequestEventArgs> SendingRequest;
        public event EventHandler<HttpReceivedResponseEventArgs> ReceivedResponse;
        public event EventHandler<FailedToGetResponseEventArgs> FailedToGetResponse;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="httpClient">The httpclient used</param>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(HttpClient httpClient, string serverAddress)
        {
            if (serverAddress == null)
            {
                throw new ArgumentNullException(nameof(serverAddress));
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (!serverAddress.EndsWith("/"))
            {
                serverAddress += "/";
            }

            _serverAddress = serverAddress;
            _httpClient = httpClient;
            AdditionalHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the additional headers.
        /// </summary>
        /// <value>
        /// The additional headers.
        /// </value>
        public Dictionary<string, string> AdditionalHeaders
        {
            get; private set;
        }

        public ISerializer DefaultSerializer
        {
            get
            {
                return _defaultSerializer;
            }

            private set
            {
                _defaultSerializer = value ?? throw new NullReferenceException();
            }
        }

        #region Get

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">URL : {requestUri}</exception>
        public async Task GetAsync(string route, CancellationToken cancellationToken = default)
        {
            var requestUri = BuildRequestUri(route);
            using (var response = await SendRequestAsync(HttpMethod.Get, requestUri, null, cancellationToken))
            {
                await ReadResponseAsync(response, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route">The route.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">URL : {requestUri}</exception>
        public async Task<T> GetAsync<T>(string route, CancellationToken cancellationToken = default)
        {
            var requestUri = BuildRequestUri(route);

            using (var response = await SendRequestAsync(HttpMethod.Get, requestUri, null, cancellationToken))
            {
                return await ReadResponseAsync<T>(response, cancellationToken);
            }
        }

        public async Task<Stream> GetStreamAsync(string route, CancellationToken cancellationToken = default)
        {
            var requestUri = BuildRequestUri(route);

            var response = await SendRequestAsync(HttpMethod.Get, requestUri, null, cancellationToken);
            var stream = await ReadResponseAsync(response, cancellationToken);
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            return stream;
        }
        #endregion

        #region Post

        public async Task<T> PostAsync<T>(IEnumerable<KeyValuePair<string, string>> data, CancellationToken cancellationToken)
        {
            var requestUri = new Uri(_serverAddress);

            var stringBuilder = new StringBuilder();
            foreach (var item in data)
            {
                stringBuilder.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}");
                if (item.Key != data.Last().Key)
                {
                    stringBuilder.Append("&");
                }
            }

            using (var content = new FormUrlEncodedContent(data))
            {
                HttpResponseMessage response = await SendRequestAsync(HttpMethod.Post, requestUri, content, cancellationToken);

                return await ReadResponseAsync<T>(response, cancellationToken);
            }
        }

        /// <summary>
        /// Post data
        /// </summary>
        /// <typeparam name="TResult">type of response</typeparam>
        /// <param name="route">route</param>
        /// <param name="data">data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task<TResult> PostAsync<TResult, TInput>(string route, TInput data, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);
            using (var response = await SendRequestAsync(HttpMethod.Post, requestUri, GetStringContent(data), cancellationToken))
            {
                return await ReadResponseAsync<TResult>(response, cancellationToken);
            }
        }

        /// <summary>
        /// Post a data
        /// </summary>
        /// <param name="route">the route</param>
        /// <param name="data">the data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task PostAsync<TInput>(string route, TInput data, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);

            using (var response = await SendRequestAsync(HttpMethod.Post, requestUri, GetStringContent(data), cancellationToken))
            {
                await ReadResponseAsync(response, cancellationToken);
            }
        }
        #endregion

        #region Put

        /// <summary>
        /// Put the data
        /// </summary>
        /// <typeparam name="TResult">type of result</typeparam>
        /// <typeparam name="TInput">type of input</typeparam>
        /// <param name="route">route</param>
        /// <param name="data">data to put</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task<TResult> PutAsync<TResult, TInput>(string route, TInput data, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);

            using (var response = await SendRequestAsync(HttpMethod.Put, requestUri, GetStringContent(data), cancellationToken))
            {
                return await ReadResponseAsync<TResult>(response, cancellationToken);
            }
        }

        /// <summary>
        /// Put the data async
        /// </summary>
        /// <param name="route">the route</param>
        /// <param name="data">the data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task PutAsync<T>(string route, T data, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);
            using (var response = await SendRequestAsync(HttpMethod.Put, requestUri, GetStringContent(data), cancellationToken))
            {
                await ReadResponseAsync(response, cancellationToken);
            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete async
        /// </summary>
        /// <param name="route">the route to delete</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task DeleteAsync(string route, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);

            using (var response = await SendRequestAsync(HttpMethod.Delete, requestUri, null, cancellationToken))
            {
                await ReadResponseAsync(response, cancellationToken);
            }
        }

        /// <summary>
        /// Delete async
        /// </summary>
        /// <param name="route">the route to delete</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task<T> DeleteAsync<T>(string route, CancellationToken cancellationToken)
        {
            var requestUri = BuildRequestUri(route);

            using (var response = await SendRequestAsync(HttpMethod.Delete, requestUri, null, cancellationToken))
            {
                return await ReadResponseAsync<T>(response, cancellationToken);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// Builds the request URI.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>the buided uri</returns>
        private Uri BuildRequestUri(string route)
        {
            return new Uri(string.Concat(_serverAddress, route));
        }

        /// <summary>
        /// Reads the response asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return the response</returns>
        private async Task<T> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var stream = await ReadResponseAsync(response, cancellationToken);

            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            using (var sr = new StreamReader(stream))
            {
                using (var jtr = new JsonTextReader(sr))
                {
                    var js = new JsonSerializer();

                    var searchResult = js.Deserialize<T>(jtr);
                    return searchResult;
                }
            }
        }

        /// <summary>
        /// Reads the response asynchronous.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a tas</returns>
        private async Task<Stream> ReadResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            Stream stream = null;
            string content = null;
            try
            {
                stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    return stream;
                }
                else
                {
                    content = await StreamToStringAsync(stream);
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
                    content = await sr.ReadToEndAsync();
                }
            }

            return content;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, Uri uri, HttpContent content, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid().ToString();
            Stopwatch sw = new Stopwatch();
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    // TODO : get from deserializer
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // TODO : add something to customize that stuff
                    request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
                    request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                    foreach (var item in AdditionalHeaders)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }

                    if (content != null)
                    {
                        request.Content = content;
                    }

                    OnSendingRequest(requestId, uri, httpMethod);
                    sw.Start();
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
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

        private StringContent GetStringContent<TData>(TData data)
        {
            var content = new StringContent(DefaultSerializer.Serialize(data), Encoding.UTF8);

            if (_defaultSerializer.HasMediaType)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(_defaultSerializer.MediaType);
            }

            return content;
        }

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