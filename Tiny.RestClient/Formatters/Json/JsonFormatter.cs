using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tiny.RestClient
{
    /// <summary>
    ///  Serializes and deserializes objects into and from the JSON format using the System.Text.Json.
    /// </summary>
    /// <seealso cref="IFormatter" />
    public class JsonFormatter : IFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/>.
        /// </summary>
        public JsonFormatter()
        {
            JsonSerializerOptions = new JsonSerializerOptions();
        }

        /// <summary>
        /// Gets the instance of JsonSerializerOptions.
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        /// Enable camel case for properties mapping. A property "PropertyName" will become "propertyName".
        /// </summary>
        public void UseCamelCase()
        {
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        /// <summary>
        /// Enable kebab case (also named spinal case) for properties mapping. A property "PropertyName" will become "property-name".
        /// </summary>
        public void UseKebabCase()
        {
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicies.KebabCase;
        }

        /// <summary>
        /// Enable snake case for properties mapping. A property "PropertyName" will become "property_name".
        /// </summary>
        public void UseSnakeCase()
        {
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicies.SnakeCase;
        }

        /// <inheritdoc/>
        public string DefaultMediaType => "application/json";

        /// <inheritdoc/>
        public IEnumerable<string> SupportedMediaTypes
        {
            get
            {
                yield return "application/json";
                yield return "application/json-patch+json";
                yield return "application/*+json";
                yield return "text/json";
                yield return "text/x-json";
                yield return "text/javascript";
            }
        }

        /// <inheritdoc/>
        public ValueTask<T> DeserializeAsync<T>(Stream stream, Encoding encoding, CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<string> SerializeAsync<T>(T data, Encoding encoding, CancellationToken cancellationToken)
            where T : class
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, data, JsonSerializerOptions, cancellationToken);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
        }
    }
}
