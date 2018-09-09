using System.IO;

namespace Tiny.RestClient
{
    internal class StreamContent : BaseContent<Stream>
    {
        public StreamContent(Stream data, string contentType)
            : base(data, contentType)
        {
        }
    }
}