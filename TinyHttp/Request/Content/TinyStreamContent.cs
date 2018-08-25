using System.IO;

namespace Tiny.Http
{
    internal class TinyStreamContent : BaseContent<Stream>
    {
        public TinyStreamContent(Stream data, string contentType)
            : base(data, contentType)
        {
        }
    }
}