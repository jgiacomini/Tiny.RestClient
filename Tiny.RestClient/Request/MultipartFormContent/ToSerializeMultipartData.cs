using System;
using System.Text;

namespace Tiny.RestClient
{
    internal class ToSerializeMultipartData<T> : MultipartData, IToSerializeContent
        where T : class
    {
        public ToSerializeMultipartData(T data, string name, string fileName, IFormatter serializer, ICompression compression)
            : base(name, fileName, null)
        {
            Data = data;
            Serializer = serializer;
            Compression = compression;
        }

        public Type TypeToSerialize => typeof(T);
        public T Data { get; }

        public string GetSerializedString(IFormatter serializer, Encoding encoding)
        {
            return serializer.Serialize<T>(Data, encoding);
        }

        public IFormatter Serializer { get; private set; }
        public ICompression Compression { get; private set; }
    }
}