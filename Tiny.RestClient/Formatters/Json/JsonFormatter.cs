using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Tiny.RestClient.Json;

namespace Tiny.RestClient
{
    /// <summary>
    ///  Serializes and deserializes objects into and from the JSON format using the Newtonsoft.Json.JsonSerializer
    /// </summary>
    /// <seealso cref="IFormatter" />
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
        /// Gets the instance of JsonSerializer
        /// </summary>
        public JsonSerializer JsonSerializer { get; }

        /// <summary>
        /// Enable snake case for properties mapping. A property "PropertyName" will become "property_name"
        /// </summary>
        public void UseSnakeCase()
        {
            JsonSerializer.ContractResolver = new SnakeCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Enable camel case for properties mapping. A property "PropertyName" will become "propertyName"
        /// </summary>
        public void UseCamelCase()
        {
            JsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Enable kebab case (also named spinal case) for properties mapping. A property "PropertyName" will become "property-name"
        /// </summary>
        public void UseKebabCase()
        {
            JsonSerializer.ContractResolver = new KebabCasePropertyNamesContractResolver();
        }

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
        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            using (var sr = new StreamReader(stream, encoding))
            {
                using (var jtr = new JsonTextReader(sr))
                {
                    return JsonSerializer.Deserialize<T>(jtr);
                }
            }
        }

        /// <inheritdoc/>
        public string Serialize<T>(T data, Encoding encoding)
        {
            using (var stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture))
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = JsonSerializer.Formatting;
                    JsonSerializer.Serialize(jsonTextWriter, data, typeof(T));
                }

                return stringWriter.ToString();
            }
        }
    }
}
