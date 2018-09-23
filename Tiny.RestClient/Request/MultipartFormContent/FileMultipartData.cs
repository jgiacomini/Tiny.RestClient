#if !FILEINFO_NOT_SUPPORTED
using System.IO;

namespace Tiny.RestClient
{
    internal class FileMultipartData : MultipartData, IContent
    {
        public FileMultipartData(FileInfo data, string name, string fileName, string contentType)
            : base(name, fileName, contentType)
        {
            Data = data;
        }

        public FileInfo Data { get; set; }
    }
}
#endif