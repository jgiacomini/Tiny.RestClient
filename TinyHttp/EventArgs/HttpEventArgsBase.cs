using System;

namespace Tiny.Http
{
    public abstract class HttpEventArgsBase : EventArgs
    {
        protected HttpEventArgsBase(string requestId, string uri, string method)
        {
            RequestId = requestId;
            Uri = uri;
            Method = method;
        }

        public string RequestId { get; }
        public string Uri { get; }
        public string Method { get; }
    }
}