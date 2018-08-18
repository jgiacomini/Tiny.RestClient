using System;
using System.Net;

namespace Tiny.Http
{
    /// <summary>
    /// Class HttpReceivedResponseEventArgs.
    /// </summary>
    /// <seealso cref="Tiny.Http.HttpEventArgsBase" />
    public class HttpReceivedResponseEventArgs : HttpEventArgsBase
    {
        internal HttpReceivedResponseEventArgs(string requestId, string uri, string method, HttpStatusCode statusCode, string reasonPhrase, TimeSpan elapsedTime)
            : base(requestId, uri, method)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the reason phrase.
        /// </summary>
        /// <value>The reason phrase.</value>
        public string ReasonPhrase { get; }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        /// <value>The elapsed time.</value>
        public TimeSpan ElapsedTime { get; }
    }
}