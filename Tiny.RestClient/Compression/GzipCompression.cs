using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Gzip compression
    /// </summary>
    public class GzipCompression : ICompression
    {
        /// <inheritdoc/>
        public string ContentEncoding => "gzip";

        /// <inheritdoc/>
        public bool AddAcceptEncodingHeader
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public async Task<Stream> CompressAsync(Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            try
            {
                var compressedStream = new MemoryStream();

                using (var compressionStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
                {
                    await stream.CopyToAsync(compressionStream).ConfigureAwait(false);
                }

                return compressedStream;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <inheritdoc/>
        public Task<Stream> DecompressAsync(Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new GZipStream(stream, CompressionMode.Decompress));
        }
    }
}