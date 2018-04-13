using System;

namespace TinyHttp
{
    public class ConnectionException : TinyHttpException
    {
        public ConnectionException(string message, string url, string verb, Exception innerException)
            : base(message, innerException)
        {
            Url = url;
            Verb = verb;
        }

        public string Url
        {
            get => (string)Data[nameof(Url)];
            private set => Data[nameof(Url)] = value;
        }

        public string Verb
        {
            get => (string)Data[nameof(Verb)];
            private set => Data[nameof(Verb)] = value;
        }
    }
}
