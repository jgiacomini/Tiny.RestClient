#if !FILEINFO_NOT_SUPPORTED
using System.IO;

namespace Tiny.RestClient
{
    internal class FileContent : BaseContent<FileInfo>
    {
        public FileContent(FileInfo data, string contentType)
            : base(data, contentType)
        {
        }
    }
}
#endif