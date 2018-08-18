using System;

namespace Tiny.Http
{
    /// <summary>
    /// Class HttpEventArgsBase.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public abstract class HttpEventArgsBase : EventArgs
    {
        internal HttpEventArgsBase(string requestId, string uri, string method)
        {
            RequestId = requestId;
            Uri = uri;
            Method = method;
        }

        /// <summary>
        /// Gets the request identifier.
        /// </summary>
        /// <value>The request identifier.</value>
        public string RequestId { get; }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public string Method { get; }
    }
}