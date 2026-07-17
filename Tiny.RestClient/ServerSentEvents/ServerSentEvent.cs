#if SUPPORTS_ASYNC_ENUMERABLE
namespace Tiny.RestClient
{
    /// <summary>
    /// Represents a single event received from a Server-Sent Events (SSE) stream.
    /// </summary>
    public class ServerSentEvent
    {
        internal ServerSentEvent(string id, string eventType, string data, int? retry)
        {
            Id = id;
            Event = eventType;
            Data = data;
            Retry = retry;
        }

        /// <summary>
        /// Gets the last event ID (<c>id</c> field), or <c>null</c> if not provided.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the event type (<c>event</c> field). Defaults to <c>message</c> when not provided by the server.
        /// </summary>
        public string Event { get; }

        /// <summary>
        /// Gets the event payload (<c>data</c> field). Multiple <c>data</c> lines are joined with a line feed.
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Gets the reconnection time in milliseconds (<c>retry</c> field), or <c>null</c> if not provided.
        /// </summary>
        public int? Retry { get; }
    }
}
#endif
