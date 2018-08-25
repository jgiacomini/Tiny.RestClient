using System.Collections.Generic;

namespace Tiny.Http
{
    internal abstract class MultiPartData : ITinyContent
    {
        public string Name { get; protected set; }
        public string FileName { get; protected set; }
        public string ContentType { get; protected set; }
    }

    internal class MultiPartContent : List<MultiPartData>, ITinyContent
    {
        public MultiPartContent(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; protected set; }
    }
}