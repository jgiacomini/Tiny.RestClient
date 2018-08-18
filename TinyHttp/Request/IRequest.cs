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
        /// <returns>The current request</returns>
        IContentRequest AddContent<TContent>(TContent content);

        /// <summary>
        /// Adds the content of the byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>The current request</returns>
        IContentRequest AddByteArrayContent(byte[] byteArray);

        /// <summary>
        /// Adds the content of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The current request</returns>
        IContentRequest AddStreamContent(Stream stream);
    }
}