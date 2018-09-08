using System;
using System.Net.Http;

namespace Tiny.Http
{
    /// <summary>
    /// Represent a request listener
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Enable the measure of time between requests
        /// </summary>
        bool MeasureTime { get; }

        /// <summary>
        /// Invoked when a request is sending
        /// </summary>
        /// <param name="uri">uri of the request</param>
        /// <param name="httpMethod">verb of the request</param>
        /// <param name="httpRequestMessage">message sended to server</param>
        void OnSendingRequest(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage);

        /// <summary>
        /// Invoked when received a response from server
        /// </summary>
        /// <param name="uri">uri of the request</param>
        /// <param name="httpMethod">verb of the request</param>
        /// <param name="response">response of the server</param>
        /// <param name="elapsedTime">time ellapsed between the send of request and response of server</param>
        void OnReceivedResponse(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime);

        /// <summary>
        /// Invoke when a request failed to be invoked
        /// </summary>
        /// <param name="uri">uri of the request</param>
        /// <param name="httpMethod">verb of the request</param>
        /// <param name="exception">exception</param>
        /// <param name="elapsedTime">time ellapsed between the send of request and response of server (can be null if no listener measure time)</param>
        void OnFailedToReceiveResponse(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime);
    }
}
