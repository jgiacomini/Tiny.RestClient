using System.IO;
using System.Threading.Tasks;
namespace Tiny.Http
{
    /// <summary>
    /// Interface IDeserializer
    /// </summary>
    public interface IDeserializer
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
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>An instance of type <typeparamref name="T"/></returns>
        T Deserialize<T>(Stream stream);
    }
}