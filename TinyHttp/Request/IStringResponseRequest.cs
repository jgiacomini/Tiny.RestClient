using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IStringResponseRequest
    /// </summary>
    public interface IStringResponseRequest
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>Task of <see cref="string"/></returns>
        Task<string> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
