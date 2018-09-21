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
                using (var decompressionStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    await decompressionStream.CopyToAsync(compressedStream, bufferSize, cancellationToken).ConfigureAwait(false);
                }

                return compressedStream;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <inheritdoc/>
        public async Task<Stream> DecompressAsync(Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            var decompressedStream = new MemoryStream();
            using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
            {
                await decompressionStream.CopyToAsync(decompressedStream, bufferSize, cancellationToken).ConfigureAwait(false);
            }

            return decompressedStream;
        }
    }
}