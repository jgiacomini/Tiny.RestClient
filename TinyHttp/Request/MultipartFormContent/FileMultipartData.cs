using System.IO;

namespace Tiny.Http
{
    internal class FileMultipartData : MultipartData, ITinyContent
    {
        public FileMultipartData(FileInfo data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public FileInfo Data { get; set; }
    }
}