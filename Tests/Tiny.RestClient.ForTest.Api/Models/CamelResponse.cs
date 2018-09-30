using Newtonsoft.Json;

namespace Tiny.RestClient.Tests.Models
{
    public class CamelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("responseData")]
        public string ResponseData { get; set; }
    }
}
