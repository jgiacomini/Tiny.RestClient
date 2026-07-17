using System.Text.Json.Serialization;

namespace Tiny.RestClient.ForTest.Api.Models
{
    public class PascalResponse
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("ResponseData")]
        public string ResponseData { get; set; }
    }
}
