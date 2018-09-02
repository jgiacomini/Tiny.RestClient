using System.IO;

namespace Tiny.Http
{
    internal class StreamMultipartData : MultipartData, ITinyContent
    {
        public StreamMultipartData(Stream data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public Stream Data { get; }
    }
}