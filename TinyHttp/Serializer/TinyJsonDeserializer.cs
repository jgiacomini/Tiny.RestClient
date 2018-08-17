using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Tiny.Http
{
    public class TinyJsonDeserializer : IDeserializer
    {
        public string MediaType => "application/json";
        public bool HasMediaType => true;

        public T Deserialize<T>(Stream stream)
        {
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    using (var jtr = new JsonTextReader(sr))
                    {
                        var js = new JsonSerializer();

                        var searchResult = js.Deserialize<T>(jtr);
                        return searchResult;
                    }
                }
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
