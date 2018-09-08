using System;
using System.Net.Http;

namespace Tiny.Http
{
    public interface IListener
    {
        bool MeasureTime { get; }

        void OnSendingRequest(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage);
        void OnReceivedResponse(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime);
        void OnFailedToReceiveResponse(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime);
    }
}
