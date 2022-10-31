using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tiny.RestClient.ForTest.Api.Models
{
    public class CamelResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("responseData")]
        public string ResponseData { get; set; }
    }
}
