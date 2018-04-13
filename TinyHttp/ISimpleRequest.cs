using System.Threading;
using System.Threading.Tasks;

namespace TinyHttp
{
    public interface ISimpleRequest
    {
        Task PostAsync(string route, CancellationToken cancellationToken = default);
        Task GetAsync(string route, CancellationToken cancellationToken = default);
        Task PutAsync(string route, CancellationToken cancellationToken = default);
        Task PatchAsync(string route, CancellationToken cancellationToken = default);
        Task DeleteAsync(string route, CancellationToken cancellationToken = default);
    }
}