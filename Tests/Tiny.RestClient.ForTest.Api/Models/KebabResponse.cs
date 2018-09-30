using Newtonsoft.Json;
using System;

namespace Tiny.RestClient.Tests.Models
{
    public class KebabResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("response-data")]
        public string ResponseData { get; set; }
    }
}
