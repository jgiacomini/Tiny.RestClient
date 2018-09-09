using System.Collections.Generic;

namespace Tiny.RestClient
{
    internal class MultipartContent : List<MultipartData>, IContent
    {
        public MultipartContent(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; protected set; }
    }
}