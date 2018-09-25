using Microsoft.AspNetCore.ResponseCompression;
using System.IO;
using System.IO.Compression;

namespace Tiny.RestClient.ForTest.Api.CompressionProvider
{
    public class DeflateCompressionProvider : ICompressionProvider
    {
        private readonly CompressionLevel _compressionLevel;
        public DeflateCompressionProvider(CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            _compressionLevel = compressionLevel;
        }

        public string EncodingName => "deflate";
        public bool SupportsFlush => true;
        public Stream CreateStream(Stream outputStream)
        {
            return new BrotliStream(outputStream, CompressionMode.Compress);
        }
    }
}
