using System;
using System.Text.Json.Serialization;

namespace Tiny.RestClient.ForTest.Api.Models
{
    public class KebabResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("response-data")]
        public string ResponseData { get; set; }
    }
}
