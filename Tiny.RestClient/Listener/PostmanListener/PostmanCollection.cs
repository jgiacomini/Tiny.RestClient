using Newtonsoft.Json;
using System.Collections.Generic;
namespace Tiny.RestClient.PostMan
{
    internal class PostmanCollection
    {
        [JsonProperty(PropertyName = "info")]
        public Info Info { get; set; }

        [JsonProperty(PropertyName = "item")]
        public List<IItem> Items { get; set; }
    }

    internal class Info
    {
        [JsonProperty(PropertyName = "_postman_id")]
        public string PostmanId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }
    }

    internal class Folder : IItem
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "item")]
        public List<Item> Items { get; set; }
    }

    internal class Item : IItem
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "request")]
        public Request Request { get; set; }
    }

    internal interface IItem
    {
        string Name { get; set; }
    }

    internal class Request
    {
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "header")]
        public List<Header> Headers { get; set; }

        [JsonProperty(PropertyName = "body")]
        public Body Body { get; set; }

        [JsonProperty(PropertyName = "url")]
        public Url Url { get; set; }
    }

    internal class Body
    {
        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "raw")]
        public string Raw { get; set; }
    }

    internal class Url
    {
        [JsonProperty(PropertyName = "raw")]
        public string Raw { get; set; }

        [JsonProperty(PropertyName = "protocol")]
        public string Protocol { get; set; }
        [JsonProperty(PropertyName = "port")]
        public string Port { get; set; }

        [JsonProperty(PropertyName = "host")]
        public string[] Host { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string[] Path { get; set; }

        [JsonProperty(PropertyName = "query")]
        public List<Query> QueryParameters { get; set; }
    }

    internal class Query
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    internal class Header
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
