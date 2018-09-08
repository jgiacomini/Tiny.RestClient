using System;
using System.Diagnostics;
using System.Net.Http;

namespace Tiny.Http
{
    /// <summary>
    /// A listener which will trace all requests with <see cref="Debug.WriteLine(object)"/>
    /// </summary>
    public class DebugListener : IListener
    {
        /// <inheritdoc/>
        public bool MeasureTime => true;

        /// <inheritdoc/>
        public void OnFailedToReceiveResponse(Uri uri, HttpMethod httpMethod, Exception exception, TimeSpan? elapsedTime)
        {
            Debug.WriteLine($"FailedToGetResponse Method = {httpMethod}, Uri = {exception}, ElapsedTime = {ToReadableString(elapsedTime)}");
        }

        /// <inheritdoc/>
        public void OnReceivedResponse(Uri uri, HttpMethod httpMethod, HttpResponseMessage response, TimeSpan? elapsedTime)
        {
            Debug.WriteLine($"Received Method = {httpMethod}, Uri = {uri}, StatusCode = {response.StatusCode}, ElapsedTime = {ToReadableString(elapsedTime)}");
        }

        /// <inheritdoc/>
        public void OnSendingRequest(Uri uri, HttpMethod httpMethod, HttpRequestMessage httpRequestMessage)
        {
            Debug.WriteLine($"Sending Method = {httpMethod}, Uri = {uri}");
        }

        private string ToReadableString(TimeSpan? span)
        {
            if (span == null)
            {
                return string.Empty;
            }

            var spanNotNullable = span.Value;

            string formatted = string.Format(
                "{0}{1}{2}",
                spanNotNullable.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", spanNotNullable.Hours, spanNotNullable.Hours == 1 ? string.Empty : "s") : string.Empty,
                spanNotNullable.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", spanNotNullable.Minutes, spanNotNullable.Minutes == 1 ? string.Empty : "s") : string.Empty,
                spanNotNullable.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", spanNotNullable.Seconds, spanNotNullable.Seconds == 1 ? string.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", "))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }

            if (string.IsNullOrEmpty(formatted))
            {
                formatted = "0 seconds";
            }

            return formatted;
        }
    }
}
