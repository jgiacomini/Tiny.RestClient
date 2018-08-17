using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public interface IExecutableRequest
    {
        Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default);
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}