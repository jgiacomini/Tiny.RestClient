using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        /// Initializes a new instance of the <see cref="XmlFormatter"/>.
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
        /// Gets the instance of WriterSettings.
        /// </summary>
        public XmlWriterSettings WriterSettings { get; }

        /// <inheritdoc/>
        public ValueTask<T> DeserializeAsync<T>(Stream stream, Encoding encoding, CancellationToken cancellationToken)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                var serializer = new XmlSerializer(typeof(T));
                return ValueTask.FromResult((T)serializer.Deserialize(reader));
            }
        }

        /// <inheritdoc/>
        public Task<string> SerializeAsync<T>(T data, Encoding encoding, CancellationToken cancellationToken)
            where T : class
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
                    return Task.FromResult(stringWriter.ToString());
                }
            }
        }
    }
}
