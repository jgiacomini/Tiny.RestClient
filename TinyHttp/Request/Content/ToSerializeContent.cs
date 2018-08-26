using System.IO;
using System.Text;

namespace Tiny.Http
{
    internal class ToSerializeContent<T> : BaseContent<T>, IToSerializeContent
    {
        public ToSerializeContent(T data, IFormatter serializer)
            : base(data, null)
        {
            Serializer = serializer;
        }

        public string GetSerializedStream(IFormatter serializer, Encoding encoding)
        {
            return serializer.Serialize<T>(Data, encoding);
        }

        public IFormatter Serializer { get; private set; }
    }
}