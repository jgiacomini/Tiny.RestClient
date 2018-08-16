using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public interface IDeserializableRequest
    {
        Task<TResult> PostAsync<TResult>(string route, CancellationToken cancellationToken = default);
        Task<TResult> GetAsync<TResult>(string route, CancellationToken cancellationToken = default);
        Task<TResult> PutAsync<TResult>(string route, CancellationToken cancellationToken = default);
        Task<TResult> PatchAsync<TResult>(string route, CancellationToken cancellationToken = default);
        Task<TResult> DeleteAsync<TResult>(string route, CancellationToken cancellationToken = default);
    }
}