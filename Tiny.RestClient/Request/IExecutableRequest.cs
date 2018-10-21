using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IExecutableRequest
    /// </summary>
    public interface IExecutableRequest
    {
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResult">The type of the TResult.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><see cref="Task{TResult}"/></returns>
        Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResult">The type of the TResult.</typeparam>
        /// <param name="formatter">Allow to override the formatter use for the deserialization.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of TResukt</returns>
        Task<TResult> ExecuteAsync<TResult>(IFormatter formatter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><see cref="Task{Stream}"/></returns>
        Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of string</returns>
        Task<string> ExecuteAsStringAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of byte array</returns>
        Task<byte[]> ExecuteAsByteArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of <see cref="HttpResponseMessage"/></returns>
        Task<HttpResponseMessage> ExecuteAsHttpResponseMessageAsync(CancellationToken cancellationToken = default);
#if !FILEINFO_NOT_SUPPORTED
        /// <summary>
        /// Download file async
        /// </summary>
        /// <param name="path">path to download</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return a file info</returns>
        Task<FileInfo> DownloadFileAsync(string path, CancellationToken cancellationToken = default);
#endif

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}