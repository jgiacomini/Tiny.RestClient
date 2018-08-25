using System.IO;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IRequest
    /// </summary>
    /// <seealso cref="Tiny.Http.IContentRequest" />
    /// <seealso cref="Tiny.Http.IFormRequest" />
    public interface IRequest : IContentRequest, IFormRequest
    {
        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="serializer">Override the default serializer setted on the client.</param>
        /// <returns>The current request</returns>
        IContentRequest AddContent<TContent>(TContent content, ISerializer serializer = null);

        /// <summary>
        /// Adds the content of the byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="contentType">The Content type</param>
        /// <returns>The current request</returns>
        IContentRequest AddByteArrayContent(byte[] byteArray, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds the content of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">The Content type</param>
        /// <returns>The current request</returns>
        IContentRequest AddStreamContent(Stream stream, string contentType = "application/octet-stream");

        /// <summary>
        /// As a multipart data from request
        /// </summary>
        /// <param name="contentType">content type of the request (default value  = "multipart/form-data")</param>
        /// <returns>The current request</returns>
        IMultiPartFromDataRequest AsMultiPartFromDataRequest(string contentType = "multipart/form-data");
    }
}