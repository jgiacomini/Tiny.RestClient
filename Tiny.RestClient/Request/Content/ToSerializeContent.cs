using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    internal class ToSerializeContent<T> : BaseContent<T>, IToSerializeContent
        where T : class
    {
        public ToSerializeContent(T data, IFormatter serializer, ICompression compression)
            : base(data, null)
        {
            Serializer = serializer;
            Compression = compression;
        }

        public Type TypeToSerialize => typeof(T);

        public Task<string> GetSerializedStringAsync(IFormatter serializer, Encoding encoding, CancellationToken cancellationToken)
        {
            try
            {
                return serializer.SerializeAsync<T>(Data, encoding, cancellationToken);
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