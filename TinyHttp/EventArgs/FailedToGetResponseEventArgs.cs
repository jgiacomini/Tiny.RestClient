using System;

namespace TinyHttp
{
    public class FailedToGetResponseEventArgs : HttpEventArgsBase
    {
        public FailedToGetResponseEventArgs(string requestId, string uri, string method, Exception exception, TimeSpan elapsedTime)
            : base(requestId, uri, method)
        {
            Exception = exception;
            ElapsedTime = elapsedTime;
        }

        public Exception Exception { get; }
        public TimeSpan ElapsedTime { get; }
    }
}