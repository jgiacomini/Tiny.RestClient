using System.Collections.Generic;

namespace Tiny.Http
{
    internal class MultipartContent : List<MultipartData>, ITinyContent
    {
        public MultipartContent(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; protected set; }
    }
}