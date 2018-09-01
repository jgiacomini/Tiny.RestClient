using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IByteArrayResponseRequest
    /// </summary>
    public interface IByteArrayResponseRequest
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>Task of byte array</returns>
        Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}