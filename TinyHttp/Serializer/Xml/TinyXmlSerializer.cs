using System.IO;
using System.Xml.Serialization;

namespace Tiny.Http
{
    public class TinyXmlSerializer : ISerializer
    {
        public string MediaType => "application/xml";

        public bool HasMediaType => true;

        public string Serialize<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }
    }
}
