using System.Text;

namespace Tiny.Http
{
    internal class ToSerializeMultiPartData<T> : MultiPartData, IToSerializeContent
    {
        public ToSerializeMultiPartData(T data, string name, string fileName, IFormatter serializer)
        {
            Data = data;
            Name = name;
            FileName = fileName;
            Serializer = serializer;
        }

        public T Data { get; }

        public string GetSerializedStream(IFormatter serializer, Encoding encoding)
        {
            return serializer.Serialize<T>(Data, encoding);
        }

        public IFormatter Serializer { get; private set; }
    }
}