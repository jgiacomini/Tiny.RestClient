using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class TinyHttpClient : IDisposable
    {
        #region Fields

        /// <summary>
        /// The server address
        /// </summary>
        private readonly string _serverAddress;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        public TinyHttpClient(string serverAddress)
        {
            if (serverAddress == null)
            {
                throw new ArgumentNullException(nameof(serverAddress));
            }

            if (!serverAddress.EndsWith("/"))
            {
                serverAddress += "/";
            }

            _serverAddress = serverAddress;
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

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route">The route.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">URL : {requestUri}</exception>
        public async Task<T> GetAsync<T>(string route, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync(new Uri(requestUri), cancellationToken))
                {
                    return await ReadResponseAsync<T>(response, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">URL : {requestUri}</exception>
        public async Task GetAsync(string route, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);

                using (var response = await client.GetAsync(new Uri(requestUri), cancellationToken))
                {
                    await ReadResponseAsync(response, cancellationToken);
                }
            }
        }

        #region Post

        public async Task<T> PostAsync<T>(IEnumerable<KeyValuePair<string, string>> data, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = _serverAddress;

                var stringBuilder = new StringBuilder();
                foreach (var item in data)
                {
                    stringBuilder.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}");
                    if (item.Key != data.Last().Key)
                    {
                        stringBuilder.Append($"&");
                    }
                }

                using (var content = new FormUrlEncodedContent(data))
                {
                    HttpResponseMessage response = await client.PostAsync(requestUri, content);

                    return await ReadResponseAsync<T>(response, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Post data
        /// </summary>
        /// <typeparam name="T">type of response</typeparam>
        /// <param name="route">route</param>
        /// <param name="data">data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task<T> PostAsync<T>(string route, object data, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);
                var serializedContent = await Task.Factory
                        .StartNew(() => JsonConvert.SerializeObject(data), cancellationToken)
                        .ConfigureAwait(false);
                var content = new StringContent(serializedContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await client.PostAsync(new Uri(requestUri), content, cancellationToken))
                {
                    return await ReadResponseAsync<T>(response, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Post a data
        /// </summary>
        /// <param name="route">the route</param>
        /// <param name="data">the data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task PostAsync(string route, object data, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);
                var serializedContent = await Task.Factory
                        .StartNew(() => JsonConvert.SerializeObject(data), cancellationToken)
                        .ConfigureAwait(false);
                var content = new StringContent(serializedContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await client.PostAsync(new Uri(requestUri), content, cancellationToken))
                {
                    await ReadResponseAsync(response, cancellationToken);
                }
            }
        }
        #endregion

        #region Put

        /// <summary>
        /// Put the data
        /// </summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="route">route</param>
        /// <param name="data">data to put</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task<T> PutAsync<T>(string route, object data, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);
                var serializedContent = await Task.Factory
                        .StartNew(() => JsonConvert.SerializeObject(data), cancellationToken)
                        .ConfigureAwait(false);
                var content = new StringContent(serializedContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await client.PutAsync(new Uri(requestUri), content, cancellationToken))
                {
                    return await ReadResponseAsync<T>(response, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Put the data async
        /// </summary>
        /// <param name="route">the route</param>
        /// <param name="data">the data to post</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a task</returns>
        public async Task PutAsync(string route, object data, CancellationToken cancellationToken)
        {
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = BuildRequestUri(route);
                var serializedContent = await Task.Factory
                        .StartNew(() => JsonConvert.SerializeObject(data), cancellationToken)
                        .ConfigureAwait(false);
                var content = new StringContent(serializedContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await client.PutAsync(new Uri(requestUri), content, cancellationToken))
                {
                    await ReadResponseAsync(response, cancellationToken);
                }
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
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = string.Concat(_serverAddress, route);

                using (var response = await client.DeleteAsync(new Uri(requestUri), cancellationToken))
                {
                    await ReadResponseAsync(response, cancellationToken);
                }
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
            using (var client = GetConfiguredHttpClient())
            {
                var requestUri = string.Concat(_serverAddress, route);

                using (var response = await client.DeleteAsync(new Uri(requestUri), cancellationToken))
                {
                    return await ReadResponseAsync<T>(response, cancellationToken);
                }
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// Gets the configured HTTP client.
        /// </summary>
        /// <returns>return the http client</returns>
        private HttpClient GetConfiguredHttpClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
            foreach (var header in AdditionalHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return client;
        }

        /// <summary>
        /// Builds the request URI.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>the buided uri</returns>
        private string BuildRequestUri(string route)
        {
            return string.Concat(_serverAddress, route);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}