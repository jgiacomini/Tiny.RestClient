using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#if SUPPORTS_ASYNC_ENUMERABLE
using System.Collections.Generic;
#endif

namespace Tiny.RestClient
{
    /// <summary>
    /// Represents a request that is ready to be sent. Call one of the <c>ExecuteAs...Async</c> methods
    /// to send it and materialize the response in the desired shape.
    /// </summary>
    public interface IExecutableRequest
    {
        /// <summary>
        /// Sends the request and deserializes the response body into <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type the response body is deserialized into.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the deserialized response.</returns>
        /// <example>
        /// <code>
        /// List&lt;City&gt; cities = await client.GetRequest("City/All").ExecuteAsync&lt;List&lt;City&gt;&gt;();
        /// </code>
        /// </example>
        Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and deserializes the response body into <typeparamref name="TResult"/> using a specific formatter.
        /// </summary>
        /// <typeparam name="TResult">The type the response body is deserialized into.</typeparam>
        /// <param name="formatter">The formatter used to override the deserialization.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the deserialized response.</returns>
        Task<TResult> ExecuteAsync<TResult>(IFormatter formatter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and returns the raw response body as a <see cref="Stream"/> (no deserialization).
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the response <see cref="Stream"/>.</returns>
        Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and returns the raw response body as a <see cref="string"/> (no deserialization).
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the response body as a string.</returns>
        /// <example>
        /// <code>
        /// string json = await client.GetRequest("City/All").ExecuteAsStringAsync();
        /// </code>
        /// </example>
        Task<string> ExecuteAsStringAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and returns the raw response body as a byte array (no deserialization).
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the response body as a byte array.</returns>
        Task<byte[]> ExecuteAsByteArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and returns the underlying <see cref="HttpResponseMessage"/> unmodified.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the raw <see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> ExecuteAsHttpResponseMessageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and writes the response body to a file on disk.
        /// </summary>
        /// <param name="path">The destination file path.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the <see cref="FileInfo"/> of the downloaded file.</returns>
        /// <example>
        /// <code>
        /// FileInfo file = await client.GetRequest("City/map.pdf").DownloadFileAsync(@"c:\map.pdf");
        /// </code>
        /// </example>
        Task<FileInfo> DownloadFileAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request and ignores the response body.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes when the request has been sent.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
#if SUPPORTS_ASYNC_ENUMERABLE

        /// <summary>
        /// Executes the request as a Server-Sent Events (SSE) stream.
        /// The connection stays open and each <see cref="ServerSentEvent"/> is yielded as it is received.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to stop listening to the stream.</param>
        /// <returns>An async stream of <see cref="ServerSentEvent"/>.</returns>
        /// <example>
        /// <code>
        /// await foreach (var sse in client.GetRequest("notifications/stream").ExecuteAsSSEAsync(cancellationToken))
        /// {
        ///     Console.WriteLine($"{sse.Id} {sse.Event} {sse.Data} {sse.Retry}");
        /// }
        /// </code>
        /// </example>
        IAsyncEnumerable<ServerSentEvent> ExecuteAsSSEAsync(CancellationToken cancellationToken = default);
#endif
    }
}