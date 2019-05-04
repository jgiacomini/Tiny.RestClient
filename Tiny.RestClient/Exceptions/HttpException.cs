using System;
using System.Net;
using System.Net.Http.Headers;

namespace Tiny.RestClient
{
    /// <summary>
    /// A <see cref="HttpException"/>.
    /// </summary>
    /// <seealso cref="TinyRestClientException" />
    public class HttpException : TinyRestClientException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class.
        /// </summary>
        /// <param name="uri">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="reasonPhrase">The reason phrase.</param>
        /// <param name="headers">The headers of the request.</param>
        /// <param name="content">The content.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseHeaders">The headers of response.</param>
        /// <param name="ex">The ex.</param>
        internal HttpException(
            Uri uri,
            string verb,
            string reasonPhrase,
            HttpRequestHeaders headers,
            string content,
            HttpStatusCode statusCode,
            HttpResponseHeaders responseHeaders,
            Exception ex)
        : base($"Response status code does not indicate success. Url : {uri.ToString()}, Verb : {verb}, StatusCode : {statusCode}, ReasonPhrase : {reasonPhrase}", ex)
        {
            Verb = verb;
            Uri = uri;
            Content = content;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;
            ResponseHeaders = responseHeaders;
        }

        /// <summary>
        /// Gets the verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public string Verb { get; private set; }

        /// <summary>
        /// Gets the headers of sended request.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public HttpRequestHeaders Headers { get; private set; }

        /// <summary>
        /// Gets the reason phrase.
        /// </summary>
        /// <value>
        /// The reason phrase.
        /// </value>
        public string ReasonPhrase { get;  private set; }

        /// <summary>
        /// Gets the Uri.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; private set; }

        /// <summary>
        /// Gets the response headers of sended request.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public HttpResponseHeaders ResponseHeaders { get; private set; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; private set; }
    }
}
