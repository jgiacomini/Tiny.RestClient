using Newtonsoft.Json;

namespace Tiny.RestClient.Tests.Models
{
    public class SnakeResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("response_data")]
        public string ResponseData { get; set; }
    }
}
