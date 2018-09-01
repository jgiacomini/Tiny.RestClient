using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IStringResponseRequest
    /// </summary>
    public interface IHttpResponseRequest
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>Task of <see cref="HttpResponseMessage"/></returns>
        Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
