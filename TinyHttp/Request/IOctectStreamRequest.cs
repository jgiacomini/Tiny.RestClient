using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IOctectStreamRequest
    /// </summary>
    /// <seealso cref="Tiny.Http.ICommonResquest" />
    public interface IOctectStreamRequest : ICommonResquest
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>Task of byte array</returns>
        Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}