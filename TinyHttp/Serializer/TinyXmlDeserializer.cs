using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tiny.Http
{
    public class TinyXmlDeserializer : IDeserializer
    {
        public string MediaType => "application/xml";
        public bool HasMediaType => true;

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
