using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tiny.RestClient.PostMan
{
    internal class PostmanCollection
    {
        [JsonPropertyName("info")]
        public Info Info { get; set; }

        [JsonPropertyName("item")]
        public List<IItem> Items { get; set; }
    }

    internal class Info
    {
        [JsonPropertyName("_postman_id")]
        public string PostmanId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("schema")]
        public string Schema { get; set; }
    }

    internal class Folder : IItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("item")]
        public List<Item> Items { get; set; }
    }

    internal class Item : IItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("request")]
        public Request Request { get; set; }
    }

    internal interface IItem
    {
        string Name { get; set; }
    }

    internal class Request
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("header")]
        public List<Header> Headers { get; set; }

        [JsonPropertyName("body")]
        public Body Body { get; set; }

        [JsonPropertyName("url")]
        public Url Url { get; set; }
    }

    internal class Body
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }

    internal class Url
    {
        [JsonPropertyName("raw")]
        public string Raw { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
        [JsonPropertyName("port")]
        public string Port { get; set; }

        [JsonPropertyName("host")]
        public string[] Host { get; set; }

        [JsonPropertyName("path")]
        public string[] Path { get; set; }

        [JsonPropertyName("query")]
        public List<Query> QueryParameters { get; set; }
    }

    internal class Query
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    internal class Header
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
