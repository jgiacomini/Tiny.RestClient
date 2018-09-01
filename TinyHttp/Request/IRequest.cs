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
        /// Fill header of response
        /// </summary>
        /// <param name="headers">Header filled after execute method</param>
        /// <returns>The current request</returns>
        IRequest FillResponseHeaders(out Headers headers);

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddHeader(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, string value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, bool value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, bool? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, int value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, int? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, uint value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, uint? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, double value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, double? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, decimal value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, decimal? value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, float value);

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current request</returns>
        IRequest AddQueryParameter(string key, float? value);

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="serializer">Override the default serializer setted on the client.</param>
        /// <returns>The current request</returns>
        IContentRequest AddContent<TContent>(TContent content, IFormatter serializer = null);

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