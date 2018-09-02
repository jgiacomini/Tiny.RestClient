using System.IO;

namespace Tiny.Http
{
    internal class FileContent : BaseContent<FileInfo>
    {
        public FileContent(FileInfo data, string contentType)
            : base(data, contentType)
        {
        }
    }
}