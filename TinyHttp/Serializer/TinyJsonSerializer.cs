using Newtonsoft.Json;

namespace TinyHttp
{
    public class TinyJsonSerializer : ISerializer
    {
        public string MediaType
        {
            get
            {
                return "application/json";
            }
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
