using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public interface IStreamRequest : ICommonResquest
    {
        Task<Stream> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}