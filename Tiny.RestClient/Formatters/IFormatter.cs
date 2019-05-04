using System.Collections.Generic;
using System.IO;
using System.Text;

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
        /// <returns>System.String.</returns>
        string Serialize<T>(T data, Encoding encoding);

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>An instance of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(Stream stream, Encoding encoding);
    }
}