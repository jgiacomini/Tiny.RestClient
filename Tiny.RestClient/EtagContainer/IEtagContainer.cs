using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Entity Tag container
    /// </summary>
    public interface IEtagContainer
    {
        /// <summary>
        /// Get the existing Etag.
        /// </summary>
        /// <param name="uri">the uri</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return the etag if found. If not return null.</returns>
        Task<string> GetExistingEtagAsync(Uri uri, CancellationToken cancellationToken);

        /// <summary>
        /// Get data of specific uri
        /// </summary>
        /// <param name="uri">the uri</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>return the <see cref="Stream"/> of data</returns>
        Task<Stream> GetDataAsync(Uri uri, CancellationToken cancellationToken);

        /// <summary>
        /// S
        /// </summary>
        /// <param name="uri">the uri</param>
        /// <param name="etag">the etag of data</param>
        /// <param name="stream"><see cref="Stream"/> of data to store</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SaveDataAsync(Uri uri, string etag, Stream stream, CancellationToken cancellationToken);
    }
}