using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IFormatter.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Gets the default media type used to send request.
        /// </summary>
        /// <value>The type of the media.</value>
        string DefaultMediaType { get; }

        /// <summary>
        /// Gets the supported media type by the serializer.
        /// </summary>
        /// <value>The type of the media.</value>
        IEnumerable<string> SupportedMediaTypes { get; }

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T">Type of data serialized.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>System.String.</returns>
        Task<string> SerializeAsync<T>(T data, Encoding encoding, CancellationToken cancellationToken)
            where T : class;

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An instance of type <typeparamref name="T"/>.</returns>
        ValueTask<T> DeserializeAsync<T>(Stream stream, Encoding encoding, CancellationToken cancellationToken);
    }
}