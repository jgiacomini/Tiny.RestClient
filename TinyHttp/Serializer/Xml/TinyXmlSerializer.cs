using System.Text;
using System.Xml.Serialization;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyXmlSerializer.
    /// </summary>
    /// <seealso cref="Tiny.Http.ISerializer" />
    public class TinyXmlSerializer : ISerializer
    {
        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        public string MediaType => "application/xml";

        /// <summary>
        /// Gets a value indicating whether this instance has media type.
        /// </summary>
        /// <value><c>true</c> if this instance has media type; otherwise, <c>false</c>.</value>
        public bool HasMediaType => true;

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T">type of the data to serialize</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The serialized data.</returns>
        public string Serialize<T>(T data, Encoding encoding)
        {
            if (data == default)
            {
                return null;
            }

            var serializer = new XmlSerializer(data.GetType());
            using (var stringWriter = new DynamicEncodingStringWriter(encoding))
            {
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }
    }
}
