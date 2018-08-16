namespace TinyHttp
{
    public class HttpSendingRequestEventArgs : HttpEventArgsBase
    {
        public HttpSendingRequestEventArgs(string requestId, string uri, string method)
            : base(requestId, uri, method)
        {
        }
    }
}