using System.Text;

namespace Tiny.Http
{
    /// <summary>
    /// Interface ISerializer
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Gets a value indicating whether this instance has media type.
        /// </summary>
        /// <value><c>true</c> if this instance has media type; otherwise, <c>false</c>.</value>
        bool HasMediaType { get; }

        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        string MediaType { get; }

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T">Type of data serialized</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        string Serialize<T>(T data, Encoding encoding);
    }
}