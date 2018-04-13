using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace TinyHttp
{
    /// <summary>
    /// A httpException
    /// </summary>
    /// <seealso cref="System.Exception" />
    /// <summary>
    /// A httpexception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class HttpException : TinyHttpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="reasonPhrase">The reason phrase.</param>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="content">The content.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="ex">The ex.</param>
        public HttpException(
            string message,
            HttpRequestHeaders headers,
            string reasonPhrase,
            string url,
            string verb,
            string content,
            HttpStatusCode statusCode,
            Exception ex)
        : base($"URL : {url}, Headers {HeadersToString(headers)}, Verb : {verb}, StatusCode : {statusCode}, message : {message}, Response Content : {content}", ex)
        {
            Verb = verb;
            Url = url;
            Content = content;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        private static string HeadersToString(HttpRequestHeaders headers)
        {
            var stringBuilder = new StringBuilder(headers.Count() * 50);
            foreach (var header in headers)
            {
                stringBuilder.AppendLine($"Header key : {header.Key}");

                var countValuesHeader = header.Value.Count();
                if (countValuesHeader > 1)
                {
                    stringBuilder.AppendLine($"Header values ({countValuesHeader}) :");
                }
                else
                {
                    stringBuilder.AppendLine($"Header value :");
                }

                foreach (var value in header.Value)
                {
                    stringBuilder.AppendLine(value);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public string Verb { get; private set; }

        /// <summary>
        /// Gets the headers of sended request
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public HttpRequestHeaders Headers { get; private set; }

        /// <summary>
        /// Gets or sets the reason phrase.
        /// </summary>
        /// <value>
        /// The reason phrase.
        /// </value>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; private set; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; private set; }
    }
}
