using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyXmlDeserializer.
    /// </summary>
    /// <seealso cref="Tiny.Http.IDeserializer" />
    public class TinyXmlDeserializer : IDeserializer
    {
        /// <summary>
        /// Gets the type of the media. ("application/xml")
        /// </summary>
        /// <value>The type of the media.</value>
        public string MediaType => "application/xml";

        /// <summary>
        /// Gets a value indicating whether this instance has media type.
        /// </summary>
        /// <value><c>true</c> if this instance has media type; otherwise, <c>false</c>.</value>
        public bool HasMediaType => true;

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>An instance of type <typeparamref name="T" /></returns>
        /// <exception cref="Tiny.Http.DeserializeException">Error during deserialization</exception>
        public T Deserialize<T>(Stream stream)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                string data = null;
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    data = reader.ReadToEnd();
                }

                throw new DeserializeException("Error during deserialization", ex, data);
            }
        }
    }
}
