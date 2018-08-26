using System.IO;

namespace Tiny.Http
{
    internal class StreamContent : BaseContent<Stream>
    {
        public StreamContent(Stream data, string contentType)
            : base(data, contentType)
        {
        }
    }
}