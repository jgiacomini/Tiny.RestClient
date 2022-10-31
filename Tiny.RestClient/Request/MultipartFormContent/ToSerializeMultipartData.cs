using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public Task<string> GetSerializedStringAsync(IFormatter serializer, Encoding encoding, CancellationToken cancellationToken)
        {
            return serializer.SerializeAsync<T>(Data, encoding, cancellationToken);
        }

        public IFormatter Serializer { get; private set; }
        public ICompression Compression { get; private set; }
    }
}