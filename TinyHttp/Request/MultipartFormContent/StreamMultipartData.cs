using System.IO;

namespace Tiny.RestClient
{
    internal class StreamMultipartData : MultipartData, IContent
    {
        public StreamMultipartData(Stream data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public Stream Data { get; }
    }
}