using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public interface IFluent : ISerializableFluent, ISimpleRequest
    {
        Dictionary<string, string> DefaultHeaders { get; }
        IDeserializer DefaultDeserializer { get; }
        ISerializer DefaultSerializer { get; }

        IFluent AddHeader(string key, string value);
        IFluent AddQueryParameter(string key, string value);
        IFluent AddFormParameter(string key, string value);
        IFluent AddFormParameter(IEnumerable<KeyValuePair<string, string>> datas);
    }

    public class FluentClient : IFluent
    {
        public Dictionary<string, string> DefaultHeaders => throw new System.NotImplementedException();

        public IDeserializer DefaultDeserializer => throw new System.NotImplementedException();

        public ISerializer DefaultSerializer => throw new System.NotImplementedException();

        public IFluent AddFormParameter(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public IFluent AddFormParameter(IEnumerable<KeyValuePair<string, string>> datas)
        {
            throw new System.NotImplementedException();
        }

        public IFluent AddHeader(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public IFluent AddQueryParameter(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ISerializableFluent DeserializeWith(IDeserializer deserializer)
        {
            throw new System.NotImplementedException();
        }

        public Task GetAsync(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task PatchAsync(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult> PostAsync<TResult>(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult> PostAsync<TResult, TInput>(string route, TInput data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> PostAsync<T>(string route, byte[] data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task PostAsync(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task PutAsync(string route, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ISerializableFluent SerializeWith(ISerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}