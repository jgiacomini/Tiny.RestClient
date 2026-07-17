using System;
using System.IO;

namespace Tiny.RestClient
{
    /// <summary>
    /// Represents a request whose body has not been set yet. Add a body (serialized content, string, stream,
    /// byte array or file), switch to a multipart request, or execute it directly.
    /// </summary>
    /// <seealso cref="IFormRequest" />
    /// <seealso cref="IExecutableRequest" />
    public interface IRequest : IExecutableRequest, IFormRequest
    {
        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="serializer">Override the default serializer setted on the client.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddContent<TContent>(TContent content, IFormatter serializer = null)
            where TContent : class;

        /// <summary>
        /// Add byte array as content of request.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="contentType">The Content type.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddByteArrayContent(byte[] byteArray, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds stream as content.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddStreamContent(Stream stream, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds string as content (without apply any serialization).
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddStringContent(string content, string contentType = "text/plain");

        /// <summary>
        /// Adds file as content.
        /// </summary>
        /// <param name="file">The file to add as content of request.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The current request.</returns>
        IParameterRequest AddFileContent(FileInfo file, string contentType);

        /// <summary>
        /// Switches the request to a multipart/form-data request, allowing several parts (objects, byte arrays, streams, strings, files) to be added.
        /// </summary>
        /// <param name="contentType">Content type of the request (default value = "multipart/form-data").</param>
        /// <returns>The current request as a multipart request.</returns>
        /// <example>
        /// <code>
        /// await client.PostRequest("MultiPart/Test")
        ///     .AsMultiPartFromDataRequest()
        ///     .AddContent&lt;City&gt;(city1, "city1", "city1.json")
        ///     .AddContent&lt;City&gt;(city2, "city2", "city2.json")
        ///     .ExecuteAsync();
        /// </code>
        /// </example>
        IMultipartFromDataRequest AsMultiPartFromDataRequest(string contentType = "multipart/form-data");
    }
}