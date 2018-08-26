using System.IO;

namespace Tiny.Http
{
    internal class StreamMultiPartData : MultiPartData, ITinyContent
    {
        public StreamMultiPartData(Stream data, string name, string fileName, string contentType)
        {
            Data = data;
            Name = name;
            FileName = fileName;
            ContentType = contentType;
        }

        public Stream Data { get; }
    }
}