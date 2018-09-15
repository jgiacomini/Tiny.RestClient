using System;
using System.Text;

namespace Tiny.RestClient
{
    internal class ToSerializeMultipartData<T> : MultipartData, IToSerializeContent
    {
        public ToSerializeMultipartData(T data, string name, string fileName, IFormatter serializer)
            : base(name, fileName, null)
        {
            Data = data;
        }

        public T Data { get; }

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
    }
}