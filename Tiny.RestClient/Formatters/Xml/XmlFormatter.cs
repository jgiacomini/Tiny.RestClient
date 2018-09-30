using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tiny.RestClient
{
    /// <summary>
    /// Class TinyXmlSerializer.
    /// </summary>
    /// <seealso cref="IFormatter" />
    public class XmlFormatter : IFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/>
        /// </summary>
        public XmlFormatter()
        {
            WriterSettings = new XmlWriterSettings
            {
                Indent = false
            };
        }

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

        /// <summary>
        /// Gets the instance of WriterSettings
        /// </summary>
        public XmlWriterSettings WriterSettings { get; }

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

            var serializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new DynamicEncodingStringWriter(encoding))
            {
                using (var writer = XmlWriter.Create(stringWriter, WriterSettings))
                {
                    serializer.Serialize(writer, data);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
