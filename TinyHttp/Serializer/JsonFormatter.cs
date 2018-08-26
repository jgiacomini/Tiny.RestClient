using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tiny.Http
{
    /// <summary>
    ///  Serializes and deserializes objects into and from the JSON format using the Newtonsoft.Json.JsonSerializer
    /// </summary>
    /// <seealso cref="Tiny.Http.IFormatter" />
    public class JsonFormatter : IFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/>
        /// </summary>
        public JsonFormatter()
        {
            JsonSerializer = new JsonSerializer();
        }

        /// <summary>
        /// Get the instance of JsonSerializer
        /// </summary>
        public JsonSerializer JsonSerializer { get; }

        /// <inheritdoc/>
        public string DefaultMediaType => "application/json";

        /// <inheritdoc/>
        public IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "application/json";
                yield return "text/json";
                yield return "text/x-json";
                yield return "text/javascript";
            }
        }

        /// <inheritdoc/>
        public T Deserialize<T>(Stream stream)
        {
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    using (var jtr = new JsonTextReader(sr))
                    {
                        return JsonSerializer.Deserialize<T>(jtr);
                    }
                }
            }
            catch (Exception ex)
            {
                string data = null;
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    data = reader.ReadToEnd();
                }

                throw new DeserializeException("Error during deserialization", ex, data);
            }
        }

        /// <inheritdoc/>
        public string Serialize<T>(T data, Encoding encoding)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
