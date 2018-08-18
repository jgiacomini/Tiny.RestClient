using System;

namespace Tiny.Http
{
    /// <summary>
    /// Class FailedToGetResponseEventArgs.
    /// </summary>
    /// <seealso cref="Tiny.Http.HttpEventArgsBase" />
    public class FailedToGetResponseEventArgs : HttpEventArgsBase
    {
        internal FailedToGetResponseEventArgs(string requestId, string uri, string method, Exception exception, TimeSpan elapsedTime)
            : base(requestId, uri, method)
        {
            Exception = exception;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        /// <value>The elapsed time.</value>
        public TimeSpan ElapsedTime { get; }
    }
}