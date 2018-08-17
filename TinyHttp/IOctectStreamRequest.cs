using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public interface IOctectStreamRequest : ICommonResquest
    {
        Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}