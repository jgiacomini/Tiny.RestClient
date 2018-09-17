using System;
using System.IO;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IRequest
    /// </summary>
    /// <seealso cref="IFormRequest" />
    /// <seealso cref="IExecutableRequest" />
    public interface IRequest : IExecutableRequest, IFormRequest
    {
        /// <summary>
        /// With timeout for current request
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The current request</returns>
        IRequest WithTimeout(TimeSpan timeout);

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="serializer">Override the default serializer setted on the client.</param>
        /// <returns>The current request</returns>
        IParameterRequest AddContent<TContent>(TContent content, IFormatter serializer = null);

        /// <summary>
        /// Add byte array as content of request
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="contentType">The Content type</param>
        /// <returns>The current request</returns>
        IParameterRequest AddByteArrayContent(byte[] byteArray, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds stream as content
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">The content type</param>
        /// <returns>The current request</returns>
        IParameterRequest AddStreamContent(Stream stream, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds file as content.
        /// </summary>
        /// <param name="file">The file to add as content of request.</param>
        /// <param name="contentType">The content type</param>
        /// <returns>The current request</returns>
        IParameterRequest AddFileContent(FileInfo file, string contentType);

        /// <summary>
        /// As a multipart data from request
        /// </summary>
        /// <param name="contentType">content type of the request (default value  = "multipart/form-data")</param>
        /// <returns>The current request</returns>
        IMultipartFromDataRequest AsMultiPartFromDataRequest(string contentType = "multipart/form-data");
    }
}