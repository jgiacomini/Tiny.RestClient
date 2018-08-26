using System.IO;
using System.Text;

namespace Tiny.Http
{
    internal class ToSerializeContent<T> : BaseContent<T>, IToSerializeContent
    {
        public ToSerializeContent(T data, ISerializer serializer)
            : base(data, null)
        {
            Serializer = serializer;
        }

        public string GetSerializedStream(ISerializer serializer, Encoding encoding)
        {
            return serializer.Serialize<T>(Data, encoding);
        }

        public ISerializer Serializer { get; private set; }
    }
}