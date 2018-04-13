using System.Threading;
using System.Threading.Tasks;

namespace TinyHttp
{
    public interface ISerializableFluent
    {
        ISerializableFluent SerializeWith(ISerializer serializer);
        ISerializableFluent DeserializeWith(IDeserializer deserializer);
        Task<TResult> PostAsync<TResult>(string route, CancellationToken cancellationToken = default);
        Task<TResult> PostAsync<TResult, TInput>(string route, TInput data, CancellationToken cancellationToken = default);
        Task<T> PostAsync<T>(string route, byte[] data, CancellationToken cancellationToken = default);
    }
}