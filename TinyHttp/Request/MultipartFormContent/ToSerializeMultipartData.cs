using System.Text;

namespace Tiny.Http
{
    internal class ToSerializeMultipartData<T> : MultipartData, IToSerializeContent
    {
        public ToSerializeMultipartData(T data, string name, string fileName, IFormatter serializer)
            : base(name, fileName, null)
        {
            Data = data;
        }

        public T Data { get; }

        public string GetSerializedStream(IFormatter serializer, Encoding encoding)
        {
            return serializer.Serialize<T>(Data, encoding);
        }

        public IFormatter Serializer { get; private set; }
    }
}