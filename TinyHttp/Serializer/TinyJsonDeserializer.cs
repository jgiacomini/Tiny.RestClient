using Newtonsoft.Json;
using System;

namespace TinyHttp
{
    public class TinyJsonDeserializer : IDeserializer
    {
        public T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                throw new DeserializeException("Error during deserialization", ex, data);
            }
        }
    }
}
