using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IExecutableRequest
    /// </summary>
    public interface IExecutableRequest
    {
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of TResukt</returns>
        Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="formatter">Allow to override the formatter use for the deserialization.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task of TResukt</returns>
        Task<TResult> ExecuteAsync<TResult>(IFormatter formatter = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}