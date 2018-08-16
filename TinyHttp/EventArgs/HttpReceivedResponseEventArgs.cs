using System;
using System.Net;

namespace Tiny.Http
{
    public class HttpReceivedResponseEventArgs : HttpEventArgsBase
    {
        public HttpReceivedResponseEventArgs(string requestId, string uri, string method, HttpStatusCode statusCode, string reasonPhrase, TimeSpan elapsedTime)
            : base(requestId, uri, method)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ElapsedTime = elapsedTime;
        }

        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public TimeSpan ElapsedTime { get; }
    }
}