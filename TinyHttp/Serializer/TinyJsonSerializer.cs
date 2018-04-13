using Newtonsoft.Json;

namespace TinyHttp
{
    public class TinyJsonSerializer : ISerializer
    {
        public string MediaType => "application/json";

        public bool HasMediaType => true;

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
