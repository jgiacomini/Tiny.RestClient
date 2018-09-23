using System;
using System.Text;

namespace Tiny.RestClient
{
    internal class ToSerializeContent<T> : BaseContent<T>, IToSerializeContent
    {
        public ToSerializeContent(T data, IFormatter serializer, ICompression compression)
            : base(data, null)
        {
            Serializer = serializer;
            Compression = compression;
        }

        public Type TypeToSerialize => typeof(T);

        public string GetSerializedString(IFormatter serializer, Encoding encoding)
        {
            try
            {
                return serializer.Serialize<T>(Data, encoding);
            }
            catch (Exception ex)
            {
                throw new SerializeException(typeof(T), ex);
            }
        }

        public IFormatter Serializer { get; private set; }

        public ICompression Compression { get; private set; }
    }
}