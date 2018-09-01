using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IStreamRequest
    /// </summary>
    public interface IStreamRequest
    {
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <returns>Task of Stream</returns>
        Task<Stream> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}