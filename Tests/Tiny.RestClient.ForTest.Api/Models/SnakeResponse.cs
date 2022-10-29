using System.Text.Json.Serialization;

namespace Tiny.RestClient.ForTest.Api.Models
{
    public class SnakeResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("response_data")]
        public string ResponseData { get; set; }
    }
}
