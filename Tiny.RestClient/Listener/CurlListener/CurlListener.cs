#define DEBUG
// We define debug symbol to be able to log in debug even if we are compiled in release mode
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// A listener which will create a postMan collection/>.
    /// </summary>
    public class CurlListener : IListener
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="CurlListener"/> class.
        /// </summary>
        public CurlListener()
        {
        }

        /// <inheritdoc/>
        public bool MeasureTime => false;

        /// <inheritdoc/>
        public Task OnFailedToReceiveResponseAsync(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime, CancellationToken cancellationToken)
        {
#if COMPLETED_TASK_NOT_SUPPORTED
            return TaskHelper.CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }

        /// <inheritdoc/>
        public Task OnReceivedResponseAsync(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime, CancellationToken cancellationToken)
        {
#if COMPLETED_TASK_NOT_SUPPORTED
            return TaskHelper.CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }

        /// <inheritdoc/>
        public async Task OnSendingRequestAsync(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            var headers = GetHeaders(httpRequestMessage);
            var body = await GetBodyAsync(httpRequestMessage);

            var bodyLenght = body == null ? 0 : body.Length;
            var stringBuilder = new StringBuilder(uri.OriginalString.Length + (headers.Count * 20) + bodyLenght);
            stringBuilder.Append($"curl -X {httpMethod.Method} \"{uri.OriginalString}\"");

            foreach (var item in headers)
            {
                stringBuilder.Append($"-H \"{item.Item1}: {item.Item2}\" ");
            }

            if (!string.IsNullOrEmpty(body))
            {
                body = body.Replace("\"", "\\\"");
                stringBuilder.Append($"-d \"{body}\"");
            }

            Debug.WriteLine(stringBuilder);
        }

        private List<Tuple<string, string>> GetHeaders(HttpRequestMessage httpRequestMessage)
        {
            var headers = new List<Tuple<string, string>>();
            foreach (var header in httpRequestMessage.Headers)
            {
                foreach (var currentValue in header.Value)
                {
                    headers.Add(new Tuple<string, string>(header.Key, currentValue));
                }
            }

            if (httpRequestMessage.Content != null)
            {
                foreach (var header in httpRequestMessage.Content.Headers)
                {
                    foreach (var currentValue in header.Value)
                    {
                        headers.Add(new Tuple<string, string>(header.Key, currentValue));
                    }
                }
            }

            return headers;
        }

        private async Task<string> GetBodyAsync(HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage.Content != null)
            {
                return await httpRequestMessage.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
