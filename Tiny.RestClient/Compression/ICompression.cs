using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represent a way to manage compression / decompression.
    /// </summary>
    public interface ICompression
    {
        /// <summary>
        /// Gets the content encoding of compression.
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// Gets or sets if the compression system add accept headers when the request is sended.
        /// </summary>
        bool AddAcceptEncodingHeader { get; set; }

        /// <summary>
        ///  Compresses the stream.
        /// </summary>
        /// <param name="stream">the stream to compress.</param>
        /// <param name="bufferSize">the buffer size to use.</param>
        /// <param name="cancellationToken">the cancellation token.</param>
        /// <returns>returns stream compressed.</returns>
        Task<Stream> CompressAsync(Stream stream, int bufferSize, CancellationToken cancellationToken);

        /// <summary>
        ///  Decompresses the stream.
        /// </summary>
        /// <param name="stream">the stream to decompress.</param>
        /// <param name="bufferSize">the buffer size to use.</param>
        /// <param name="cancellationToken">the cancellation token.</param>
        /// <returns>returns stream compressed.</returns>
        Task<Stream> DecompressAsync(Stream stream, int bufferSize, CancellationToken cancellationToken);
    }
}
