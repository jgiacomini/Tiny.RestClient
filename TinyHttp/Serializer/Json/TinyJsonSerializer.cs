using Newtonsoft.Json;
using System.Text;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyJsonSerializer.
    /// </summary>
    /// <seealso cref="Tiny.Http.ISerializer" />
    public class TinyJsonSerializer : ISerializer
    {
        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        public string MediaType => "application/json";

        /// <summary>
        /// Gets a value indicating whether this instance has media type.
        /// </summary>
        /// <value><c>true</c> if this instance has media type; otherwise, <c>false</c>.</value>
        public bool HasMediaType => true;

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T">Type of data serialized</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public string Serialize<T>(T data, Encoding encoding)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
