using System.Net.Http;

namespace Tiny.Http
{
    /// <summary>
    /// Interface IContentRequest
    /// </summary>
    /// <seealso cref="Tiny.Http.IExecutableRequest" />
    public interface IWithNoStandardResponse
    {
        /// <summary>
        /// Withes <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <returns><see cref="IHttpResponseRequest"/></returns>
        IHttpResponseRequest WithHttpResponse();

        /// <summary>
        /// Withes string response.
        /// </summary>
        /// <returns>IStringResponseRequest.</returns>
        IStringResponseRequest WithStringResponse();

        /// <summary>
        /// Withes the byte array response.
        /// </summary>
        /// <returns>IOctectStreamRequest.</returns>
        IByteArrayResponseRequest WithByteArrayResponse();

        /// <summary>
        /// Withes the stream response.
        /// </summary>
        /// <returns>The current request</returns>
        IStreamResponseRequest WithStreamResponse();
    }
}