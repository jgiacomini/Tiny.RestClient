namespace Tiny.Http
{
    /// <summary>
    /// Class HttpSendingRequestEventArgs.
    /// </summary>
    /// <seealso cref="Tiny.Http.HttpEventArgsBase" />
    public class HttpSendingRequestEventArgs : HttpEventArgsBase
    {
        internal HttpSendingRequestEventArgs(string requestId, string uri, string method)
            : base(requestId, uri, method)
        {
        }
    }
}