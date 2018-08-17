using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tiny.Http
{
    public class TinyXmlSerializer : ISerializer
    {
        public string MediaType => "application/xml";

        public bool HasMediaType => true;

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

    public class DynamicEncodingStringWriter : StringWriter
    {
        private readonly Encoding _encoding;
        public DynamicEncodingStringWriter(Encoding encoding)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding { get { return _encoding; } }
    }
}
