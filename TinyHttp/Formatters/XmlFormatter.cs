using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tiny.Http
{
    /// <summary>
    /// Class TinyXmlSerializer.
    /// </summary>
    /// <seealso cref="Tiny.Http.IFormatter" />
    public class XmlFormatter : IFormatter
    {
        /// <inheritdoc/>
        public string DefaultMediaType => "application/xml";

        /// <inheritdoc/>
        public IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "application/xml";
                yield return "text/xml";
            }
        }

        /// <inheritdoc/>
        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <inheritdoc/>
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
