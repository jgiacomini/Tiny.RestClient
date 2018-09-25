using Microsoft.AspNetCore.ResponseCompression;
using System.IO;
using System.IO.Compression;

namespace Tiny.RestClient.ForTest.Api.CompressionProvider
{
    public class BrotliCompressionProvider : ICompressionProvider
    {
        private readonly CompressionLevel _compressionLevel;
        public BrotliCompressionProvider(CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            _compressionLevel = compressionLevel;
        }

        public string EncodingName => "br";
        public bool SupportsFlush => true;
        public Stream CreateStream(Stream outputStream)
        {
            return new BrotliStream(outputStream, _compressionLevel);
        }
    }
}
